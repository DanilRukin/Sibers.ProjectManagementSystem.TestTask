using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.SharedKernel
{
    public class ValueObjectTests
    {
        private string _guid = "97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0";

        [Fact]
        public void EqualsOperator_EqualsValueObjects_ReturnTrue()
        {
            // arrange
            ValueObjectA a = new ValueObjectA("1", 2, Guid.Parse(_guid), new ComplexObject(3, "4"));
            ValueObjectA b = new ValueObjectA("1", 2, Guid.Parse(_guid), new ComplexObject(3, "4"));
            // act
            bool result = a == b;
            // assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsOperator_TheSameObjects_ReturnTrue()
        {
            ValueObjectA a = new ValueObjectA("1", 2, Guid.Parse(_guid), new ComplexObject(3, "4"));
            ValueObjectA b = a;
            bool result = a == b;
            Assert.True(result);
        }

        [Fact]
        public void EqualsOperator_DifferentNotEqualComponents_ShouldReturnTrue()
        {
            ValueObjectA a = new ValueObjectA("1", 2, Guid.Parse(_guid), new ComplexObject(3, "4"), _guid);
            ValueObjectA b = new ValueObjectA("1", 2, Guid.Parse(_guid), new ComplexObject(3, "4"), "12546");
            bool result = a == b;
            Assert.True(result);
        }

        [Fact]
        public void EqualsOperator_TheSameNotEqualComponents_ShouldReturnTrue()
        {
            ValueObjectA a = new ValueObjectA("1", 2, Guid.Parse(_guid), new ComplexObject(3, "4"), _guid);
            ValueObjectA b = new ValueObjectA("1", 2, Guid.Parse(_guid), new ComplexObject(3, "4"), _guid);
            bool result = a == b;
            Assert.True(result);
        }

        [Fact]
        public void EqualsOperator_BothAreNull_ShouldReturnTrue()
        {
            ValueObjectA a = null;
            ValueObjectB b = null;
            bool result = a == b;
            Assert.True(result);
        }

        [Fact]
        public void EqualsOperator_ValueObjectsBHaveDifferentLists_ShouldReturnFalse()
        {
            ValueObjectB a = new ValueObjectB(1, "2", new List<int> { 1, 2, 4 });
            ValueObjectB b = new ValueObjectB(1, "2", new List<int> { 1, 2, 4, 7 });
            Assert.False(a == b);
        }

        [Fact]
        public void EqualsOperator_ValueObjectsBHaveDifferentListsWithTheSameValues_ShouldReturnTrue()
        {
            ValueObjectB a = new ValueObjectB(1, "2", new List<int> { 1, 2, 4 });
            ValueObjectB b = new ValueObjectB(1, "2", new List<int> { 1, 2, 4 });
            Assert.True(a == b);
        }

        private class ValueObjectA : ValueObject
        {
            public string A { get; protected set; }
            public int B { get; protected set; }
            public Guid C { get; protected set; }
            public ComplexObject D { get; protected set; }
            public string NotAnEqualityComponent { get; protected set; }

            public ValueObjectA(string a, int b, Guid c, ComplexObject d, string notAnEqualityComponent = null)
            {
                A = a;
                B = b;
                C = c;
                D = d;
                NotAnEqualityComponent = notAnEqualityComponent;
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return A;
                yield return B;
                yield return C;
                yield return D;
            }
        }

        private class ComplexObject : IEquatable<ComplexObject>
        {
            public int A { get; }
            public string B { get; }

            public ComplexObject(int a, string b)
            {
                A = a;
                B = b;
            }
            public override bool Equals(object? obj)
            {
                return Equals(obj as ComplexObject);
            }
            public bool Equals(ComplexObject? other)
            {
                return other != null
                    && other.A == A
                    && other.B == B;
            }
        }

        private class ValueObjectB : ValueObject
        {
            public int A { get; }
            public string B { get; }
            public List<int> C { get; }

            public ValueObjectB(int a, string b, List<int> c)
            {
                A = a;
                B = b;
                C = c;
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return A;
                yield return B;
                foreach (var c in C)
                {
                    yield return c;
                }
            }
        }
    }
}
