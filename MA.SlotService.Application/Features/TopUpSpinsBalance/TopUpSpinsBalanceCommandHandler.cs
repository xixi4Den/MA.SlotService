using MA.SlotService.Application.Abstractions;
using MediatR;

namespace MA.SlotService.Application.Features.TopUpSpinsBalance;

public class TopUpSpinsBalanceCommandHandler(ISpinsBalanceRepository spinsBalanceRepository)
    : IRequestHandler<TopUpSpinsBalanceCommand, TopUpSpinsBalanceCommandResult>
{
    public async Task<TopUpSpinsBalanceCommandResult> Handle(TopUpSpinsBalanceCommand request, CancellationToken cancellationToken)
    {
        if (await spinsBalanceRepository.ContainsReferenceIdAsync(request.ReferenceId))
            return TopUpSpinsBalanceCommandResult.DuplicateError(request.ReferenceId);
        
        if (request.Amount <= 0)
            return TopUpSpinsBalanceCommandResult.ValidationError("Invalid amount");
        
        var newBalance = await spinsBalanceRepository.AddAsync(request.UserId, request.Amount, request.ReferenceId);

        return TopUpSpinsBalanceCommandResult.Success(newBalance);
    }
}