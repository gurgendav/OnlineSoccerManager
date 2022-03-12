using System.ComponentModel.DataAnnotations;

namespace SoccerManager.WebAPI.Models;

public class CreateTransferItemRequest
{
    [Required]
    public int PlayerId { get; set; }
    
    [Required]
    public int Price { get; set; }
}