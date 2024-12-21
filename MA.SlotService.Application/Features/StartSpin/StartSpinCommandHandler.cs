using MA.SlotService.Application.Abstractions;
using MediatR;

namespace MA.SlotService.Application.Features.StartSpin;

public class StartSpinCommandHandler(
    ISpinResultGenerator spinResultGenerator,
    ISpinsBalanceRepository spinsBalanceRepository)
    : IRequestHandler<StartSpinCommand, StartSpinCommandResult>
{
    public async Task<StartSpinCommandResult> Handle(StartSpinCommand request, CancellationToken cancellationToken)
    {
        var deductionResult = await spinsBalanceRepository.TryDeduct(request.UserId);
        if (!deductionResult.IsSuccess)
        {
            return StartSpinCommandResult.InsufficientBalance();
        }

        var result = new SpinResult
        {
            SpinId = Guid.NewGuid(),
            Result = spinResultGenerator.Generate()
        };

        return StartSpinCommandResult.Success(result, deductionResult.NewBalance);
    }
}