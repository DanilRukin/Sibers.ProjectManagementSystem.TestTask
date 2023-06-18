using System.ComponentModel.DataAnnotations;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Это обязательное поле")]
        [StringLength(50, ErrorMessage = "Слишком длинное имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Это обязательное поле")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
