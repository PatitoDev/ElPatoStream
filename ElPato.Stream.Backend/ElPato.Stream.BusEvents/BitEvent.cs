using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record BitEvent: BusEvent<BitEventContent>
{
    public BitEvent(): base("bit-event") { }
}

public record BitEventContent
{
    public required int Bits { get; set; }
    public required bool IsAnonymous { get; set; }
    public required string Message { get; set; }
    public required string? UserId { get; set; }
    public required string? UserLogin { get; set; }
    public required string? UserName { get; set; }
}
