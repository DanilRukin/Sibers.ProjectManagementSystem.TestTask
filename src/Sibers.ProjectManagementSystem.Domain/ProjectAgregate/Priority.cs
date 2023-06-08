using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate
{
    public class Priority : ValueObject
    {
        public int Value { get; protected set; }

        public Priority(int value)
        {
            if (value < 0)
                throw new ArgumentException("Priority cannot be less, than 0");
            Value = value;
        }

        public static Priority Default() => new Priority(0);

        protected Priority() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
