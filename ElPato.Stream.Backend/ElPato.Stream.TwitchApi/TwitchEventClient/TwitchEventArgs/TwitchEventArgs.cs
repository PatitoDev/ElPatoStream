namespace ElPato.Stream.TwitchApi;

public class TwitchEventArgs<T>: EventArgs
{
    public required T Payload;
}
