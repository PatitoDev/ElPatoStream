using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record DeathResetEvent: BusEvent<object>
{
    public DeathResetEvent():base("reset-death-counter") {
        Content = null;
    }
}
