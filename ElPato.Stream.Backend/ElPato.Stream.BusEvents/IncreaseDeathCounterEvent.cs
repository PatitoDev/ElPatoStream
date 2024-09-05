using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record IncreaseDeathCounterEvent: BusEvent<IncreaseDeathCounterContent>
{
    public IncreaseDeathCounterEvent() : base("increase-death-counter") { }
}

public record IncreaseDeathCounterContent(int Amount);
