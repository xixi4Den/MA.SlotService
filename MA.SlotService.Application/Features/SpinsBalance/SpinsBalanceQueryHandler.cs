using MA.SlotService.Application.Abstractions;
using MediatR;

namespace MA.SlotService.Application.Features.SpinsBalance;

public class SpinsBalanceQueryHandler: IRequestHandler<SpinsBalanceQuery, long>
{
    private readonly ISpinsBalanceRepository _repository;

    public SpinsBalanceQueryHandler(ISpinsBalanceRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<long> Handle(SpinsBalanceQuery request, CancellationToken cancellationToken)
    {
        long result = await _repository.Get(request.UserId);

        return result;
    }
}