

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookem.Pages;

public class ViewModel : PageModel
{

    public Sql meme = new Sql();

    [BindProperty]
    public string? Options { get; set; }

    public bool IsPost { get; set; }

    private string GetOptions(){
        meme.connect("test.db");
        List<string> strs = meme.GetRecipeNames();
        string ret = "";
        foreach(var ent in strs){
            string option = String.Format(@"<option>{0}</option>", ent);
            ret += option;
        }
        return ret;
    }

    public void OnGet()
    {
        IsPost = true;
        this.Options = GetOptions();
    }
}
