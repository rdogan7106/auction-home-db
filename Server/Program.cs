using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Server;
string connectionString = "server=localhost;uid=root;pwd=Rd0671rd..;database=AuctionDatabase2;port=3306";

var builder = WebApplication.CreateBuilder(args);
State state = new(new(connectionString));
builder.Services.AddSingleton(state);
var app = builder.Build();

;
app.MapPost("/users", Users.AddUser);
app.MapGet("/users", Users.All);
app.MapDelete("/users/{userID}", Users.DeleteUser);
app.MapPut("/users/{userID}", Users.UpdateUser);

app.MapGet("/auctions", AuctionManager.GetAllItems);
app.MapPost("/auctions", AuctionManager.AddItem);
app.MapDelete("/auctions/{ItemID}",AuctionManager.DeleteItem);
//app.MapPut("/auctions/{ItemID}", AuctionManager.UpdateAuction);



app.MapGet("/", () =>
{
  return "Hello World!";
});

app.Run("http://localhost:3000");
public record State(MySqlConnection DB);