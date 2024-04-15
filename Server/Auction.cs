using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;

namespace Server
{
  public record Item(int Id, String ItemID, string SellerId, string SellerName,
        DateTime StartDate, DateTime EndDate, string Status, ItemDetails ItemDetails, List<Bid> Bids);

  public record Bid(int Id, string BidderID, string ItemID, double BidPrice, DateTime BidTime);

  public record ItemDetails(int Id, string Description, float Price, string ItemID, string Title);

  public record SoldItems(int Id, String ItemID, string SellerId, string SellerName,
                DateTime StartDate, DateTime EndDate);

    public static class AuctionManager
  {
    public static List<Item> GetAllItems(State state)
    {
      var itemList = new List<Item>();
      var Bids = new List<Bid>();
      var query = @"SELECT i.id, i.itemID, i.sellerId, i.sellerName, i.startDate, i.endDate, i.status,
                                id.description, id.price, id.title FROM Items i 
                                JOIN ItemDetails id ON i.itemID = id.itemID";
      using var reader = MySqlHelper.ExecuteReader(state.DB, query);


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


      var cmd1 = "INSERT INTO Items (itemID, sellerId, sellerName, startDate, endDate, status) VALUES (@ItemID, @SellerId, @SellerName, @StartDate, @EndDate, @Status); select LAST_INSERT_ID();";
      var cmd2 = "INSERT INTO ItemDetails (itemID, description, price, title) VALUES (@ItemID, @Description, @Price, @Title);select LAST_INSERT_ID();";

      var uuid = Guid.NewGuid().ToString();
      var result1 = MySqlHelper.ExecuteScalar(state.DB, cmd1, [new MySqlParameter("@ItemID", uuid), new MySqlParameter("@sellerID",item.SellerId),
             new MySqlParameter("@sellerName",item.SellerName),new MySqlParameter("@startDate",item.StartDate), new MySqlParameter("@endDate",item.EndDate),
             new MySqlParameter("@status",item.Status)
      ]);

      var result2 = MySqlHelper.ExecuteScalar(state.DB, cmd2, [new MySqlParameter("@ItemID", uuid),
                new MySqlParameter("@description",item.ItemDetails.Description),
             new MySqlParameter("@price",item.ItemDetails.Price),
                new MySqlParameter("@title",item.ItemDetails.Title)

      ]);
    }


    public static void DeleteItem(string ItemID, State state)
    {
      var cmd1 = "DELETE FROM ItemDetails WHERE itemID = @ItemID";
      var cmd2 = "DELETE FROM Items WHERE itemID = @ItemID";
      MySqlHelper.ExecuteNonQuery(state.DB, cmd1, new MySqlParameter("@ItemID", ItemID));
      MySqlHelper.ExecuteNonQuery(state.DB, cmd2, new MySqlParameter("@ItemID", ItemID));

    }
    public static void UpdateAuction(string itemID, Item item, State state)
    {

      var cmd1 = "UPDATE Items SET startDate = @startDate, endDate = @endDate WHERE itemID = @itemID";
      var cmd2 = "UPDATE ItemDetails SET title = @title, description = @description, price = @price WHERE itemID = @itemID";
      MySqlHelper.ExecuteNonQuery(state.DB, cmd1, [new MySqlParameter("@startDate", item.StartDate),
                                                    new MySqlParameter( "@endDate",item.EndDate),
                                                    new MySqlParameter("@itemID", itemID)]);
      MySqlHelper.ExecuteNonQuery(state.DB, cmd2, [new MySqlParameter("@title", item.ItemDetails.Title),
                                                    new MySqlParameter( "@description",item.ItemDetails.Description),
                                                     new MySqlParameter( "@price",item.ItemDetails.Price),
                                                    new MySqlParameter("@itemID", itemID)]);
    }
    public static List<Bid> GetBidHistoryForAuction(string auctionId, State state)
    {
      List<Bid> bidHistory = new List<Bid>();


      string query = "SELECT * FROM Bids WHERE ItemId = @ItemId";
      using var reader = MySqlHelper.ExecuteReader(state.DB, query, [new("@ItemId", auctionId)]);


      while (reader.Read())
      {
        Bid bid = new Bid(
            reader.GetInt32("Id"),
            reader.GetString("BidderId"),
            reader.GetString("ItemId"),
            reader.GetDouble("BidPrice"),
            reader.GetDateTime("BidTime")
        );

        bidHistory.Add(bid);
      }



      return bidHistory;


    }

        public static List<Item> GetSoldItems(State state)
        {
            List<Item> soldItems = new List<Item>();
            string query = @"SELECT i.Id, i.ItemID, i.SellerId, i.SellerName, i.StartDate, i.EndDate, i.Status,
             id.Description, id.Price, id.Title
             FROM Items i
             JOIN ItemDetails id ON i.ItemID = id.ItemID
             WHERE i.Status = 'Sold'";

            using var reader = MySqlHelper.ExecuteReader(state.DB, query);

            while (reader.Read())
            {
                var itemDetails = new ItemDetails(
                    reader.GetInt32("Id"),
                    reader.GetString("Description"),
                    reader.GetFloat("Price"),
                    reader.GetString("ItemID"),
                    reader.GetString("Title")
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
                soldItems.Add(item);
            }

            return soldItems;
        }






    }
}
