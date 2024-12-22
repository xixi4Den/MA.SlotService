namespace MA.SlotService.Application.Abstractions;

public interface ISpinsBalanceRepository
{
    Task<long> GetAsync(int userId);
    Task<long> AddAsync(int userId, long amount, string referenceId);
    Task<SpinsBalanceDeductionResult> TryDeductAsync(int userId);
    Task<bool> ContainsReferenceIdAsync(string referenceId);
}

public record struct SpinsBalanceDeductionResult(bool IsSuccess, long NewBalance);