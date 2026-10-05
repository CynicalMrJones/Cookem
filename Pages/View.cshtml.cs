
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookem.Pages;

public class ViewModel : PageModel
{

    public Sql meme = new Sql();

    [BindProperty]
    public string? Options { get; set; }

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

    public void OnGet()
    {
        IsPost = true;
        this.Recipes= GetOptions();
    }
}
