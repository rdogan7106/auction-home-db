using MySql.Data.MySqlClient;

namespace Server
{
    public class User
    {
        public int Id { get; set; }
        public string? UserID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Type { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? PersonalNumber { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }

    }

    public static class Users
    {
        public static List<User> GetAllUsers(MySqlConnection conn)
        {
            var userList = new List<User>();

            using (var command = new MySqlCommand("SELECT * FROM Users", conn))
            {
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var user = new User
                    {
                        Id = reader.GetInt32("id"),
                        UserID = reader.GetString("userID"),
                        Username = reader.GetString("username"),
                        Password = reader.GetString("password"),
                        Type = reader.GetString("type"),
                        Email = reader.GetString("email"),
                        Phone = reader.GetString("phone"),
                        PersonalNumber = reader.GetInt32("personalNumber"),
                        Firstname = reader.GetString("firstname"),
                        Lastname = reader.GetString("lastname")
                    };
                    userList.Add(user);
                }
            }

            return userList;
        }
        public static User AddUser(MySqlConnection conn, User user)
        {
            using (var command = new MySqlCommand("INSERT INTO Users (userID, username, password, type, email, phone, personalNumber, firstname, lastname) " +
                                                   "VALUES (@userID, @username, @password, @type, @email, @phone, @personalNumber, @firstname, @lastname); " +
                                                   "SELECT LAST_INSERT_ID();", conn))
            {
                command.Parameters.AddWithValue("@userID", user.UserID);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@password", user.Password);
                command.Parameters.AddWithValue("@type", user.Type);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@phone", user.Phone);
                command.Parameters.AddWithValue("@personalNumber", user.PersonalNumber);
                command.Parameters.AddWithValue("@firstname", user.Firstname);
                command.Parameters.AddWithValue("@lastname", user.Lastname);

                int userId = Convert.ToInt32(command.ExecuteScalar());

                user.Id = userId;

                return user;
            }
        }

        public static void DeleteUser(MySqlConnection conn, string userID)
        {
            using var command = new MySqlCommand("delete from Users where userID = @userID", conn);
            command.Parameters.AddWithValue("@userID", userID);
            command.ExecuteNonQuery();
        }

        public static void UpdateUser(MySqlConnection conn, User user)
        {
            using var command = new MySqlCommand("UPDATE Users SET username = @username, password = @password, type = @type, " +
                                                 "email = @email, phone = @phone, personalNumber = @personalNumber, " +
                                                 "firstname = @firstname, lastname = @lastname WHERE userID = @userID", conn);
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@type", user.Type);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@phone", user.Phone);
            command.Parameters.AddWithValue("@personalNumber", user.PersonalNumber);
            command.Parameters.AddWithValue("@firstname", user.Firstname);
            command.Parameters.AddWithValue("@lastname", user.Lastname);
            command.Parameters.AddWithValue("@userID", user.UserID);
            command.ExecuteNonQuery();
        }

    }
}
