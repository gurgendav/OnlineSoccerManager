namespace SoccerManager.WebAPI.Models;

public class TransferItemResponse
{
    public int Id { get; set; }
    
    public SoccerPlayerResponse SoccerPlayer { get; set; }
    
    public int Price { get; set; }
}