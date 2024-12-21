namespace MA.SlotService.Application.Features.TopUpSpinsBalance;

public class TopUpSpinsBalanceCommandResult
{
    private TopUpSpinsBalanceCommandResult()
    {
    }
    
    public bool IsSuccessful => Balance is not null;
    
    public long? Balance { get; private init; }
    
    public string? Error { get; private set; }

    public static TopUpSpinsBalanceCommandResult Success(long balance)
    {
        return new TopUpSpinsBalanceCommandResult {Balance = balance};
    }
    
    public static TopUpSpinsBalanceCommandResult ValidationError(string error)
    {
        return new TopUpSpinsBalanceCommandResult {Error = error};
    }
}