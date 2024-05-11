using Microsoft.AspNetCore.Mvc.Testing;
using Server;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AuctionTests
{
    public class UserTests
    {
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
            var user = new User(0, testUserID, "testUser", "testPass", "TestType", "Test@email",
                                "testPhone", 12345, "TestName", "Testlastname");

            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
           

            var response = await _client.PostAsync("/users", content);
            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var addedUserResponse = await _client.GetAsync($"/users/{testUserID}");            
            var addedUserJson = await addedUserResponse.Content.ReadAsStringAsync();
            var addedUsers = JsonSerializer.Deserialize<List<User>>(addedUserJson); 
            var addedUser = addedUsers.FirstOrDefault(u => u.UserID == testUserID);
            
        }


        [Fact]
        public async Task DeleteUser()
        {
            String testUserID = Guid.NewGuid().ToString();
            var user = new User(0, testUserID, "DeleteTestUser", "testPass", "TestType", "Test@email",
                                "testPhone", 12345, "TestName", "Testlastname");

            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/users", content);
            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseDelete = await _client.DeleteAsync($"/users/{testUserID}");
           // Assert.True(responseDelete.IsSuccessStatusCode);
            
        }

        [Fact]
        public async Task UpdateUser()
        {
            String testUserID = Guid.NewGuid().ToString();
            var user = new User(0, testUserID, "UpdateTestUser", "testPass", "TestType", "test@email",
                                "testPhone", 12345, "TestName", "Testlastname");

            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var createResponse = await _client.PostAsync("/users", content);
            //Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

            user = new User(user.Id, testUserID, "UpdatedUser", "newPass", "NewType", "new@testemail.com",
                            "newPhone", 67890, "NewFirstName", "NewLastName");

            json = JsonSerializer.Serialize(user);
            content = new StringContent(json, Encoding.UTF8, "application/json");

            var updateResponse = await _client.PutAsync($"/users/{testUserID}", content);
           // Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var responseGet = await _client.GetAsync($"/users/{testUserID}");
            //Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);

            var updatedUserJson = await responseGet.Content.ReadAsStringAsync();
             var updatedUser = JsonSerializer.Deserialize<List<User>>(updatedUserJson);
            // Assert.NotNull(updatedUser);
            // Assert.Equal("UpdatedUser", updatedUser.Username);
            // Assert.Equal("new@testemail.com", updatedUser.Email);
        }

    }
}
