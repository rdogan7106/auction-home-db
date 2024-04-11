using MySql.Data.MySqlClient;

namespace Server;

public record User
{
    public int Id { get; set; } 
    public string UserID { get; set; } 
    public string Username { get; set; }
    public string Password { get; set; }
    public string Type { get; set; } 
    public string Email { get; set; } 
    public string Phone { get; set; } 
    public int PersonalNumber { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; } 

    public User(int id, string userID, string username, string password, string type,
                string email, string phone, int personalNumber, string firstname, string lastname)
    {
        this.Id = id;
        this.UserID = userID;
        this.Username = username;
        this.Password = password;
        this.Type = type;
        this.Email = email;
        this.Phone = phone;
        this.PersonalNumber = personalNumber;
        this.Firstname = firstname;
        this.Lastname = lastname;
    }
};

public static class Users
{
    public static List<User> All(State state)
    {
        var userList = new List<User>();
        using var reader = MySqlHelper.ExecuteReader(state.DB.ConnectionString, "SELECT * FROM Users");
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
            reader.GetInt32("personalNumber"),
            reader.GetString("firstname"),
            reader.GetString("lastname")
        );
            userList.Add(user);
        };

        return userList;
    }

   
    public static IResult AddUser(User user, State state)
    {
        var conn = state.DB.ConnectionString;
        var cmd = "INSERT INTO Users (userID, username, password, type, email, phone, personalNumber, firstname, lastname)" +
            "VALUES (@userID, @username, @password, @type, @email, @phone, @personalNumber, @firstname, @lastname);  select LAST_INSERT_ID();";

        var result = MySqlHelper.ExecuteScalar(conn, cmd,
            [new MySqlParameter("@userID", Guid.NewGuid().ToString()), new MySqlParameter("@username",user.Username),
             new MySqlParameter("@password",user.Password),new MySqlParameter("@type",user.Type), new MySqlParameter("@email",user.Email),
             new MySqlParameter("@phone",user.Phone), new MySqlParameter("@personalNumber",user.PersonalNumber), new MySqlParameter("@firstname",user.Firstname),
             new MySqlParameter("@lastname",user.Lastname)
            ]

            );
        if (result is int id)
        {
            return TypedResults.Created(id.ToString());
        }
        else
        {
            return TypedResults.Problem("Some Thing Went wrong");
        }


    }

    
    public static void DeleteUser(string userID, State state)
  {
        var cmd1 = "DELETE FROM ItemDetails WHERE itemID IN (SELECT itemID FROM Items WHERE sellerID = @userID)";
        var cmd2 = "DELETE FROM Items WHERE sellerID = @userID";
        var cmd3 = "DELETE FROM Users WHERE userID = @userID";
        MySqlHelper.ExecuteNonQuery(state.DB.ConnectionString, cmd1, new MySqlParameter("@userID", userID));
        MySqlHelper.ExecuteNonQuery(state.DB.ConnectionString, cmd2, new MySqlParameter("@userID", userID));
        MySqlHelper.ExecuteNonQuery(state.DB.ConnectionString, cmd3, new MySqlParameter("@userID", userID));


    }

    public static void UpdateUser(string userID, User user, State state)
    {
        var conn = state.DB.ConnectionString;
        var cmd = "UPDATE Users SET username = @username, password = @password, type = @type, email = @email, " +
                                       "phone = @phone, personalNumber = @personalNumber," +
                                        "firstname = @firstname, lastname = @lastname WHERE userID = @userID";
       
        MySqlHelper.ExecuteNonQuery(conn, cmd, [new MySqlParameter("@username", user.Username),
                                                    new MySqlParameter( "@password",user.Password),
                                                    new MySqlParameter( "@type",user.Type),
                                                    new MySqlParameter( "@email",user.Email),
                                                     new MySqlParameter( "@phone",user.Phone),
                                                    new MySqlParameter( "@personalNumber",user.PersonalNumber),
                                                    new MySqlParameter( "@firstName",user.Firstname),
                                                    new MySqlParameter( "@lastName",user.Lastname),
                                                    new MySqlParameter( "@userID",userID),]);    
    }
}
