using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SQLite;

namespace Cookem.Pages;

public class TestModel : PageModel
{

    public Sql meme = new Sql();

    [BindProperty]
    public int Num1 { get; set; }

    [BindProperty]
    public int Num2 { get; set; }

    [BindProperty]
    public int Ans{ get; set; }

    public bool IsPost { get; set; }

    public int Simple_Add(int num1, int num2){
        Ans = num1 + num2;
        return num1 + num2;
    }

    public void OnGet()
    {
        //TODO: Figure out why no work
        Console.WriteLine(meme.IsConnected());
        IsPost = true;
        if (meme.IsConnected() == false) {
            meme.connect("test.db");
        }
        meme.PrintTables();
    }
}
