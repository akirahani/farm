using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farm.Models;

public class LoginViewModel 
{
    public string username {get; set;}
    public string passd {get; set;}
    public string name {get; set;}
    public List<User> userList {get; set;}
    public List<News> newsList {get; set;}
    public List<Category> categoryList {get; set;}
    public List<Cage> cageList {get; set;}
    public List<Animal> animalList {get; set;}

    public List<Therapy> therapyList {get; set;}
    public List<Deal> dealList {get; set;}

    public List<Contact> contactList {get; set;}



}