using AutoMapper;
using MediatR;
using Sibers.ProjectManagementSystem.Data;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.EmployeeAgregate.Commands
{
    internal class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<EmployeeDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper _mapper;

        public CreateEmployeeCommandHandler(ProjectManagementSystemContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<EmployeeDto>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Employee employee = await _context.CreateEmployee(
                    new PersonalData(request.EmployeeDto.FirstName, request.EmployeeDto.LastName, request.EmployeeDto.Patronymic),
                    new Email(request.EmployeeDto.Email));
                return Result<EmployeeDto>.Success(_mapper.Map<EmployeeDto>(employee));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.Handle<EmployeeDto>(ex);
            }
        }
    }
}
