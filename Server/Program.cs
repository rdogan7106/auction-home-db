using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Server;

var builder = WebApplication.CreateBuilder(args);
var state = new Dictionary<string, string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
var app = builder.Build();
app.UseCors("AllowSpecificOrigin");
string connectionString = "server=localhost;uid=root;pwd=Rd0671rd..;database=AuctionDatabase;port=3306";

app.MapPost("/users", (User user) =>
{
    using var db = new MySqlConnection(connectionString);
    try
    {
        db.Open();
        var addedUser = Users.AddUser(db, user);
        return Results.Ok(addedUser);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Something went wrong!");
    }
});
app.MapGet("/users",()=> @"
[
    {
      ""id"": ""1"",
      ""username"": ""a1"",
      ""password"": ""a1"",
      ""type"": ""admin"",
      ""email"": ""asd@gmail.com"",
      ""phone"": ""123456789"",
      ""personalNumber"": ""12354123"",
      ""firstname"": ""Sven"",
      ""lastname"": ""Svenson""
    },
    {
      ""id"": ""3"",
      ""username"": ""u3"",
      ""password"": ""u3"",
      ""type"": ""user"",
      ""email"": ""asd@gmail.com"",
      ""phone"": ""123456789"",
      ""personalNumber"": ""12354123"",
      ""firstname"": ""Sven"",
      ""lastname"": ""Svenson""
    },
    {
      ""id"": ""4"",
      ""username"": ""u4"",
      ""password"": ""u4"",
      ""type"": ""user"",
      ""email"": ""asd@gmail.com"",
      ""phone"": ""123456789"",
      ""personalNumber"": ""12354123"",
      ""firstname"": ""Sven"",
      ""lastname"": ""Svenson""
    },
    {
      ""id"": ""5"",
      ""username"": ""u5"",
      ""password"": ""u5"",
      ""type"": ""user"",
      ""email"": ""asd@gmail.com"",
      ""phone"": ""123456789"",
      ""personalNumber"": ""12354123"",
      ""firstname"": ""Sven"",
      ""lastname"": ""Svenson""
    },
    {
      ""id"": ""4311a58b-d3f8-44ca-8a1f-f83378f3e654"",
      ""username"": ""a"",
      ""password"": ""sd"",
      ""type"": ""user"",
      ""email"": ""asd1@asd.com"",
      ""phone"": ""0764110990"",
      ""personalNumber"": ""34123"",
      ""firstname"": ""asdsadasd"",
      ""lastname"": ""DOGAN""
    },
    {
      ""username"": ""a"",
      ""password"": ""12"",
      ""type"": ""user"",
      ""email"": ""asd2@gmail.com"",
      ""phone"": ""0764110990"",
      ""personalNumber"": ""123123"",
      ""firstname"": ""RAHMAN"",
      ""lastname"": ""DOGAN"",
      ""id"": ""2a83ba0e-7d08-4da6-b0aa-76e87e8e2215""
    },
    {
      ""id"": ""804ecb53-af3d-462e-982a-639b1c29f402"",
      ""username"": ""Sofie"",
      ""password"": ""ASDADSDASDA"",
      ""type"": ""user"",
      ""email"": ""7777777777777777@hotmail.com"",
      ""phone"": ""4444444444444444444444"",
      ""personalNumber"": ""777777777777777777"",
      ""firstname"": ""Person"",
      ""lastname"": ""KanelBulle""
    },
    {
      ""id"": ""6544f876-8537-4d56-a558-eb0e1ec4f34a"",
      ""username"": ""Sofies"",
      ""password"": ""AsSDADSDASDA"",
      ""type"": ""user"",
      ""email"": ""7777777s777777777@hotmail.com"",
      ""phone"": ""44444s44444444444444444"",
      ""personalNumber"": ""3333323333333333333333"",
      ""firstname"": ""Persons"",
      ""lastname"": ""KanelBulles""
    },
    {
      ""id"": ""957f73d8-3944-40f7-ab82-08cd4f03188d"",
      ""username"": ""test"",
      ""password"": ""ASDASDASD"",
      ""type"": ""user"",
      ""email"": ""kamel@hotmail.com"",
      ""phone"": ""6666666666666666666"",
      ""personalNumber"": ""222222222222222"",
      ""firstname"": ""testing"",
      ""lastname"": ""kanelnakel""
    },
    {
      ""id"": ""f7fd5d34-400e-4c9d-bbc4-81e2fe404324"",
      ""username"": ""Rahman"",
      ""password"": ""ASDASDASDASDASD"",
      ""type"": ""user"",
      ""email"": ""5555555555555555@hotmail.com"",
      ""phone"": ""4444444444444444444444"",
      ""personalNumber"": ""555555555555555555"",
      ""firstname"": ""Godzilla"",
      ""lastname"": ""Trexasoriouse""
    },
    {
      ""id"": ""fdda326a-2f11-4b5d-8675-f687bb683ee6"",
      ""username"": ""KamelKamel"",
      ""password"": ""ASDSADASDASD"",
      ""type"": ""user"",
      ""email"": ""sdasdasd@hotmail.com"",
      ""phone"": ""999999999999999"",
      ""personalNumber"": ""444444444444444"",
      ""firstname"": ""asdadasdasd"",
      ""lastname"": ""MASDASD""
    },
    {
      ""id"": ""8d7cda9a-137e-4197-91b6-4e46fb65db7c"",
      ""username"": ""JoeMasoud"",
      ""password"": ""ASDASDASD"",
      ""type"": ""user"",
      ""email"": ""letsfindout@hotmail.com"",
      ""phone"": ""1323232332323298"",
      ""personalNumber"": ""231231231313"",
      ""firstname"": ""okokokokok"",
      ""lastname"": ""sssssssssslslss""
    },
    {
      ""username"": ""u6"",
      ""password"": ""u6"",
      ""type"": ""user"",
      ""email"": ""rdogan7106@gmail.com"",
      ""phone"": ""0764410990"",
      ""personalNumber"": ""1234565"",
      ""firstname"": ""user6"",
      ""lastname"": ""user6"",
      ""id"": ""e8862655-a781-41ac-9a9b-2f595f5ffc1d""
    },
    {
      ""username"": ""u10"",
      ""password"": ""u10"",
      ""type"": ""user"",
      ""email"": ""asd@asd.com"",
      ""phone"": ""0764110990"",
      ""personalNumber"": ""123213"",
      ""firstname"": ""u10"",
      ""lastname"": ""DOGAN"",
      ""id"": ""20b6cfad-451a-4cc6-b700-b54d284a7bcf""
    },
    {
      ""username"": ""u11"",
      ""password"": ""abc123"",
      ""type"": ""user"",
      ""email"": ""asdsa@gmail.com"",
      ""phone"": ""0764410990"",
      ""personalNumber"": ""23234"",
      ""firstname"": ""u11"",
      ""lastname"": ""u11"",
      ""id"": ""25e3aac0-d144-4828-a9bc-fbd71a14b7d4""
    }
  ]");

app.MapGet("/auctions", () => @"[
    {
      ""sellerId"": ""3"",
      ""sellerName"": ""u3"",
      ""itemDetails"": {
        ""title"": ""Fifa 2020"",
        ""description"": ""New Item 4"",
        ""image"": ""data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAoHCBUVFBgVFRUYGBgaGxsdGhsbGyAeJBwgIBsgIBobHR0jIi0kIB0pIBgdJTclKS4wNDQ0ICM5PzkyPi4yNDABCwsLEA8QHRISHTIpJCk2MjIyNTIyNTIyMjIyMDQyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMv/AABEIAKgBLAMBIgACEQEDEQH/xAAcAAABBQEBAQAAAAAAAAAAAAADAQIEBQYABwj/xABJEAACAQIEAwUEBwUDCwQDAAABAhEAAwQSITEFQVEGEyJhcTKBkbEHQlKSocHRFCNy4fAzYtIVJCVTVIKTorLC40Oz0/EXc3T/xAAZAQADAQEBAAAAAAAAAAAAAAAAAQIDBAX/xAAsEQACAgEDBAAFAwUAAAAAAAAAAQIRAxIhMQQTQVEiMnGBoTNSYRQjQpHw/9oADAMBAAIRAxEAPwD0heLxmz23GVwhK+L2iuXQa/XHKu4Zj7TJ3ZZZV3TK2+jHKCp19mN68hw30h4hcwuIjhoMgEHOqKEbf7SKT74q7wHbvDv3ouoyZ7qXFEZxA7sMv8RCHlzqqQj0LFdn8HdnNZtnfVRlPnqsa1mOL9gsKQSjuh9Qw/ET+NFw3G8G5m3iEAW5IC3MujJEhSQPac8uRqBj+NgARikPiuLqyHmSp0I5L+NPQxnnPEcEtt2W3cZ8rMp8BUyNDpMRMjntUU4Vx9U+8Vub3FsuRBbz7TlMatBLARl+sJJM60TD4stcKNbjyMtI5nRY59etTqXcUfHs3WJabfJgUIOhWKelnL4kZkbqpI+VWHG8Jbt4ggMUMA7+7Y+lbLsVfwOIw3+d/s3eK7KSclskQMp8OX4+VXpWpr0c7sy2G7S4y2ADfF1RstwZo95mB6VNw/bW8oH7rUfWRpHOJBPUj7vnNbm72HwF0ZrZYA7FLgYe7NmqmxP0ZqWZbWIZfCphkmZLaGGH2elGk"",
        ""price"": ""10""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-07T22:05"",
      ""endDate"": ""2024-03-23T22:06"",
      ""status"": ""in progress"",
      ""id"": ""fb375fe1-2130-46c5-8ccd-d2c4065e2480""
    },
    {
      ""sellerId"": ""3"",
      ""sellerName"": ""u3"",
      ""itemDetails"": {
        ""title"": ""1"",
        ""description"": ""asd"",
        ""image"": ""asdsad"",
        ""price"": ""1""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-07T22:34"",
      ""endDate"": ""2024-03-07T23:16"",
      ""status"": ""finished"",
      ""id"": ""fe295d9d-ec22-4d02-b21e-7f1942148805""
    },
    {
      ""id"": ""3b47c93e-48b8-4027-a68a-d2fbd9bf3184"",
      ""sellerId"": ""4"",
      ""sellerName"": ""u4"",
      ""itemDetails"": {
        ""title"": ""test"",
        ""description"": ""spel"",
        ""image"": ""t"",
        ""price"": ""20""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-12T12:48"",
      ""endDate"": ""2024-03-22T12:48"",
      ""status"": ""in progress""
    },
    {
      ""id"": ""fa1ba460-e9f9-4e0c-aabd-909bc19675ec"",
      ""sellerId"": ""4"",
      ""sellerName"": ""u4"",
      ""itemDetails"": {
        ""title"": ""spel"",
        ""description"": ""game"",
        ""image"": ""ga,e"",
        ""price"": ""40""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-22T14:36"",
      ""endDate"": ""2024-03-29T14:36"",
      ""status"": ""in progress""
    },
    {
      ""id"": ""274431f8-fa1b-4a32-831b-420bd43829dc"",
      ""sellerId"": ""5"",
      ""sellerName"": ""u5"",
      ""itemDetails"": {
        ""title"": ""Need For Speed"",
        ""description"": ""spel"",
        ""image"": ""spel"",
        ""price"": ""10""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-09T15:13"",
      ""endDate"": ""2024-03-10T15:13"",
      ""status"": ""finished""
    },
    {
      ""id"": ""ad1acd85-97b1-4cae-8923-149bee4c8efa"",
      ""sellerId"": ""5"",
      ""sellerName"": ""u5"",
      ""itemDetails"": {
        ""title"": ""game1"",
        ""description"": ""gamegame"",
        ""image"": ""f"",
        ""price"": ""30""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-22T13:23"",
      ""endDate"": ""2024-03-31T13:23"",
      ""status"": ""in progress""
    },
    {
      ""sellerId"": ""3"",
      ""sellerName"": ""u3"",
      ""itemDetails"": {
        ""title"": ""game 11"",
        ""description"": ""old game"",
        ""image"": ""https://www.operationsports.com/wp-content/uploads/2023/09/efootball-2024-latest-mobile-menu-v0-t1i8bb900fmb1.jpg?w=1536"",
        ""price"": ""10""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-12T10:46"",
      ""endDate"": ""2024-03-12T10:49"",
      ""status"": ""finished"",
      ""id"": ""121556d2-ef5c-49e0-b88e-aa39c1a9a814""
    },
    {
      ""sellerId"": ""4"",
      ""sellerName"": ""u4"",
      ""itemDetails"": {
        ""title"": ""game 12"",
        ""description"": ""Brand New"",
        ""image"": ""https://cdn.pixabay.com/photo/2020/03/05/12/48/spring-4904395_640.jpg"",
        ""price"": ""12""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-12T10:50"",
      ""endDate"": ""2024-03-21T10:50"",
      ""status"": ""in progress"",
      ""id"": ""c84d1758-1d6a-4cee-bab1-5872f31a0cd5""
    },
    {
      ""sellerId"": ""4"",
      ""sellerName"": ""u4"",
      ""itemDetails"": {
        ""title"": ""game 13"",
        ""description"": ""Brand New"",
        ""image"": ""https://cdn.pixabay.com/photo/2020/03/05/12/48/spring-4904395_640.jpg"",
        ""price"": ""13""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-12T10:56"",
      ""endDate"": ""2024-03-12T10:58"",
      ""status"": ""finished"",
      ""id"": ""e793b1c0-6beb-4e84-83f4-f1183261c81e""
    },
    {
      ""sellerId"": ""5"",
      ""sellerName"": ""u5"",
      ""itemDetails"": {
        ""title"": ""game 14"",
        ""description"": ""Brand New"",
        ""image"": ""data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAoHCBUVFBgVFRUYGBgaGxsdGhsbGyAeJBwgIBsgIBobHR0jIi0kIB0pIBgdJTclKS4wNDQ0ICM5PzkyPi4yNDABCwsLEA8QHRISHTIpJCk2MjIyNTIyNTIyMjIyMDQyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMv/AABEIAKgBLAMBIgACEQEDEQH/xAAcAAABBQEBAQAAAAAAAAAAAAADAQIEBQYABwj/xABJEAACAQIEAwUEBwUDCwQDAAABAhEAAwQSITEFQVEGEyJhcTKBkbEHQlKSocHRFCNy4fAzYtIVJCVTVIKTorLC40Oz0/EXc3T/xAAZAQADAQEBAAAAAAAAAAAAAAAAAQIDBAX/xAAsEQACAgEDBAAFAwUAAAAAAAAAAQIRAxIhMQQTQVEiMnGBoTNSYRQjQpHw/9oADAMBAAIRAxEAPwD0heLxmz23GVwhK+L2iuXQa/XHKu4Zj7TJ3ZZZV3TK2+jHKCp19mN68hw30h4hcwuIjhoMgEHOqKEbf7SKT74q7wHbvDv3ouoyZ7qXFEZxA7sMv8RCHlzqqQj0LFdn8HdnNZtnfVRlPnqsa1mOL9gsKQSjuh9Qw/ET+NFw3G8G5m3iEAW5IC3MujJEhSQPac8uRqBj+NgARikPiuLqyHmSp0I5L+NPQxnnPEcEtt2W3cZ8rMp8BUyNDpMRMjntUU4Vx9U+8Vub3FsuRBbz7TlMatBLARl+sJJM60TD4stcKNbjyMtI5nRY59etTqXcUfHs3WJabfJgUIOhWKelnL4kZkbqpI+VWHG8Jbt4ggMUMA7+7Y+lbLsVfwOIw3+d/s3eK7KSclskQMp8OX4+VXpWpr0c7sy2G7S4y2ADfF1RstwZo95mB6VNw/bW8oH7rUfWRpHOJBPUj7vnNbm72HwF0ZrZYA7FLgYe7NmqmxP0ZqWZbWIZfCphkmZLaGGH2elGk"",
        ""price"": ""5""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-12T11:07"",
      ""endDate"": ""2024-03-12T11:09"",
      ""status"": ""finished"",
      ""id"": ""8f3d4281-8d9d-4536-97a4-9ebbcab43717""
    },
    {
      ""sellerId"": ""5"",
      ""sellerName"": ""u5"",
      ""itemDetails"": {
        ""title"": ""game 15"",
        ""description"": ""old game"",
        ""image"": ""data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAoHCBUVFBgVFRUYGBgaGxsdGhsbGyAeJBwgIBsgIBobHR0jIi0kIB0pIBgdJTclKS4wNDQ0ICM5PzkyPi4yNDABCwsLEA8QHRISHTIpJCk2MjIyNTIyNTIyMjIyMDQyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMv/AABEIAKgBLAMBIgACEQEDEQH/xAAcAAABBQEBAQAAAAAAAAAAAAADAQIEBQYABwj/xABJEAACAQIEAwUEBwUDCwQDAAABAhEAAwQSITEFQVEGEyJhcTKBkbEHQlKSocHRFCNy4fAzYtIVJCVTVIKTorLC40Oz0/EXc3T/xAAZAQADAQEBAAAAAAAAAAAAAAAAAQIDBAX/xAAsEQACAgEDBAAFAwUAAAAAAAAAAQIRAxIhMQQTQVEiMnGBoTNSYRQjQpHw/9oADAMBAAIRAxEAPwD0heLxmz23GVwhK+L2iuXQa/XHKu4Zj7TJ3ZZZV3TK2+jHKCp19mN68hw30h4hcwuIjhoMgEHOqKEbf7SKT74q7wHbvDv3ouoyZ7qXFEZxA7sMv8RCHlzqqQj0LFdn8HdnNZtnfVRlPnqsa1mOL9gsKQSjuh9Qw/ET+NFw3G8G5m3iEAW5IC3MujJEhSQPac8uRqBj+NgARikPiuLqyHmSp0I5L+NPQxnnPEcEtt2W3cZ8rMp8BUyNDpMRMjntUU4Vx9U+8Vub3FsuRBbz7TlMatBLARl+sJJM60TD4stcKNbjyMtI5nRY59etTqXcUfHs3WJabfJgUIOhWKelnL4kZkbqpI+VWHG8Jbt4ggMUMA7+7Y+lbLsVfwOIw3+d/s3eK7KSclskQMp8OX4+VXpWpr0c7sy2G7S4y2ADfF1RstwZo95mB6VNw/bW8oH7rUfWRpHOJBPUj7vnNbm72HwF0ZrZYA7FLgYe7NmqmxP0ZqWZbWIZfCphkmZLaGGH2elGk"",
        ""price"": ""15""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-12T11:11"",
      ""endDate"": ""2024-03-12T11:13"",
      ""status"": ""finished"",
      ""id"": ""26d203bc-6343-43d9-945d-70af6feef67f""
    },
    {
      ""sellerId"": ""4"",
      ""sellerName"": ""u4"",
      ""itemDetails"": {
        ""title"": ""game 17"",
        ""description"": ""old game"",
        ""image"": ""https://www.operationsports.com/wp-content/uploads/2023/09/efootball-2024-latest-mobile-menu-v0-t1i8bb900fmb1.jpg?w=1536"",
        ""price"": ""17""
      },
      ""bids"": [],
      ""startDate"": ""2024-03-12T11:26"",
      ""endDate"": ""2024-03-12T11:28"",
      ""status"": ""finished"",
      ""id"": ""ff201214-9d47-4e5e-8c82-440725ba9047""
    }
  ]
");

app.MapDelete("/users/{userID}", (string userID) => {
    using var db = new MySqlConnection(connectionString);
    try
    {
        db.Open();
        Users.DeleteUser(db, userID);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Something went wrong!");
    }
});

app.MapPut("/users/{userID}", (string userID, User updatedUser) =>
{
    using var db = new MySqlConnection(connectionString);
    try
    {
        db.Open();
        updatedUser.UserID = userID;
        Users.UpdateUser(db, updatedUser);
        return Results.Ok(updatedUser);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Something went wrong");
    }
});


app.MapGet("/", () =>
{
    return "Hello World!";
});
app.MapGet("/userss/", () =>
{
    using var db = new MySqlConnection(connectionString);
    try
    {
        db.Open();
        var users = Users.GetAllUsers(db);
        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("An error occurred while processing your request.");
    }
});


app.Run("http://localhost:3000");
