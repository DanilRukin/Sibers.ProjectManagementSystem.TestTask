using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Components
{
    public partial class RedirectToLoginComponent
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.NavigateToLogin("authentication/login");
        }
    }
}
