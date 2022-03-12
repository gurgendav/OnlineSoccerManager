namespace SoccerManager.Application.Exceptions;

public class NotEnoughBudgetException : Exception
{
    public NotEnoughBudgetException()
    {
    }
    
    public NotEnoughBudgetException(string message) : base(message)
    {
    }
    
    public NotEnoughBudgetException(string message, Exception innerException) : base(message, innerException)
    {
    }
}