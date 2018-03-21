namespace Matrix.Srv.Tests
{
    using MatrixClient;

    using Xunit;
    using Shouldly;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ExtensionsTests
    {
        public class Person
        {
            public string Name { get; set; }
            public string Key { get; set; }
        }

        [Fact]
        public void Should_Add_New_Record_To_Collection()
        {
            var persons = new ObservableCollection<Person>();

            persons.Add(new Person { Name = "Alex", Key = "Alex" });

            var newPerson = new Person { Name = "Alex2", Key = "Alex2" };
            persons.AddOrReplace(newPerson, p => p.Key == newPerson.Key);

            persons.Count.ShouldBe(2);
        }

        [Fact]
        public void Should_Replace_First_Person()
        {
            var persons = new ObservableCollection<Person>();

            persons.Add(new Person { Name = "Alex", Key = "Alex" });

            var newPerson = new Person { Name = "Bob", Key = "Alex" };
            persons.AddOrReplace(newPerson, p => p.Key == newPerson.Key);

            persons.Count.ShouldBe(1);
            persons[0].Name.ShouldBe("Bob");
        }

        [Fact]
        public void Should_Remove_Person()
        {
            var persons = new ObservableCollection<Person>();

            persons.Add(new Person { Name = "Alex", Key = "Alex" });
            persons.Add(new Person { Name = "Alex2", Key = "Alex2" });

            
            persons.Remove(p => p.Key == "Alex2");

            persons.Count.ShouldBe(1);
            persons.Any(p => p.Key == "Alex2").ShouldBe(false);
            persons.Any(p => p.Key == "Alex").ShouldBe(true);
        }
    }
}
