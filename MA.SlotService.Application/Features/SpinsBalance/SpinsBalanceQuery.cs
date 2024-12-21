using MediatR;

namespace MA.SlotService.Application.Features.SpinsBalance;

public record SpinsBalanceQuery(int UserId): IRequest<long>;