using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record VipRedeemEvent: BusEvent<VipRedeemEventContent>
{
    public VipRedeemEvent():base("vip-redeem-success") { }
}

public record VipRedeemEventContent
{
    public required int IncreasedTo { get; set; }
    public required string UserName { get; set; } 
    public required string UserId { get; set; }
}
