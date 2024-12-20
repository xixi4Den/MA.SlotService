namespace MA.SlotService.Application.Features.StartSpin;

public class StartSpinCommandResult
{
    private StartSpinCommandResult()
    {
    }

    public bool IsSuccessful => Data is not null;
    
    public long Balance { get; private init; }
    
    public SpinResult? Data { get; private init; }
    
    public string? Error { get; private init; }

    public static StartSpinCommandResult Success(SpinResult result, long balance) => new() {Data = result, Balance = balance};

    public static StartSpinCommandResult InsufficientBalance() => new() {Error = "Insufficient balance", Balance = 0};
}