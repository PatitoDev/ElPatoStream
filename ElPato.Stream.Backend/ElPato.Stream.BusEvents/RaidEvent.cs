using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record RaidEvent: BusEvent<RaidEventContent>
{
    public RaidEvent(): base("raid-event") { }
}

public record RaidEventContent {
    public required string Id { get; set; }
    public required string Login { get; set; }
    public required string Name { get; set; }
    public required int Viewers { get; set; }
}
