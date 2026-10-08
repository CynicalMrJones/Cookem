
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookem.Pages;

public class ViewModel : PageModel
{

    public Sql meme = new Sql();

    [BindProperty]
    public string? Options { get; set; }

    [BindProperty]
    public string? Ans{ get; set; }

    [BindProperty]
    public string? Text{ get; set; }

    [BindProperty]
    public string? IngredientsText{ get; set; }

    public bool IsPost { get; set; }

    public List<SelectListItem>? Recipes { set; get;}

    private List<SelectListItem> GetOptions(){
        List<string> strs = meme.GetRecipeNames();

        return strs.Select(x => new SelectListItem {
                Text = x,
                Value = x
        }).ToList();
    }

    public string GetIngredientsText(string recipe_name){
        List<string> list = meme.GetRecipeIngredients(recipe_name);
        string ret = "";
        foreach(var ent in list){
            ret += ent + "\n";
        }
        return ret;
    }


    public string GetText(string name){
        string ret = meme.GetRecipeInstructions(name);
        return ret;
    }

    public void OnGet()
    {
        meme.connect("test.db");
        IsPost = true;
        this.Recipes= GetOptions();
    }

    public void OnPost()
    {
        meme.connect("test.db");
        Text = GetText(Ans);
        IngredientsText = GetIngredientsText(Ans);
        this.Recipes = GetOptions();
    }
}
