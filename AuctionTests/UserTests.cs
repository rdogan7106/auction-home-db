using Microsoft.AspNetCore.Mvc.Testing;
using Server;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AuctionTests
{
    public class UserTests  {
        private readonly HttpClient _client;

        public UserTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000")
            };
        }

        [Fact]
        public async Task AddUser()
        {
            String testUserID = Guid.NewGuid().ToString();
            var user = new User(0, testUserID, "testUser", "testPass", "TestType", "Test@email", "testPhone", 12345, "TestName", "Testlastname");

            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/users", content);

            if (response.IsSuccessStatusCode)
            {
                var addedUserResponse = await _client.GetAsync($"/users/{testUserID}");
                if (addedUserResponse.IsSuccessStatusCode)
                {
                    var addedUserJson = await addedUserResponse.Content.ReadAsStringAsync();
                    var addedUser = JsonSerializer.Deserialize<User>(addedUserJson);
                    Assert.Equal(testUserID, addedUser.UserID);

                }
                else
                {
                    Console.WriteLine("Something went wrong");

                }
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }
    }
}
