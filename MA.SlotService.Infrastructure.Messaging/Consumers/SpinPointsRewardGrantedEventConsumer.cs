using System.Text.Json;
using MA.RewardService.Contracts;
using MA.SlotService.Application.Features.TopUpSpinsBalance;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MA.SlotService.Infrastructure.Messaging.Consumers;

// ReSharper disable once ClassNeverInstantiated.Global
public class SpinPointsRewardGrantedEventConsumer(
    ILogger<SpinPointsRewardGrantedEventConsumer> logger,
    IMediator mediator)
    : IConsumer<SpinPointsRewardGrantedEvent>
{
    public async Task Consume(ConsumeContext<SpinPointsRewardGrantedEvent> context)
    {
        var message = context.Message;
        logger.LogDebug("Received message {MessageId} of type {EventType}: {Message}", context.MessageId, nameof(SpinPointsRewardGrantedEvent), JsonSerializer.Serialize(message));
        
        var command = new TopUpSpinsBalanceCommand(message.UserId, message.Amount, $"reward:{message.RewardId}");
        var result = await mediator.Send(command);
        if (result.IsDuplicate)
        {
            logger.LogWarning("Message {MessageId} has been skipped because it's duplicate by referenceId", context.MessageId);
            return;
        }
        
        result.ValidateThrow();
        
        logger.LogDebug("Message {MessageId} has been processed", context.MessageId);
    }
}