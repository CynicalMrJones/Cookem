
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

    public bool IsPost { get; set; }

    public List<SelectListItem>? Recipes { set; get;}

    private List<SelectListItem> GetOptions(){
        meme.connect("test.db");
        List<string> strs = meme.GetRecipeNames();

        return strs.Select(x => new SelectListItem {
                Text = x,
                Value = x
        }).ToList();
    }

    public string GetText(string name){
        meme.connect("test.db");
        string ret = meme.GetRecipeInstructionsIngredients(name);
        return ret;
    }

    public void OnGet()
    {
        IsPost = true;
        this.Recipes= GetOptions();
    }

    public void OnPost()
    {
        Text = GetText(Ans);
    }
}
