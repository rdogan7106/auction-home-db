using MySql.Data.MySqlClient;

namespace Server;

public class User
{
    public int? Id { get; set; }
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
    public static List<User> All(State state)
    {
        var userList = new List<User>();

        // MySqlCommand command = new("SELECT * FROM Users", state.DB);

        using (var connection = new MySqlConnection(state.DB.ConnectionString))
        {
            connection.Open();
            using (var command = new MySqlCommand("SELECT * FROM Users", connection))
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
        }     
        
        return userList;
    }
    public static User AddUser(User user, State state)
    {
        using (var connection = new MySqlConnection(state.DB.ConnectionString))
        {
            connection.Open();
            var userUuid = Guid.NewGuid().ToString();
            using (var command = new MySqlCommand("INSERT INTO Users (userID, username, password, type, email, phone, personalNumber, firstname, lastname) " +
                                                  "VALUES (@userID, @username, @password, @type, @email, @phone, @personalNumber, @firstname, @lastname);", connection))
            {
                command.Parameters.AddWithValue("@userID", userUuid);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@password", user.Password);
                command.Parameters.AddWithValue("@type", user.Type);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@phone", user.Phone);
                command.Parameters.AddWithValue("@personalNumber", user.PersonalNumber);
                command.Parameters.AddWithValue("@firstname", user.Firstname);
                command.Parameters.AddWithValue("@lastname", user.Lastname);

                command.ExecuteNonQuery();
            }

            using (var idCommand = new MySqlCommand("SELECT LAST_INSERT_ID();", connection))
            {
                var result = idCommand.ExecuteScalar();
                if (result != null)
                {
                    user.Id = Convert.ToInt32(result);
                }
            }
        }
            return user;
    }

    public static void DeleteUser(string userID, State state)
    {
        using (var connection = new MySqlConnection(state.DB.ConnectionString))
        {
            connection.Open();
            using (var command = new MySqlCommand("delete from Users where userID = @userID", connection))
            {
                command.Parameters.AddWithValue("@userID", userID);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void UpdateUser(User user, State state)
    {
        using (var connection = new MySqlConnection(state.DB.ConnectionString))
        {
            connection.Open();
            using var command = new MySqlCommand("UPDATE Users SET username = @username, password = @password, type = @type, " +
                                             "email = @email, phone = @phone, personalNumber = @personalNumber, " +
                                             "firstname = @firstname, lastname = @lastname WHERE userID = @userID", connection);
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
