using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Server
{
    public record Item(int Id, String ItemID, string SellerId, string SellerName, 
        DateTime StartDate, DateTime EndDate, string Status, ItemDetails ItemDetails,List<Bid> Bids);

    public record Bid(int Id, string BidderID, string ItemID, double BidPrice, DateTime BidTime);

    public record ItemDetails(int Id, string Description, float Price, string ItemID, string Title);
  
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
        public static void AddItem(Item item,State state)
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

        //public static void DeleteItem(string ItemID)
        //{
        //    using var conn = new MySqlConnection("server=localhost;uid=root;pwd=Rd0671rd..;database=AuctionDatabase2;");
        //    conn.Open();

        //    using (var cmdDeleteDetails = new MySqlCommand("DELETE FROM ItemDetails WHERE itemID = @itemID", conn))
        //    {
        //        cmdDeleteDetails.Parameters.AddWithValue("@itemID", ItemID);
        //        cmdDeleteDetails.ExecuteNonQuery();
        //    }

        //    using (var cmdDeleteItem = new MySqlCommand("DELETE FROM Items WHERE itemID = @itemID", conn))
        //    {
        //        cmdDeleteItem.Parameters.AddWithValue("@itemID", ItemID);
        //        cmdDeleteItem.ExecuteNonQuery();
        //    }
        //}
        //public static void UpdateAuction(string itemID, Item item)
        //{
        //    using var conn = new MySqlConnection("server=localhost;uid=root;pwd=Rd0671rd..;database=AuctionDatabase2;");
        //    conn.Open();

        //    using (var cmd1 = new MySqlCommand("UPDATE Items SET startDate = @startDate, endDate = @endDate WHERE itemID = @itemID", conn))
        //    {
        //        cmd1.Parameters.AddWithValue("@startDate", item.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
        //        cmd1.Parameters.AddWithValue("@endDate", item.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
        //        cmd1.Parameters.AddWithValue("@itemID", itemID);
        //        cmd1.ExecuteNonQuery();
        //    }

        //    using (var cmd2 = new MySqlCommand("UPDATE ItemDetails SET title = @title, description = @description, price = @price WHERE itemID = @itemID", conn))
        //    {
        //        cmd2.Parameters.AddWithValue("@title", item.ItemDetails.Title);
        //        cmd2.Parameters.AddWithValue("@description", item.ItemDetails.Description);
        //        cmd2.Parameters.AddWithValue("@price", item.ItemDetails.Price);
        //        cmd2.Parameters.AddWithValue("@itemID", itemID);
        //        cmd2.ExecuteNonQuery();
        //    }
        //}



    }
}
