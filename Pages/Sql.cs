
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

    public int InsertRecipe(string name, string instruction){
        using var command = new SQLiteCommand(connection);
        //Check to see if name is already in recipe db
        command.CommandText = "SELECT count(*) FROM recipe WHERE name=@name";
        command.Parameters.AddWithValue("@name", name.ToLower());
        int count = Convert.ToInt32(command.ExecuteScalar());

        if (count == 0){
            command.CommandText = @"Insert INTO recipe VALUES(@id, @name, @instruction); SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("@id", null);
            command.Parameters.AddWithValue("@name", name.ToLower());
            command.Parameters.AddWithValue("@instruction", instruction);
            command.Prepare();
            int recipeId = Convert.ToInt32(command.ExecuteScalar());
            return recipeId;
        }
        else{
            return 0;
        }
    }

    public int InsertIngredient(string name){
        using var command = new SQLiteCommand(connection);
        //Check to see if name is already in db
        command.CommandText = "SELECT count(*) FROM ingredients WHERE name=@name";
        command.Parameters.AddWithValue("@name", name.ToLower());
        int count = Convert.ToInt32(command.ExecuteScalar());
        if (count == 0){
            command.CommandText = @"Insert INTO ingredients VALUES(@id, @name); SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("@id", null);
            command.Parameters.AddWithValue("@name", name.ToLower());
            command.Prepare();
            int ingredientId = Convert.ToInt32(command.ExecuteScalar());
            return ingredientId;
        }
        else{
            command.CommandText = "SELECT id FROM ingredients WHERE name = @name";
            command.Parameters.AddWithValue("@name", name.ToLower());
            command.Prepare();
            int ingredientId = Convert.ToInt32(command.ExecuteScalar());
            return ingredientId;
        }
    }

    public void InsertRecipeIngredient(int recipeId, List<int> ingredientIds){
        using var command = new SQLiteCommand(connection);
        foreach(var ent in ingredientIds){
            command.CommandText = "INSERT INTO recipe_ingredients VALUES(@recipeId, @ingredientId)";
            command.Parameters.AddWithValue("@recipeId", recipeId);
            command.Parameters.AddWithValue("@ingredientId", ent);
            command.Prepare();
            command.ExecuteNonQuery();
        }
    }

    //Returns the names of the recipes
    public List<string> GetRecipeNames(){
        List<string> ret = new List<string>();
        using var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT name FROM recipe";
        using var reader = command.ExecuteReader();
        while(reader.Read()){
            ret.Add(reader.GetString(0));
        }
        return ret;
    }

    //For getting the ingredients and instructions for a given recipe
    public string GetRecipeInstructions(string recipe_name){
        using var command = new SQLiteCommand(connection);
        string ans = "";
        command.CommandText = "SELECT instructions FROM recipe WHERE name = @name";
        command.Parameters.AddWithValue("@name", recipe_name);
        command.Prepare();
        using var reader = command.ExecuteReader();
        while(reader.Read()){
            ans = reader.GetString(0);
        }
        return ans;
    }

    //Gets all ingredient names based on recipe name
    public List<string> GetRecipeIngredients(string recipe_name){
        using var command = new SQLiteCommand(connection);
        List<string> ans = [];
        command.CommandText = @"SELECT i.name
                                FROM ingredients i
                                JOIN recipe_ingredients ri
                                    ON i.id = ri.ingredient_id
                                JOIN recipe r
                                    ON r.id = ri.recipe_id
                                WHERE r.name = @name";
        command.Parameters.AddWithValue("@name", recipe_name);
        command.Prepare();
        var reader = command.ExecuteReader();
        while(reader.Read()){
            ans.Add(reader.GetString(0));
        }
        return ans;
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
