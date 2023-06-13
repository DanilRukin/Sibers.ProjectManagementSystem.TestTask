using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Commands
{
    internal class UpdateProjectsDataCommandHandler : IRequestHandler<UpdateProjectsDataCommand, Result<ProjectDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public UpdateProjectsDataCommandHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<ProjectDto>> Handle(UpdateProjectsDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == request.Project.Id, cancellationToken);
                if (project == null)
                    return Result<ProjectDto>.NotFound($"No such project with id: {request.Project.Id}");
                project.ChangePriority(new Priority(request.Project.Priority));
                project.ChangeContractorCompanyName(request.Project.NameOfTheContractorCompany);
                project.ChangeCustomerCompanyName(request.Project.NameOfTheCustomerCompany);
                project.ChangeName(request.Project.Name);
                project.ChangeEndDate(request.Project.EndDate);
                project.ChangeStartDate(request.Project.StartDate);               
                await _context.SaveEntitiesAsync(cancellationToken);
                return Result.Success<ProjectDto>(_mapper.Map<ProjectDto>(project));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<ProjectDto>(ex);
            }
        }
    }
}
