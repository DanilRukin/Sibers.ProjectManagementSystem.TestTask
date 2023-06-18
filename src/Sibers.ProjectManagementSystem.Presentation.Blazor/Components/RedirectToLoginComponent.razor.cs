using Microsoft.AspNetCore.Components;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Components
{
    public partial class RedirectToLoginComponent
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo("/login"); ;
        }
    }
}
