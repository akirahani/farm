using System.ComponentModel.DataAnnotations.Schema;

namespace Farm.Models;

public class Cage{
    public int id {get; set;}
    public string name {get; set;}

    public int users_id {get; set;}
    public int category_id {get; set;}

    public int quantity {get; set;}
    public int status {get; set;}
    public string note {get; set;}
    public DateTime created_at {get; set;}
    public DateTime close_at {get; set;}

}