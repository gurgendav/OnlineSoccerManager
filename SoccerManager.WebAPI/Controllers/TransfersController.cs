using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Interfaces;
using SoccerManager.WebAPI.Models;

namespace SoccerManager.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransfersController : ControllerBase
{
    private readonly ITransfersService _transfersService;
    private readonly IMapper _mapper;

    public TransfersController(ITransfersService transfersService, IMapper mapper)
    {
        _transfersService = transfersService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(int skip = 0, int take = 20)
    {
        var transfers = await _transfersService.GetAll(skip, take);
        
        return Ok(_mapper.Map<List<TransferItemResponse>>(transfers));
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var transferItem = await  _transfersService.GetById(id);
        if (transferItem == null)
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<TransferItemResponse>(transferItem));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTransfer([FromBody] CreateTransferItemRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var transferItem = await _transfersService.Create(request.PlayerId, request.Price);
        return Ok(_mapper.Map<TransferItemResponse>(transferItem));
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTransfer(int id)
    {
        await _transfersService.Delete(id);
        return NoContent();
    }
    
    [HttpPost("{id:int}/execute")]
    public async Task<IActionResult> ExecuteTransfer(int id)
    {
        await _transfersService.ExecuteTransfer(id);
        return Ok();
    }
}