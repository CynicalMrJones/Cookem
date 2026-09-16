
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
