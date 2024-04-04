using MySql.Data.MySqlClient;

namespace Server;

public record User(int Id, string UserId, string Username, string Password, string Type, string Email, string Phone, string PersonalNumber, string Firstname, string Lastname);


public static class Users
{
    public static List<User> All(State state)
    {
        var userList = new List<User>();

        using var reader = MySqlHelper.ExecuteReader("server=localhost;uid=root;pwd=mypassword;database=AuctionDatabase;", "SELECT * FROM Users");


        while (reader.Read())
        {
            var user = new User(
            reader.GetInt32("id"),
            reader.GetString("userID"),
            reader.GetString("username"),
            reader.GetString("password"),
            reader.GetString("Type"),
            reader.GetString("email"),
            reader.GetString("phone"),
            reader.GetString("personalNumber"),
            reader.GetString("firstname"),
            reader.GetString("lastname")
        );
            userList.Add(user);
        };


        return userList;

    }


    public static User AddUser(User user, State state)
    {
        var userUuid = Guid.NewGuid().ToString();
        MySqlCommand command = new("INSERT INTO Users (userID, username, password, type, email, phone, personalNumber, firstname, lastname) " +
                                              "VALUES (@userID, @username, @password, @type, @email, @phone, @personalNumber, @firstname, @lastname) returning id;", state.DB);


        command.Parameters.AddWithValue("@userID", userUuid);
        command.Parameters.AddWithValue("@username", user.Username);
        command.Parameters.AddWithValue("@password", user.Password);
        command.Parameters.AddWithValue("@type", user.Type);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@phone", user.Phone);
        command.Parameters.AddWithValue("@personalNumber", user.PersonalNumber);
        command.Parameters.AddWithValue("@firstname", user.Firstname);
        command.Parameters.AddWithValue("@lastname", user.Lastname);

        var id = command.ExecuteScalar();


        if (id is int i)
        {
            user = user with { Id = i };
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
            command.Parameters.AddWithValue("@userID", user.Id);
            command.ExecuteNonQuery();
        }
    }
}