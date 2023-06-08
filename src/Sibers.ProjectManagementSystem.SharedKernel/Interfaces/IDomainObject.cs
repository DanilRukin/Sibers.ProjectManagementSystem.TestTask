using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.SharedKernel.Interfaces
{
    public interface IDomainObject
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }

        void AddDomainEvent(DomainEvent domainEvent);

        void RemoveDomainEvent(DomainEvent domainEvent);

        void ClearDomainEvents();
    }
}
