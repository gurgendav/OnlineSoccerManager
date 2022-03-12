using SoccerManager.Application.Entities;

namespace SoccerManager.Application.Interfaces;

public interface ITransfersService
{
    Task<List<TransferItem>> GetAll(int skip = 0, int take = 20);
    
    Task<TransferItem> GetById(int transferId);
    
    Task<TransferItem> Create(int soccerPlayerId, int price);
    
    Task Delete(int transferId);
    
    Task ExecuteTransfer(int transferId); 
}