using MediatR;

namespace MA.SlotService.Application.Features.TopUpSpinsBalance;

public record TopUpSpinsBalanceCommand(int UserId, long Amount): IRequest<TopUpSpinsBalanceCommandResult>;