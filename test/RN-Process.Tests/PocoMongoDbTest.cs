using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Xunit;
using Xunit.Abstractions;

namespace RN_Process.Tests
{
    public class PocoMongoDbTest
    {
        public PocoMongoDbTest(ITestOutputHelper testOutputHelper)
        {
            JsonWriterSettings.Defaults.Indent = true;
            _testOutputHelper = testOutputHelper;
        }

        private readonly ITestOutputHelper _testOutputHelper;

        public class Pessoa
        {
            public List<string> Address = new List<string>();
            public string Description { get; set; }
            public int Age { get; set; }
            public Contact Contact { get; set; } = new Contact();
        }

        public class Contact
        {
            public string Description { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
        }

        [Fact]
        public void Automatic()
        {
            var person = new Pessoa
            {
                Description = "Meu Nome 1",
                Age = 45
            };

            person.Address.Add("My location 011");
            person.Address.Add("My location two n 2 road 11");
            person.Contact.Email = "teste@email.com";
            person.Contact.PhoneNumber = "9127756652";
            person.Contact.Description = "Personal";

            _testOutputHelper.WriteLine(person.ToJson());
        }
    }
}