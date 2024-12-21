using MA.SlotService.Application.Abstractions;
using MA.SlotService.Contracts;
using MediatR;

namespace MA.SlotService.Application.Features.StartSpin;

public class StartSpinCommandHandler(
    ISpinResultGenerator spinResultGenerator,
    ISpinsBalanceRepository spinsBalanceRepository,
    IEventPublisher eventPublisher)
    : IRequestHandler<StartSpinCommand, StartSpinCommandResult>
{
    public async Task<StartSpinCommandResult> Handle(StartSpinCommand request, CancellationToken ct)
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

        await PublishSpinProcessedEventAsync(request.UserId, result, ct);

        return StartSpinCommandResult.Success(result, deductionResult.NewBalance);
    }

    private async Task PublishSpinProcessedEventAsync(int userId, SpinResult result, CancellationToken ct)
    {
        var spinProcessedEvent = new SpinProcessedEvent
        {
            UserId = userId,
            SpinId = result.SpinId,
            Result = result.Result
        };
        await eventPublisher.PublishAsync(spinProcessedEvent, ct);
    }
}