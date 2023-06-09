using AutoMapper;
using MediatR;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.ProjectAgregate.Commands
{
    internal class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public CreateProjectCommandHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project project = ((IProjectFactory)_context).Create(request.Project.Name, request.Project.StartDate, request.Project.EndDate,
                    new Priority(request.Project.Priority), request.Project.NameOfTheCustomerCompany, request.Project.NameOfTheContractorCompany);
                return Result.Success(_mapper.Map<ProjectDto>(project));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<ProjectDto>(ex);
            }
        }
    }
}
