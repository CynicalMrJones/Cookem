
using System.Data.SQLite;

public class Sql{

    //Private Connection
    //Only the class which constructed can have
    private SQLiteConnection? connection;
    private bool isConnected = false;

    //Used for connection to database
    public void connect(string db){
        string connect = $"Data Source={db};Version=3;";
        connection = new SQLiteConnection(connect);
        connection.Open();
        if (connection.Equals(null)){
            Console.WriteLine("Failed to Connect to Database");
            return;
        }
        Console.WriteLine("Connected To DataBase");
        isConnected = true;
    }

    public bool IsConnected(){
        if (isConnected != true){
            return false;
        }
        else{
            return true;
        }
    }

    //TODO: Add string stripping (get rid of unwanted characters)
    public string Insert_Rec(string name, string instruction){
        using var command = new SQLiteCommand(connection);
        //Check to see if name is already in db
        command.CommandText = "SELECT count(*) FROM recipe WHERE name=@name";
        command.Parameters.AddWithValue("@name", name.ToLower());
        int count = Convert.ToInt32(command.ExecuteScalar());
        if (count == 0){
            command.CommandText = "Insert INTO recipe VALUES(@id, @name, @instruction)";
            command.Parameters.AddWithValue("@id", 2);
            command.Parameters.AddWithValue("@name", name.ToLower());
            command.Parameters.AddWithValue("@instruction", instruction);
            command.Prepare();
            command.ExecuteNonQuery();
            return "Entry added Successfully";
        }
        else{
            return "Entry already found in database";
        }
    }

    public List<string> GetRec(){
        List<string> ret = new List<string>();
        using var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT * FROM recipe";
        using var reader = command.ExecuteReader();
        while(reader.Read()){
            ret.Add(reader.GetString(1));
        }
        foreach(var meme in ret){
            Console.WriteLine(meme);
        }
        return ret;
    }

    //Prints out the whole database (all Tables)
    public void Printdb(){
        List<string> arr = GetTables();
        using var command = new SQLiteCommand(connection);

        foreach(var item in arr){
            if(item == "ingredients"){
                command.CommandText = $@"SELECT * FROM {item};";

                using var reader = command.ExecuteReader();

                Console.WriteLine($"From {item}");
                while(reader.Read()){
                    var id = reader.GetInt32(0);
                    var text = reader.GetString(1);
                    Console.WriteLine($"{id}, {text}");
                }
                Console.WriteLine();
            }
            else{
                command.CommandText = $@"SELECT * FROM {item};";

                using var reader = command.ExecuteReader();

                Console.WriteLine($"From {item}");
                while(reader.Read()){
                    var id = reader.GetInt32(0);
                    var text = reader.GetString(1);
                    var text2 = reader.GetString(2);
                    Console.WriteLine($"{id}, {text}, {text2}");
                }
                Console.WriteLine();
            }
        }
    }

    //Prints All tables in db
    public void PrintTables(){
        using var command = new SQLiteCommand(connection);

        command.CommandText = @"SELECT distinct tbl_name FROM sqlite_master;";
        using var reader = command.ExecuteReader();

        while(reader.Read()){
            var name = reader.GetString(0);
            Console.WriteLine(name);
        }
    }

    //Called by Printdb() to get all tables as a list
    private List<string> GetTables(){
        using var command = new SQLiteCommand(connection);
        List<string> arr = new List<string>();

        command.CommandText = @"SELECT distinct tbl_name FROM sqlite_master;";
        using var reader = command.ExecuteReader();
        while(reader.Read()){
            var name = reader.GetString(0);
            arr.Add(name);
        }
        return arr;
    }
}
