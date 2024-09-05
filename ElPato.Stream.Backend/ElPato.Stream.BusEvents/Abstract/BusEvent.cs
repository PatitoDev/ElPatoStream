namespace ElPato.Stream.BusEvents.Base;

public abstract record BusEvent<T>
{
    public BusEvent(string Type)
    {
        this.Type = Type;
    }

    public string Type { get; private set; }
    public T? Content { get; set; }
}
