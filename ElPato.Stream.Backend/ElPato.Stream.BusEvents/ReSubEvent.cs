using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record ReSubEvent: BusEvent<ReSubEventContent>
{
    public ReSubEvent(): base("resub") { }
}

public record ReSubEventContent
{
    public required string UserId { get; set; }
    public required string UserLogin { get; set; }
    public required string UserName { get; set; }
    public required string Tier { get; set; }
    public required int TotalMonths { get; set; }
    public required int? Streak { get; set; }
    public required string Message { get; set; }
    public required int Duration { get; set; }
}
