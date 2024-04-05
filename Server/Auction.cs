using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Server
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemID { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public ItemDetails? ItemDetails { get; set; }
       
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

            using var conn = new MySqlConnection("server=localhost;uid=root;pwd=Rd0671rd..;database=AuctionDatabase;");
            conn.Open();

            var query = "SELECT * FROM Items";
            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var item = new Item
                {
                    Id = reader.GetInt32("id"),
                    ItemID = reader.GetString("itemID"),
                    SellerId = reader.GetInt32("sellerId"),
                    SellerName = reader.GetString("sellerName"),
                    StartDate = DateTime.Parse(reader.GetString("startDate")),
                    EndDate = DateTime.Parse(reader.GetString("endDate")),
                    Status = reader.GetString("status"),
                    ItemDetails = new ItemDetails
                    {
                        Id = reader.GetInt32("itemDetails_id"),
                        Description = reader.GetString("description"),
                        Price = reader.GetFloat("price"),
                        ItemID = reader.GetString("itemID"),
                        Title = reader.GetString("title")
                    }
                };
                itemList.Add(item);
            }

            return itemList;
        }

        public static void AddItem(Item item)
        {
            using var conn = new MySqlConnection("server=localhost;uid=root;pwd=Rd0671rd..;database=AuctionDatabase;");
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

        
    }
}
