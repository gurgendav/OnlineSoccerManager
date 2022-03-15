namespace SoccerManager.WebAPI.Models;

public class ErrorResponse
{
    public IEnumerable<string> Errors { get; set; }

    public ErrorResponse(IEnumerable<string> errors)
    {
        Errors = errors;
    }
}