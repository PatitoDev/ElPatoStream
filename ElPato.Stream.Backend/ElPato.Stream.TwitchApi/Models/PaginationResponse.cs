namespace ElPato.Stream.TwitchApi;

public class PaginationResponse<T>
{
    public int? Total { get; set; }
    public required IEnumerable<T> Data { get; set; }
    public PaginationPage? Pagination { get; set; }
}

public record PaginationPage(string? Cursor = null);
