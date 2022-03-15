using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using SoccerManager.Application.Entities;
using SoccerManager.Application.Exceptions;
using SoccerManager.Application.Interfaces;
using SoccerManager.Application.Services;
using SoccerManager.Infrastructure.Persistence;
using Xunit;

namespace SoccerManager.Application.Tests.Services;

public class TransfersServiceTests
{
    private readonly TransfersService _sut;
    
    private readonly IUserIdAccessor _userIdAccessor;
    private readonly INumberGenerator _numberGenerator;
    private readonly ApplicationDbContext _dbContext;

    public TransfersServiceTests()
    {
        var serviceCollection = new ServiceCollection();

        _userIdAccessor = Substitute.For<IUserIdAccessor>();
        _numberGenerator = Substitute.For<INumberGenerator>();
        
        serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("SoccerManager"));

        serviceCollection.AddScoped<IApplicationDbContext>(p => p.GetService<ApplicationDbContext>());

        serviceCollection.AddTransient(_ => _userIdAccessor);
        serviceCollection.AddTransient(_ => _numberGenerator);
        serviceCollection.AddTransient<TransfersService>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        _dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        _dbContext.Database.EnsureCreated();
        
        _sut = serviceProvider.GetService<TransfersService>();
    }

    private ApplicationUser CreateUserWithBudget(int budget, params SoccerPlayer[] players)
    {
        var fixture = new Fixture();

        var user = new ApplicationUser
        {
            Id = fixture.Create<string>(),
            SoccerTeam = new SoccerTeam
            {
                Budget = budget,
                Country = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                Players = new List<SoccerPlayer>(players)
            }
        };
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        return user;
    }
    
    private static SoccerPlayer CreatePlayer() => new Fixture().Build<SoccerPlayer>()
        .Without(p => p.TransferItem)
        .Without(p => p.SoccerTeam)
        .Create();

    [Fact]
    public async Task Execute_ThrowsException_WhenNotEnoughBudget()
    {
        // Arrange
        var player = CreatePlayer();
        CreateUserWithBudget(budget: 1_000_000, player);
        var userWithLowBudget = CreateUserWithBudget(budget: 0);

        var transferItem = new TransferItem
        {
            Price = 100,
            SoccerPlayerId = player.Id,
            SoccerPlayer = player,
        };
        _dbContext.Transfers.Add(transferItem);
        await _dbContext.SaveChangesAsync();

        _userIdAccessor.GetCurrentUserId().Returns(userWithLowBudget.Id);

        // Act
        var action = async () => await _sut.ExecuteTransfer(transferItem.Id);

        // Assert
        await action.Should().ThrowAsync<NotEnoughBudgetException>();
    }
    
    [Fact]
    public async Task Execute_ThrowsException_WhenSameUser()
    {
        // Arrange
        var player = CreatePlayer();
        var user = CreateUserWithBudget(budget: 10_000_000, player);

        var transferItem = new TransferItem
        {
            Price = 100,
            SoccerPlayerId = player.Id,
            SoccerPlayer = player,
        };
        
        _dbContext.Transfers.Add(transferItem);
        await _dbContext.SaveChangesAsync();

        _userIdAccessor.GetCurrentUserId().Returns(user.Id);

        // Act
        var action = async () => await _sut.ExecuteTransfer(transferItem.Id);

        // Assert
        await action.Should().ThrowAsync<UnauthorizedException>();
    }

    [Theory]
    [InlineData(30000, 50000, 10000, 40, 40000, 40000, 14000)]
    [InlineData(1000000, 1000000, 100, 10, 1000100, 999900, 110)]
    [InlineData(1000000, 1000000, 1000, 20, 1001000, 999000, 1200)]
    public async Task Execute_Should_MakeTransfer(int initialSellerBudget, int initialBuyerBudget, int transferPrice, 
        int marketValueIncreasePercent, int expectedSellerBudget, int expectedBuyerBudget, int expectedPlayerMarketValue)
    {
        // Arrange
        var player = CreatePlayer();
        var seller = CreateUserWithBudget(initialSellerBudget, player);
        var buyer = CreateUserWithBudget(initialBuyerBudget);
        
        var transferItem = new TransferItem
        {
            Price = transferPrice,
            SoccerPlayerId = player.Id,
            SoccerPlayer = player,
        };
        
        _dbContext.Transfers.Add(transferItem);
        await _dbContext.SaveChangesAsync();
        
        _userIdAccessor.GetCurrentUserId().Returns(buyer.Id);
        _numberGenerator.GenerateInt(Arg.Any<int>(), Arg.Any<int>()).Returns(marketValueIncreasePercent);

        // Act
        await _sut.ExecuteTransfer(transferItem.Id);

        // Assert
        var sellerBudget = (await _dbContext.Users.FirstAsync(u => u.Id == seller.Id)).SoccerTeam.Budget;
        sellerBudget.Should().Be(expectedSellerBudget);
        
        var buyerBudget = (await _dbContext.Users.FirstAsync(u => u.Id == buyer.Id)).SoccerTeam.Budget;
        buyerBudget.Should().Be(expectedBuyerBudget);
        
        var transfer = await _dbContext.Transfers.FirstOrDefaultAsync(t => t.Id == transferItem.Id);
        transfer.Should().BeNull();
        
        var transferredPlayer = await _dbContext.SoccerPlayers.FirstAsync(p => p.Id == player.Id);
        transferredPlayer.MarketValue.Should().Be(expectedPlayerMarketValue);
    }
    
}