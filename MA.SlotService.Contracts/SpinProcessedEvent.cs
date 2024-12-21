namespace MA.SlotService.Contracts;

public class SpinProcessedEvent
{
    public int UserId { get; set; }
    public Guid SpinId { get; set; }
    public int[] Result { get; set; }
}