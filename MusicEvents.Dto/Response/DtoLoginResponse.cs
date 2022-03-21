namespace MusicEvents.Dto.Response;

public class DtoLoginResponse : BaseResponse
{
    public string Token { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public ICollection<string> Roles { get; set; }
}