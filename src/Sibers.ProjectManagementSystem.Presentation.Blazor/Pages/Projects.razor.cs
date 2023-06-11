using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Dialogs.Projects;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Commands;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Projects.Queries;
using Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.ViewModels;
using Sibers.ProjectManagementSystem.SharedKernel.Results;


namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Pages
{
    public partial class Projects
    {
        [Inject]
        public ISnackbar Snackbar { get; set; }

        [Inject]
        public IDialogService DialogService { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        private ICollection<ProjectViewModel> _projects;

        protected override async Task OnInitializedAsync()
        {
            await LoadProjects();
        }

        private async Task LoadProjects()
        {
            try
            {
                var response = await Mediator.Send(new GetAllProjectsQuery(false));
                if (!response.IsSuccess)
                {
                    Snackbar.Add("Не удалось загрузить проекты.", Severity.Info);
                    _projects = new List<ProjectViewModel>();
                }
                else
                {
                    _projects = Mapper.Map<IEnumerable<ProjectDto>, ICollection<ProjectViewModel>>(response.Value);
                }
            }
            catch (Exception e)
            {
                Snackbar.Add($"Что-то пошло не так. Причина: {e.ReadErrors()}", Severity.Error);
            }
        }

        private async Task OnCreateProject()
        {
            var dialog = DialogService.Show<CreateProjectDialog>("Создание проекта");
            using var task = dialog.Result;
            var result = await task;
            if (result != null && !result.Cancelled && (result.Data is ProjectViewModel createdProject))
            {
                _projects.Add(createdProject);
            }
        }

        private async Task OnWatchProject(int projectId)
        {
            DialogParameters parameters = new DialogParameters();
            GetProjectByIdQuery query = new GetProjectByIdQuery(new ProjectIncludeOptions(projectId, false));
            Result<ProjectDto> projectResult = await Mediator.Send(query);
            if (!projectResult.IsSuccess)
            {
                Snackbar.Add($"Ошибка при загрузке данных проекта. Причина: {projectResult.Errors.AsOneString()}", Severity.Error);
            }
            parameters.Add(nameof(WatchProjectDialog.Project), Mapper.Map<ProjectViewModel>(projectResult.Value));
            var dialog = DialogService.Show<WatchProjectDialog>("Информация о проекте", parameters);
        }

        private async Task OnProjectDeleting(int projectId)
        {
            bool? result = await DialogService.ShowMessageBox(
                title: $"Удаление проекта {projectId}",
                markupMessage: new MarkupString("Вы действительно хотите удалить проект? Действие невозможно будет отменить"),
                yesText: "Удалить",
                cancelText: "Отмена"
            );
            if (result != null && result == true)
            {
                DeleteProjectCommand deleteCommand = new(projectId);
                Result deleteResult = await Mediator.Send(deleteCommand);
                if (deleteResult.IsSuccess)
                {
                    _projects.RemoveWithCriterion(p => p.Id == projectId);
                    Snackbar.Add("Проект успешно удален.", Severity.Success);
                }
                else
                {
                    Snackbar.Add(deleteResult.Errors.AsOneString(), Severity.Error);
                }
            }
        }

        protected async Task OnProjectEdit(int projectId)
        {
            ProjectViewModel? project = _projects.FirstOrDefault(p => p.Id == projectId);
            DialogParameters parameters = new DialogParameters();
            parameters.Add(nameof(EditProjectDialog.ProjectToEdit), project);
            var dialog = DialogService.Show<EditProjectDialog>("Управление проектом", parameters);
            using var task = dialog.Result;
            var result = await task;
            if (result != null && !result.Cancelled && (result.Data is bool ok))
            {
                if (ok)
                {
                    GetProjectByIdQuery query = new(new ProjectIncludeOptions(projectId, false));
                    Result<ProjectDto> updatedProjectResult = await Mediator.Send(query);
                    if (!updatedProjectResult.IsSuccess)
                    {
                        Snackbar.Add($"Не удалось получить обновленные данные проекта. Причина: {updatedProjectResult.Errors.AsOneString()}", Severity.Error);
                    }
                    else
                    {
                        _projects.RemoveWithCriterion(p => p.Id == projectId);
                        _projects.Add(Mapper.Map<ProjectViewModel>(updatedProjectResult.Value));
                    }
                }
            }
        }
    }
}
