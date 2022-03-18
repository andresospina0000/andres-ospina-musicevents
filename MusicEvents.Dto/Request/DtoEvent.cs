namespace MusicEvents.Dto.Request;

public record DtoEvent(string Title, string Description, string Date, string Time, int TicketsQuantity)
{
    public string Place { get; set; }
    public decimal UnitPrice { get; init; }
    public string FileName { get; init; }
    public string ImageBase64 { get; init; }
    public int GenreId { get; init; }
}
