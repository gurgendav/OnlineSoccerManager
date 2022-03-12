namespace SoccerManager.Application.Exceptions;

public class AlreadyInTransferException : Exception
{
    public AlreadyInTransferException()
    {
    }
    
    public AlreadyInTransferException(string message) : base(message)
    {
    }
    
    public AlreadyInTransferException(string message, Exception innerException) : base(message, innerException)
    {
    }
}