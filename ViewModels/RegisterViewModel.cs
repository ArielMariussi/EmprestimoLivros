using System.ComponentModel.DataAnnotations;

namespace EmprestimoLivros.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O email é obrigatorio!")]
    [EmailAddress(ErrorMessage = "Email invalido")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "A senha é obrigatória!")]
    [StringLength(100, ErrorMessage = "A senha deve ter no minimo {2} caracteres", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirmar senha")]
    [Compare("Password", ErrorMessage = "As senhas nao conferem")]  
    public string ConfirmPassword { get; set; } = string.Empty;
}
