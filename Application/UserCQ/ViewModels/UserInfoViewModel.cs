namespace Application.UserCQ.ViewModels;

// Tipo de retorno
public class UserInfoViewModel
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpirationTime { get; set; }
    public string? TokenJWT { get; set; }
}
