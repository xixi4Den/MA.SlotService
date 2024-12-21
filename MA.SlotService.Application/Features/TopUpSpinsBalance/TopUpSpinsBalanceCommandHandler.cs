using MA.SlotService.Application.Abstractions;
using MediatR;

namespace MA.SlotService.Application.Features.TopUpSpinsBalance;

public class TopUpSpinsBalanceCommandHandler: IRequestHandler<TopUpSpinsBalanceCommand, TopUpSpinsBalanceCommandResult>
{
    private readonly ISpinsBalanceRepository _spinsBalanceRepository;

    public TopUpSpinsBalanceCommandHandler(ISpinsBalanceRepository spinsBalanceRepository)
    {
        _spinsBalanceRepository = spinsBalanceRepository;
    }
    
    public async Task<TopUpSpinsBalanceCommandResult> Handle(TopUpSpinsBalanceCommand request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
            return TopUpSpinsBalanceCommandResult.ValidationError("Invalid amount");
        
        var newBalance = await _spinsBalanceRepository.Add(request.UserId, request.Amount);

        return TopUpSpinsBalanceCommandResult.Success(newBalance);
    }
}