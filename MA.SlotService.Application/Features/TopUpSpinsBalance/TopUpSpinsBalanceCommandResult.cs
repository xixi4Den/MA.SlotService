using MA.SlotService.Application.Exceptions;

namespace MA.SlotService.Application.Features.TopUpSpinsBalance;

public class TopUpSpinsBalanceCommandResult
{
    private TopUpSpinsBalanceCommandResult()
    {
    }
    
    public bool IsSuccessful => Balance is not null;
    
    public bool IsDuplicate { get; private init; }
    
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
    
    public static TopUpSpinsBalanceCommandResult DuplicateError(string referenceId)
    {
        return new TopUpSpinsBalanceCommandResult {IsDuplicate = true, Error = $"A transaction with given ReferenceId has already been processed - ${referenceId}"};
    }

    public void ValidateThrow()
    {
        if (!IsSuccessful)
            throw new BusinessException(Error!);
    }
}