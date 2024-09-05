using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record NewSubscriberEvent: BusEvent<NewSubscriberEventContent>
{
    public NewSubscriberEvent():base("new-subscriber") { }
}

public record NewSubscriberEventContent
{
    public required bool IsGift { get; set; }
    public required string Tier { get; set; }
    public required string UserId { get; set; }
    public required string UserName { get; set; }
}
