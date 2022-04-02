using System.ComponentModel.DataAnnotations;

namespace discord_clicker.ViewModels;

public class LoginModel
{
    [Required(ErrorMessage = "Не указан Email")]
    public string Nickname { get; set; }

    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}