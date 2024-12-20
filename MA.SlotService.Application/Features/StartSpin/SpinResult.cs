namespace MA.SlotService.Application.Features.StartSpin;

public record SpinResult
{
    public required Guid SpinId { get; set; }
    
    public required byte[] Result { get; set; }
}