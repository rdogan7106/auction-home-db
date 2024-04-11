using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;

namespace Server
{
  public record Item
  {
    public int Id { get; set; }
    public string ItemID { get; set; }
    public string SellerId { get; set; }
    public string SellerName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
    public ItemDetails ItemDetails { get; set; }
    public List<Bid> Bids { get; set; } = new List<Bid>();
    public Item(int Id, string ItemID, string SellerID, string SellerName, DateTime StartDate, DateTime EndDate, string Status, ItemDetails itemDetails, List<Bid> bids)
    {
      this.Id = Id;
      this.ItemID = ItemID;
      this.SellerId = SellerID;
      this.SellerName = SellerName;
      this.StartDate = StartDate;
      this.EndDate = EndDate;
      this.Status = Status;
      this.ItemDetails = itemDetails;
      this.Bids = bids;
    }

  };

  public record Bid
  {
    public int Id { get; init; }
    public string BidderID { get; init; }
    public string ItemID { get; init; }
    public double BidPrice { get; init; }
    public DateTime BidTime { get; init; }

    public Bid(int id, string bidderID, string itemID, double bidPrice, DateTime bidTime)
    {
      this.Id = id;
      this.BidderID = bidderID;
      this.ItemID = itemID;
      this.BidPrice = bidPrice;
      this.BidTime = bidTime;
    }
  };

  public record ItemDetails
  {
    public int Id { get; init; }
    public string Description { get; init; }
    public float Price { get; init; }
    public string ItemID { get; init; }
    public string Title { get; init; }

    public ItemDetails(int id, string description, float price, string itemID, string title)
    {
      this.Id = id;
      this.Description = description;
      this.Price = price;
      this.ItemID = itemID;
      this.Title = title;
    }
  };

  public static class AuctionManager
  {
    public static List<Item> GetAllItems(State state)
    {
      var itemList = new List<Item>();
      var Bids = new List<Bid>();
      var query = "SELECT i.id, i.itemID, i.sellerId, i.sellerName, i.startDate, i.endDate, i.status," +
                               " id.description, id.price, id.title FROM Items i " +
                               "JOIN ItemDetails id ON i.itemID = id.itemID";
      using var reader = MySqlHelper.ExecuteReader(state.DB.ConnectionString, query);


      while (reader.Read())
      {
        var itemDetails = new ItemDetails(
                     reader.GetInt32("id"),
                     reader.GetString("description"),
                     reader.GetFloat("price"),
                     reader.GetString("itemID"),
                     reader.GetString("title")
                 );

        var item = new Item(
            reader.GetInt32("id"),
            reader.GetString("itemID"),
            reader.GetString("sellerId"),
            reader.GetString("sellerName"),
            reader.GetDateTime("startDate"),
            reader.GetDateTime("endDate"),
            reader.GetString("status"),
            itemDetails,
            new List<Bid>()
        );

        itemList.Add(item);

      }

      return itemList;

    }
    public static void AddItem(Item item, State state)
    {

      var conn = state.DB.ConnectionString;
      var cmd1 = "INSERT INTO Items (itemID, sellerId, sellerName, startDate, endDate, status) VALUES (@ItemID, @SellerId, @SellerName, @StartDate, @EndDate, @Status); select LAST_INSERT_ID();";
      var cmd2 = "INSERT INTO ItemDetails (itemID, description, price, title) VALUES (@ItemID, @Description, @Price, @Title);select LAST_INSERT_ID();";

      var uuid = Guid.NewGuid().ToString();
      var result1 = MySqlHelper.ExecuteScalar(conn, cmd1, [new MySqlParameter("@ItemID", uuid), new MySqlParameter("@sellerID",item.SellerId),
             new MySqlParameter("@sellerName",item.SellerName),new MySqlParameter("@startDate",item.StartDate), new MySqlParameter("@endDate",item.EndDate),
             new MySqlParameter("@status",item.Status)
      ]);

      var result2 = MySqlHelper.ExecuteScalar(conn, cmd2, [new MySqlParameter("@ItemID", uuid),
                new MySqlParameter("@description",item.ItemDetails.Description),
             new MySqlParameter("@price",item.ItemDetails.Price),
                new MySqlParameter("@title",item.ItemDetails.Title)

      ]);
    }


    public static void DeleteItem(string ItemID, State state)
    {
      var cmd1 = "DELETE FROM ItemDetails WHERE itemID = @ItemID";
      var cmd2 = "DELETE FROM Items WHERE itemID = @ItemID";
      MySqlHelper.ExecuteNonQuery(state.DB.ConnectionString, cmd1, new MySqlParameter("@ItemID", ItemID));
      MySqlHelper.ExecuteNonQuery(state.DB.ConnectionString, cmd2, new MySqlParameter("@ItemID", ItemID));

    }
    public static void UpdateAuction(string itemID, Item item, State state)
    {
      var conn = state.DB.ConnectionString;
      var cmd1 = "UPDATE Items SET startDate = @startDate, endDate = @endDate WHERE itemID = @itemID";
      var cmd2 = "UPDATE ItemDetails SET title = @title, description = @description, price = @price WHERE itemID = @itemID";
      MySqlHelper.ExecuteNonQuery(conn, cmd1, [new MySqlParameter("@startDate", item.StartDate),
                                                    new MySqlParameter( "@endDate",item.EndDate),
                                                    new MySqlParameter("@itemID", itemID)]);
      MySqlHelper.ExecuteNonQuery(conn, cmd2, [new MySqlParameter("@title", item.ItemDetails.Title),
                                                    new MySqlParameter( "@description",item.ItemDetails.Description),
                                                     new MySqlParameter( "@price",item.ItemDetails.Price),
                                                    new MySqlParameter("@itemID", itemID)]);
    }

  }
  public class BidManager
  {

  }
}
