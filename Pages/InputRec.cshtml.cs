
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookem.Pages;

public class InputModel : PageModel
{

    public Sql database = new Sql();

    [BindProperty]
    public string? Name_ingredient{ get; set; }

    [BindProperty]
    public string? Name{ get; set; }

    [BindProperty]
    public string? Instructions{ get; set; }

    [BindProperty]
    public string? Ingredients{ get; set; }

    [BindProperty]
    public string? Ans{ get; set; }

    [BindProperty]
    public string? IngResult{ get; set; }

    public bool IsPost { get; set; }

    private List<int> ParseAndInsertIngredients(string ingredients){
        string[] sub = ingredients.Split(",");
        List<int> ingredientIds = new List<int>();
        foreach(var ent in sub){
            int ingredientId = database.InsertIngredient(ent.Trim());
            ingredientIds.Add(ingredientId);
        }
        return ingredientIds;
    }

    public string AddRecipe(string name, string instructions, string ingredients){
        database.connect("test.db");
        if (string.IsNullOrEmpty(name)){
            Ans = "Name is empty";
            return "Name is empty";
        }
        if (string.IsNullOrEmpty(instructions)){
            Ans = "Instructions is empty";
            return "Instructions is empty";
        }
        if (string.IsNullOrEmpty(ingredients)){
            Ans = "Ingredient is empty";
            return "Ingredient is empty";
        }
        int recipeId = database.InsertRecipe(name, instructions);
        if (recipeId == 0){
            Ans = "Failed to insert recipe";
            return Ans;
        }
        List<int> ingredientIds = ParseAndInsertIngredients(ingredients);
        //Need to add to recipe_ingredient table
        database.InsertRecipeIngredient(recipeId, ingredientIds);
        Ans = "Inserted into DB";
        return Ans;
    }


    public void OnGet()
    {
        IsPost = true;
    }
}
