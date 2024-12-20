using MediatR;

namespace MA.SlotService.Application.Features.StartSpin;

public record StartSpinCommand(int UserId): IRequest<StartSpinCommandResult>;