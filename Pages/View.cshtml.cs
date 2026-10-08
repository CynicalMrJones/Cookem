
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookem.Pages;

public class ViewModel : PageModel
{

    public Sql database = new Sql();

    [BindProperty]
    public string? Options { get; set; }

    [BindProperty]
    public string? Ans{ get; set; }

    [BindProperty]
    public string? Search{ get; set; }

    [BindProperty]
    public string? Text{ get; set; }

    [BindProperty]
    public string? IngredientsText{ get; set; }

    public bool IsPost { get; set; }

    public List<string>? Recipes { set; get;}

    private List<string> GetOptions(){
        List<string> strs = database.GetRecipeNames();
        return strs;
    }

    public string GetIngredientsText(string recipe_name){
        List<string> list = database.GetRecipeIngredients(recipe_name);
        string ret = "";
        foreach(var ent in list){
            ret += ent + "\n";
        }
        return ret;
    }


    public string GetText(string name){
        string ret = database.GetRecipeInstructions(name);
        return ret;
    }

    public void OnGet()
    {
        database.connect("test.db");
        IsPost = true;
        this.Recipes = GetOptions();
    }

    public void OnPost()
    {
        database.connect("test.db");
        Text = GetText(Ans);
        IngredientsText = GetIngredientsText(Ans);
        this.Recipes = GetOptions();
    }
}
