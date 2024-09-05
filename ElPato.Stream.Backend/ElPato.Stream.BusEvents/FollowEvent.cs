using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record FollowEvent: BusEvent<FollowEventContent>
{
    public FollowEvent(): base("new-follower") { }
}

public record FollowEventContent
{
    public required string Id { get; set; }
    public required string Login { get; set; }
    public required string Name { get; set; }
}
