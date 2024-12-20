namespace MA.SlotService.Api.Contracts;

public class SpinResultResponse
{
    public required Guid SpinId { get; set; }
    
    public required byte[] Result { get; set; }
    
    public required long Balance { get; init; }
}