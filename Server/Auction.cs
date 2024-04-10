using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Server
{
  public class Item
  {
    public int Id { get; set; }
    public string ItemID { get; set; }
    public string SellerId { get; set; }
    public string SellerName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
    public ItemDetails? ItemDetails { get; set; }
    public List<Bid>? Bids { get; set; }

  }

  public class Bid
  {
    public int Id { get; set; }
    public int BidderID { get; set; }
    public int ItemID { get; set; }
    public double BidPrice { get; set; }
    public DateTime BidTime { get; set; }

  }

  public class ItemDetails
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public string ItemID { get; set; }
    public string Title { get; set; }
  }

  public static class AuctionManager
  {
    public static List<Item> GetAllItems()
    {
      var itemList = new List<Item>();
      var Bids = new List<Bid>();

      using var conn = new MySqlConnection("server=localhost;uid=root;pwd=mypassword;database=AuctionDatabase;");
      conn.Open();

      var query = "SELECT i.id, i.itemID, i.sellerId, i.sellerName, i.startDate, i.endDate, i.status," +
                               " id.description, id.price, id.title FROM Items i " +
                               "JOIN ItemDetails id ON i.itemID = id.itemID";
      using var cmd = new MySqlCommand(query, conn);
      using var reader = cmd.ExecuteReader();

      while (reader.Read())
      {
        var item = new Item
        {
          Id = reader.GetInt32("id"),
          ItemID = reader.GetString("itemID"),
          SellerId = reader.GetString("sellerId"),
          SellerName = reader.GetString("sellerName"),
          StartDate = DateTime.Parse(reader.GetString("startDate")),
          EndDate = DateTime.Parse(reader.GetString("endDate")),
          Status = reader.GetString("status"),
          ItemDetails = new ItemDetails
          {
            Description = reader.GetString("description"),
            Price = reader.GetFloat("price"),
            Title = reader.GetString("title")
          },
          Bids = Bids = new List<Bid>()
        };
        itemList.Add(item);
      }

      return itemList;
    }

    public static void AddItem(Item item)
    {
      using var conn = new MySqlConnection("server=localhost;uid=root;pwd=mypassword;database=AuctionDatabase;");
      conn.Open();
      var ItemUuid = Guid.NewGuid().ToString();
      using var cmd = new MySqlCommand("INSERT INTO Items (itemID, sellerId, sellerName, startDate, endDate, status) VALUES (@ItemID, @SellerId, @SellerName, @StartDate, @EndDate, @Status)", conn);
      cmd.Parameters.AddWithValue("@ItemID", ItemUuid);
      cmd.Parameters.AddWithValue("@SellerId", item.SellerId);
      cmd.Parameters.AddWithValue("@SellerName", item.SellerName);
      cmd.Parameters.AddWithValue("@StartDate", item.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
      cmd.Parameters.AddWithValue("@EndDate", item.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
      cmd.Parameters.AddWithValue("@Status", item.Status);


      using var cmd2 = new MySqlCommand("INSERT INTO ItemDetails (itemID, description, price, title) VALUES (@ItemID, @Description, @Price, @Title)", conn);
      cmd2.Parameters.AddWithValue("@ItemID", ItemUuid);
      cmd2.Parameters.AddWithValue("@Description", item.ItemDetails.Description);
      cmd2.Parameters.AddWithValue("@Price", item.ItemDetails.Price);
      cmd2.Parameters.AddWithValue("@Title", item.ItemDetails.Title);


      cmd.ExecuteNonQuery();
      cmd2.ExecuteNonQuery();

    }

    public static void DeleteItem(string ItemID)
    {
      using var conn = new MySqlConnection("server=localhost;uid=root;pwd=mypassword;database=AuctionDatabase;");
      conn.Open();

      using (var cmdDeleteDetails = new MySqlCommand("DELETE FROM ItemDetails WHERE itemID = @itemID", conn))
      {
        cmdDeleteDetails.Parameters.AddWithValue("@itemID", ItemID);
        cmdDeleteDetails.ExecuteNonQuery();
      }

      using (var cmdDeleteItem = new MySqlCommand("DELETE FROM Items WHERE itemID = @itemID", conn))
      {
        cmdDeleteItem.Parameters.AddWithValue("@itemID", ItemID);
        cmdDeleteItem.ExecuteNonQuery();
      }
    }
    public static void UpdateAuction(string itemID, Item item)
    {
      using var conn = new MySqlConnection("server=localhost;uid=root;pwd=mypassword;database=AuctionDatabase;");
      conn.Open();

      using (var cmd1 = new MySqlCommand("UPDATE Items SET startDate = @startDate, endDate = @endDate WHERE itemID = @itemID", conn))
      {
        cmd1.Parameters.AddWithValue("@startDate", item.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd1.Parameters.AddWithValue("@endDate", item.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd1.Parameters.AddWithValue("@itemID", itemID);
        cmd1.ExecuteNonQuery();
      }

      using (var cmd2 = new MySqlCommand("UPDATE ItemDetails SET title = @title, description = @description, price = @price WHERE itemID = @itemID", conn))
      {
        cmd2.Parameters.AddWithValue("@title", item.ItemDetails.Title);
        cmd2.Parameters.AddWithValue("@description", item.ItemDetails.Description);
        cmd2.Parameters.AddWithValue("@price", item.ItemDetails.Price);
        cmd2.Parameters.AddWithValue("@itemID", itemID);
        cmd2.ExecuteNonQuery();
      }
    }



  }
}
