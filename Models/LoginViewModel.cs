using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farm.Models;

public class LoginViewModel 
{
    public string username {get; set;}
    public string passd {get; set;}
    public string name {get; set;}
    public List<User> userList {get; set;}
}