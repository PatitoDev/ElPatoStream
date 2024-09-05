namespace ElPato.Stream.TwitchApi;

public record ChatMessageEvent
{
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string ChatterUserId { get; set; }
    public required string ChatterUserLogin { get; set; }
    public required string ChatterUserName { get; set; }
    public required string MessageId { get; set; }
    public required ChatMessageContent Message { get; set; }
    public required string Color { get; set; }
    public required IEnumerable<Badge> Badges { get; set; }
    public required string MessageType { get; set; }
    public bool IsSubscriber => Badges.Any(b => b.SetId == "subscriber");
    public bool IsBroadcaster => Badges.Any(b => b.SetId == "broadcaster");
    public bool IsMod => Badges.Any(b => b.SetId == "moderator");
}

public record ChatMessageContent
{
    public required string Text { get; set; }
    public required IEnumerable<ChatFragment> Fragments { get; set; }
}

public record ChatFragment
{
    public required string Type { get; set; }
    public required string Text { get; set; }
    //public required string Cheermote { get; set; }
    //public required string Emote { get; set; }
    //public required string Mention { get; set; }
}

public record Badge
{
    public required string SetId { get; set; }
    public required string Id { get; set; }
    public required string Info { get; set; }
}

