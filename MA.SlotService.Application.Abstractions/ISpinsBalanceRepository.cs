namespace MA.SlotService.Application.Abstractions;

public interface ISpinsBalanceRepository
{
    Task<SpinsBalanceDeductionResult> TryDeductPoints(int userId);
}

public record struct SpinsBalanceDeductionResult(bool IsSuccess, long NewBalance);