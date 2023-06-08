using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate
{
    public interface IProjectFactory
    {
        Project Create(string name, DateTime startDate, DateTime endDate, Priority priority,
            string nameOfTheCustomerCompany, string nameOfTheContractorComapny);
    }
}
