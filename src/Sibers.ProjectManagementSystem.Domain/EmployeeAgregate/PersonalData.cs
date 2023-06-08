using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.EmployeeAgregate
{
    public class PersonalData : ValueObject
    {
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string Patronymic { get; protected set; }

        public PersonalData(string firstName, string lastName, string patronymic = "")
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is null or empty");
            FirstName = firstName;
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is null or empty");
            LastName = lastName;
            if (patronymic == null)
                throw new ArgumentException("Patronymic is null");
            Patronymic = patronymic;
        }

        protected PersonalData() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return Patronymic;
        }
    }
}
