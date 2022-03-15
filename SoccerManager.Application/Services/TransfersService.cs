using Microsoft.EntityFrameworkCore;
using SoccerManager.Application.Entities;
using SoccerManager.Application.Exceptions;
using SoccerManager.Application.Interfaces;

namespace SoccerManager.Application.Services;

public class TransfersService : ITransfersService
{
    private const int MinPlayerMarketValueIncreasePercent = 10;
    private const int MaxPlayerMarketValueIncreasePercent = 100;

    private readonly IApplicationDbContext _applicationDbContext;
    private readonly INumberGenerator _numberGenerator;
    private readonly IUserIdAccessor _userIdAccessor;

    public TransfersService(IApplicationDbContext applicationDbContext,
        INumberGenerator numberGenerator,
        IUserIdAccessor userIdAccessor)
    {
        _applicationDbContext = applicationDbContext;
        _numberGenerator = numberGenerator;
        _userIdAccessor = userIdAccessor;
    }

    public async Task<List<TransferItem>> GetAll(int skip = 0, int take = 20) => await _applicationDbContext.Transfers
        .Include(t => t.SoccerPlayer)
        .OrderBy(t => t.Id)
        .Skip(skip)
        .Take(take)
        .ToListAsync();

    public async Task<TransferItem> GetById(int transferId) => await _applicationDbContext.Transfers
        .Include(t => t.SoccerPlayer)
        .FirstOrDefaultAsync(p => p.Id == transferId);

    public async Task<TransferItem> Create(int soccerPlayerId, int price)
    {
        var player = await _applicationDbContext.SoccerPlayers
            .Include(p => p.SoccerTeam)
            .FirstAsync(p => p.Id == soccerPlayerId);

        if (player.SoccerTeam.UserId != _userIdAccessor.GetCurrentUserId())
        {
            throw new UnauthorizedException("You are not allowed to transfer this player");
        }

        if (player.TransferItemId != null)
        {
            throw new AlreadyInTransferException("This player is already in transfers list");
        }

        var transfer = new TransferItem
        {
            SoccerPlayer = player,
            Price = price
        };

        _applicationDbContext.Transfers.Add(transfer);
        await _applicationDbContext.SaveChangesAsync();

        return transfer;
    }

    public async Task Delete(int transferId)
    {
        var transferItem = await _applicationDbContext.Transfers
            .Include(t => t.SoccerPlayer)
            .ThenInclude(p => p.SoccerTeam)
            .FirstAsync(p => p.Id == transferId);

        if (transferItem.SoccerPlayer.SoccerTeam.UserId != _userIdAccessor.GetCurrentUserId())
        {
            throw new UnauthorizedException("You are not allowed to delete this transfer");
        }

        _applicationDbContext.Transfers.Remove(transferItem);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task ExecuteTransfer(int transferId)
    {
        var transferItem = await _applicationDbContext.Transfers
            .Include(t => t.SoccerPlayer)
            .ThenInclude(p => p.SoccerTeam)
            .FirstAsync(p => p.Id == transferId);

        if (transferItem.SoccerPlayer.SoccerTeam.UserId == _userIdAccessor.GetCurrentUserId())
        {
            throw new UnauthorizedException("You are not allowed to execute this transfer");
        }

        var userId = _userIdAccessor.GetCurrentUserId();
        var team = await _applicationDbContext.SoccerTeams.FirstAsync(t => t.UserId == userId);

        var soccerPlayer = transferItem.SoccerPlayer;

        if (team.Budget < transferItem.Price)
        {
            throw new NotEnoughBudgetException("You don't have enough budget to execute this transfer");
        }

        soccerPlayer.SoccerTeam.Budget += transferItem.Price;
        team.Budget -= transferItem.Price;
        soccerPlayer.SoccerTeam = team;

        var increasePercent =
            _numberGenerator.GenerateInt(MinPlayerMarketValueIncreasePercent, MaxPlayerMarketValueIncreasePercent);
        soccerPlayer.MarketValue = transferItem.Price + transferItem.Price * increasePercent / 100;

        _applicationDbContext.Transfers.Remove(transferItem);
        await _applicationDbContext.SaveChangesAsync();
    }
}