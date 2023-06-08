using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.SharedKernel
{
    public class EntityBaseTests
    {
        [Fact]
        public void Equals_ReferenceEquals_ShouldReturnTrue()
        {
            EntityA a = new EntityA(1, 2, "3", new ComplexObject(4, "5"));
            EntityA b = a;
            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_DifferentTypeObjectsButTheSameIds_ShouldReturnFalse()
        {
            EntityA a = new EntityA(1, 2, "3", new ComplexObject(4, "5"));
            EntityB b = new EntityB(1, 2, "3", new List<int> { 1, 2, 3 });
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_ArgumentIsNull_ShouldReturnFalse()
        {
            EntityA a = new EntityA(1, 2, "3", new ComplexObject(4, "5"));
            EntityB b = null;
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_TheSameIdsButDifferentOtherProperties_ShouldReturnTrue()
        {
            EntityA a = new EntityA(1, 2, "3", new ComplexObject(4, "5"));
            EntityA b = new EntityA(1, 3, "4", new ComplexObject(5, "5"));
            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_SamePropertiesButDifferentIds_ShouldReturnFalse()
        {
            EntityA a = new EntityA(1, 2, "3", new ComplexObject(4, "5"));
            EntityA b = new EntityA(2, 2, "3", new ComplexObject(4, "5"));
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void AddDomainEvent_ArgumentIsNull_CollectionShouldNotContainEvents()
        {
            DomainEvent domainEvent = null;
            EntityB entity = new EntityB(1, 2, "3", new List<int> { 1, 2 });

            entity.AddDomainEvent(domainEvent);

            Assert.DoesNotContain<DomainEvent>(null, entity.DomainEvents);
            Assert.Empty(entity.DomainEvents);
        }

        [Fact]
        public void AddDomainEvent_TryToAddTheSameEventObjects_CollectionShouldNotChange()
        {
            DateTime now = DateTime.UtcNow;
            DomainEvent domainEvent = new SimpleDomainEvent(now);
            EntityB entity = new EntityB(1, 2, "3", new List<int> { 1 });
            entity.AddDomainEvent(domainEvent);

            entity.AddDomainEvent(domainEvent);

            Assert.NotEmpty(entity.DomainEvents);
            Assert.Contains<DomainEvent>(domainEvent, entity.DomainEvents);
            Assert.Equal(1, entity.DomainEvents.Count);
        }

        [Fact]
        public void AddDomainEvent_AddTwoEquivalentEventObjects_CollectionShouldContainBothEvents()
        {
            DateTime now = DateTime.UtcNow;
            DomainEvent first = new SimpleDomainEvent(now);
            DomainEvent second = new SimpleDomainEvent(now);
            EntityB entity = new EntityB(1, 2, "3", new List<int> { 1 });

            entity.AddDomainEvent(first);
            entity.AddDomainEvent(second);

            Assert.NotEmpty(entity.DomainEvents);
            Assert.Contains(first, entity.DomainEvents);
            Assert.Contains(second, entity.DomainEvents);
            Assert.Equal(2, entity.DomainEvents.Count);
        }

        [Fact]
        public void AddDomainEvent_AddTwoEventObjectsOfTheSameClassWithDifferentPropertiesValues_CollectionShouldContainBoth()
        {
            DateTime now = DateTime.UtcNow;
            DomainEvent first = new SimpleDomainEvent(now);
            DomainEvent second = new SimpleDomainEvent(now.AddMinutes(2));
            EntityB entity = new EntityB(1, 2, "3", new List<int> { 1 });

            entity.AddDomainEvent(first);
            entity.AddDomainEvent(second);

            Assert.NotEmpty(entity.DomainEvents);
            Assert.Contains(first, entity.DomainEvents);
            Assert.Contains(second, entity.DomainEvents);
            Assert.Equal(2, entity.DomainEvents.Count);
        }

        [Fact]
        public void AddDomainEvent_AddEventThenAddNullThenAddEvent_ShouldNotContainNull()
        {
            DateTime now = DateTime.UtcNow;
            DomainEvent first = new SimpleDomainEvent(now);
            DomainEvent second = new SimpleDomainEvent(now.AddMinutes(2));
            EntityB entity = new EntityB(1, 2, "3", new List<int> { 1 });

            entity.AddDomainEvent(first);
            entity.AddDomainEvent(null);
            entity.AddDomainEvent(second);

            Assert.NotEmpty(entity.DomainEvents);
            Assert.Contains(first, entity.DomainEvents);
            Assert.Contains(second, entity.DomainEvents);
            Assert.DoesNotContain<DomainEvent>(null, entity.DomainEvents);
            Assert.Equal(2, entity.DomainEvents.Count);
        }

        [Fact]
        public void RemoveDomainEvent_RemoveNull_ShouldNotThrow()
        {
            DateTime now = DateTime.UtcNow;
            DomainEvent first = new SimpleDomainEvent(now);
            EntityB entity = new EntityB(1, 2, "3", new List<int> { 1 });
            entity.AddDomainEvent(first);

            entity.RemoveDomainEvent(null);

            Assert.Contains(first, entity.DomainEvents);
            Assert.Equal(1, entity.DomainEvents.Count);
        }

        [Fact]
        public void RemoveDomainEvent_AddTwoEventsAndThenRemoveFirstEvent_EventShouldBeRemoved()
        {
            DateTime now = DateTime.UtcNow;
            DomainEvent first = new SimpleDomainEvent(now);
            DomainEvent second = new SimpleDomainEvent(now.AddMinutes(2));
            EntityB entity = new EntityB(1, 2, "3", new List<int> { 1 });
            entity.AddDomainEvent(first);
            entity.AddDomainEvent(second);

            entity.RemoveDomainEvent(first);

            Assert.DoesNotContain(first, entity.DomainEvents);
            Assert.Equal(1, entity.DomainEvents.Count);
        }

        [Fact]
        public void RemoveDomainEvent_AddTwoEquivalentEventsAndThenRemoveFirstEvent_EventShouldBeRemoved()
        {
            DateTime now = DateTime.UtcNow;
            DomainEvent first = new SimpleDomainEvent(now);
            DomainEvent second = new SimpleDomainEvent(now);
            EntityB entity = new EntityB(1, 2, "3", new List<int> { 1 });
            entity.AddDomainEvent(first);
            entity.AddDomainEvent(second);

            entity.RemoveDomainEvent(first);

            Assert.DoesNotContain(first, entity.DomainEvents);
            Assert.Equal(1, entity.DomainEvents.Count);
        }

        private class SimpleDomainEvent : DomainEvent
        {
            public SimpleDomainEvent(DateTime utcNow)
            {
                DateOccured = utcNow;
            }
        }

        private class EntityA : EntityBase<int>
        {
            public int A { get; set; }
            public string B { get; set; }

            public ComplexObject ComplexObject { get; set; }

            public EntityA(int id, int a, string b, ComplexObject complexObject)
            {
                Id = id;
                A = a;
                B = b;
                ComplexObject = complexObject;
            }
        }

        private class ComplexObject
        {
            public int A { get; set; }
            public string B { get; set; }

            public ComplexObject(int a, string b)
            {
                A = a;
                B = b;
            }
        }

        private class EntityB : EntityBase<int>
        {
            public int A { get; set; }
            public string B { get; set; }
            public List<int> C { get; set; }

            public EntityB(int id, int a, string b, List<int> c)
            {
                Id = id;
                A = a;
                B = b;
                C = c;
            }
        }
    }
}
