using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record SubGiftEvent: BusEvent<SubGiftEventContent>
{
    public SubGiftEvent():base("sub-gift") { }
}

public record SubGiftEventContent
{
    public required bool IsAnon { get; set; }
    public required string Tier { get; set; }
    public required string? UserId { get; set; }
    public required string? UserLogin { get; set; }
    public required string? UserName { get; set; }
    public required int Total { get; set; }
    public required int? CumulativeTotal { get; set; }
}
