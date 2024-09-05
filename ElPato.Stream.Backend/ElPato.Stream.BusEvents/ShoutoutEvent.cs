using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record ShoutoutEvent: BusEvent<ShoutoutEventContent>
{
    public ShoutoutEvent():base("channel-shoutout-create") { }
}

public record ShoutoutEventContent {
    public required string UserId { get; set; }
    public required string UserName { get; set; }
    public required string GameName { get; set; }
    public required string Title { get; set; }
    public required IEnumerable<string> Tags { get; set; }
    public required string ProfileImgAsBase64 { get; set; }
};
