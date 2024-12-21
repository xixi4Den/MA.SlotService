namespace MA.SlotService.Application.Abstractions;

public interface ISpinsBalanceRepository
{
    Task<long> Get(int userId);
    Task<long> Add(int userId, long amount);
    Task<SpinsBalanceDeductionResult> TryDeduct(int userId);
}

public record struct SpinsBalanceDeductionResult(bool IsSuccess, long NewBalance);