using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace learn_xunit
{
    
    public class MyTestFixture
    {
        [Fact]
        public void Addition_Test()
        {
            var result = 2 + 5;
            Assert.Equal(result,7);
        }

        [Fact]
        public void Collection_Test()
        {
            var myPeople = new[]
            {
                new Person { PersonId = 1, Name = "Number One" },
                new Person { PersonId = 2, Name = "Number Two" },
            };

            var otherPeople = new[]
            {
                new Person { PersonId = 3, Name = "Number Three" },
                new Person { PersonId = 4, Name = "Number Five" },
                new Person { PersonId = 4, Name = "Number Five" },
            };

            var allPeople = new List<Person>();
            allPeople.AddRange(myPeople);
            allPeople.AddRange(otherPeople);

            Assert.NotEqual(myPeople,otherPeople);
            Assert.NotEmpty(allPeople);
            Assert.Subset(allPeople.ToSet(),myPeople.ToSet());
            Assert.Subset(allPeople.ToSet(),otherPeople.ToSet());

            Assert.All(allPeople,a=>Assert.NotNull(a.Name));
            
        }
       
        [Fact]
        public void AssertStrings()
        {
            Assert.Equal("foo","FOO",ignoreCase:true);
            Assert.Equal(1.1d,1.14d,1);
        }
    }

    public static class TestExtensions
    {
        public static ISet<T> ToSet<T>(this IEnumerable<T> data)
        {
            return new HashSet<T>(data);
        }
    }

    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
    }
}
