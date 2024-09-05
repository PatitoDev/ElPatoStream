namespace ElPato.Stream.Api;

public class ChatCommand
{
    public required IEnumerable<string> Commands;
    public required string Result;
}

public static class ChatCommands
{
    public static readonly IEnumerable<string> Shoutout = ["so", "shoutout"];
    public static readonly IEnumerable<string> Death = ["die", "death", "muerte"];
    public static readonly IEnumerable<string> DeathReset = ["diereset", "deathreset", "muertereset"];


    private const string YoutubeUrl = "https://www.youtube.com/channel/UCoP0r9z8DHWJxD2KuQhWNSA";
    private const string ItchUrl = "https://patitodev.itch.io/";
    private const string DiscordUrl = "https://discord.gg/Y5EnYnrJ4K";
    private const string GithubUrl = "github.com/patitodev";
    private const string TwitterUrl = "twitter.com/patito_dev";
    private const string InstagramUrl = "instagram.com/patitodev";
    private const string TiktokUrl = "tiktok.com/@patitodev";

    private const string PronombresInfo = "Respeten los pronombres de cada persona, para ver los pronombres en el chat pueden instalarse la extension de pronombres 👉 https://chromewebstore.google.com/detail/twitch-chat-pronouns/agnfbjmjkdncblnkpkgoefbpogemfcii?pli=1 y pueden configurarse sus pronombres 👉 https://pr.alejo.io/";
    private const string TransBible = "Si tienes dudas sobre tu genero o quieres aprender mas sobre la disforia y las personas trans para ser un buen ally este es un buen recurso 👉 https://genderdysphoria.fyi/es/";
    private const string PrideFlags = "https://www.prideflags.org/";

    public static readonly IEnumerable<ChatCommand> Commands = new List<ChatCommand>()
    {
        new ()
        {
            Commands = new List<string>(){ "youtube", "yt", "y" },
            Result = YoutubeUrl,
        },

        new ()
        {
            Commands = new List<string>(){ "itch", "i" },
            Result = ItchUrl,
        },

        new ()
        {
            Commands = new List<string>(){ "discord", "d", "disc" },
            Result = DiscordUrl,
        },

        new ()
        {
            Commands = new List<string>(){ "github", "g", "git", "code" },
            Result = GithubUrl,
        },

        new ()
        {
            Commands = new List<string>(){ "twitter", "t", "tweet", "twiter" },
            Result = TwitterUrl,
        },

        new ()
        {
            Commands = new List<string>(){ "instagram", "insta", "gram", "ig", "art" },
            Result = InstagramUrl,
        },

        new ()
        {
            Commands = new List<string>(){ "tiktok", "tok", "clips" },
            Result = TiktokUrl,
        },

        new ()
        {
            Commands = new List<string>(){ "redes", "socials", "social", "s", "sociales" },
            Result = $"Encuentra mas patos aqui 👉 Twitter: {TwitterUrl} Instagram: {InstagramUrl} Discord: {DiscordUrl} Tiktok: {TiktokUrl} Youtube: {YoutubeUrl} Github: {GithubUrl} Itch: {ItchUrl}",
        },

        new ()
        {
            Commands = new List<string>(){ "pronombre", "pronombres", "pronouns", "pronoun" },
            Result = PronombresInfo,
        },

        new ()
        {
            Commands = new List<string>(){ "transbible", "transhelp", "ayudatrans", "transayuda", "trans", "disforia", "dysphoria"},
            Result = TransBible,
        },

        new ()
        {
            Commands = new List<string>(){ "prideflags", "pride", "flags", "banderas", "lgbtq", "lgbt", "lgbtqi", "lgbtqia", "lgbtqia+" },
            Result = PrideFlags,
        },
    };

    public static void ValidateCommands()
    {
        var repeated = Commands
            .SelectMany(x => x.Commands)
            .Select(s => s.ToLower())
            .GroupBy(x => x)
            .Where(g => g.Count() > 1);

        if (repeated.Any())
        {
            throw new Exception($"Duplicate commands found {string.Join(",", repeated)}");
        }
    }
}
