
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SQLite;
using System;

namespace Cookem.Pages;

public class InputModel : PageModel
{

    public Sql meme = new Sql();

    [BindProperty]
    public string? Name{ get; set; }

    [BindProperty]
    public string? Instructions{ get; set; }

    [BindProperty]
    public string? Ans{ get; set; }

    public bool IsPost { get; set; }

    public string Add(string name, string instructions){
        meme.connect("test.db");
        if (string.IsNullOrEmpty(name)){
            Ans = "Name is empty";
            return "Name is empty";
        }
        if (string.IsNullOrEmpty(instructions)){
            Ans = "Instructions is empty";
            return "Instructions is empty";
        }
        Ans = meme.Insert_Rec(name, instructions);
        return Ans;
    }

    public void OnGet()
    {
        IsPost = true;
    }
}
