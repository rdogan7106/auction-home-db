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
string connectionString = "server=localhost;uid=root;pwd=Rd0671rd..;database=exampleNet;port=3306";

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
app.MapGet("/users", () =>
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


app.Run();
