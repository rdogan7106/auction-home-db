using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MySql.Data.MySqlClient;
using Server;
string connectionString = "server=127.0.0.1;uid=root;pwd=your-password;database=AuctionDatabase2;port=3306";


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(3000);
});
State state = new(connectionString);
builder.Services.AddSingleton(state);
var app = builder.Build();

// För att köra lokalt igen utan distmap ta bort rad 19 -> 33, samt 68 -> 82, och lägg till "http://localhost:3000" i app.run på rad 84
var distPath = Path.Combine(app.Environment.ContentRootPath, "../../AuctionHomeGroup2/dist");
var fileProvider = new PhysicalFileProvider(distPath);

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = fileProvider,
    DefaultFileNames = new List<string> { "index.html" }
});

// Här deklarerar vi att vår app ska hantera statiska filer (vår distmapp)
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = fileProvider,
    RequestPath = ""
});

app.UseRouting();

app.MapGet("/api/auctions/{itemId}/bidHistory",

 AuctionManager.GetBidHistoryForAuction
);
app.MapPost("/api/bids", AuctionManager.AddBid);





app.MapPost("/api/users", Users.AddUser);
app.MapGet("/api/users", Users.All);
app.MapDelete("/api/users/{userID}", Users.DeleteUser);
app.MapPut("/api/users/{userID}", Users.UpdateUser);

app.MapGet("/api/auctions", AuctionManager.GetAllItems);
app.MapPost("/api/auctions", AuctionManager.AddItem);
app.MapDelete("/api/auctions/{ItemID}", AuctionManager.DeleteItem);
app.MapPut("/api/auctions/{ItemID}", AuctionManager.UpdateAuction);
app.MapGet("/api/auctions/{status}", AuctionManager.GetSoldItems);






app.MapGet("/", () =>
{
  return "Hello World!";
});

app.MapFallback(async context =>
{

    string path = context.Request.Path.Value;

    if (!path.StartsWith("/api/"))
    {
        context.Response.ContentType = "text/html";
        await context.Response.SendFileAsync(Path.Combine(distPath, "index.html"));

    }

   

});

app.Run();
public record State(string DB);
