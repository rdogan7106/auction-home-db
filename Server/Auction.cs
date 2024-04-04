using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Server;

public record Auction(string Id, int SellerId, int ItemId, string SellerName, string Title, string Description, string Image, float Price, string StartDate, string EndDate);

public static class Auctions
{
  public static List<Auction> All(State state)
  {
    var auctionList = new List<Auction>();

    using var reader = MySqlHelper.ExecuteReader("server=localhost;uid=root;pwd=mypassword;database=AuctionDatabase;", "SELECT * FROM Items");
    while (reader.Read())
    {
      var auction = new Auction
      (reader.GetString("id"),
       reader.GetInt32("sellerId"),
       reader.GetInt32("itemId"),
       reader.GetString("sellerName"),
       reader.GetString("title"),
       reader.GetString("description"),
       reader.GetString("image"),
       reader.GetFloat("price"),
       reader.GetString("startDate"),
       reader.GetString("endDate")
        );
      auctionList.Add(auction);

    }

    return auctionList;
  }

  public static int AddAuction(MySqlConnection conn, Auction auction)
  {
    var command = new MySqlCommand("INSERT INTO Items (sellerId, itemId, sellerName, Title, Description, Image, Price, StartDate, EndDate) " +
                                           "VALUES (@sellerId, @itemId, @sellerName, @Title, @Description, @Image, @Price, @StartDate, @EndDate); " +
                                           "SELECT LAST_INSERT_ID();", conn);

    command.Parameters.AddWithValue("@sellerId", auction.SellerId);
    command.Parameters.AddWithValue("@itemId", auction.ItemId);
    command.Parameters.AddWithValue("@sellerName", auction.SellerName);
    command.Parameters.AddWithValue("@Title", auction.Title);
    command.Parameters.AddWithValue("@Description", auction.Description);
    command.Parameters.AddWithValue("@Image", auction.Image);
    command.Parameters.AddWithValue("@Price", auction.Price);
    command.Parameters.AddWithValue("@StartDate", auction.StartDate);
    command.Parameters.AddWithValue("@EndDate", auction.EndDate);

    return Convert.ToInt32(command.ExecuteScalar());

  }

  public static void DeleteAuction(MySqlConnection conn, string auctionId)
  {
    var command = new MySqlCommand("DELETE FROM Items WHERE Id = @Id", conn);
    command.Parameters.AddWithValue("@Id", auctionId);
    command.ExecuteNonQuery();
  }

  public static void UpdateAuction(MySqlConnection conn, Auction auction)
  {
    var command = new MySqlCommand("UPDATE Items SET sellerId = @sellerId, itemId = @itemId, sellerName = @sellerName, " +
                                        "Title = @Title, Description = @Description, Image = @Image, " +
                                        "Price = @Price, StartDate = @StartDate, EndDate = @EndDate WHERE Id = @Id", conn);
    command.Parameters.AddWithValue("@sellerId", auction.SellerId);
    command.Parameters.AddWithValue("@itemId", auction.ItemId);
    command.Parameters.AddWithValue("@sellerName", auction.SellerName);
    command.Parameters.AddWithValue("@Title", auction.Title);
    command.Parameters.AddWithValue("@Description", auction.Description);
    command.Parameters.AddWithValue("@Image", auction.Image);
    command.Parameters.AddWithValue("@Price", auction.Price);
    command.Parameters.AddWithValue("@StartDate", auction.StartDate);
    command.Parameters.AddWithValue("@EndDate", auction.EndDate);
    command.Parameters.AddWithValue("@Id", auction.Id);
    command.ExecuteNonQuery();
  }
}
