using System.ComponentModel.DataAnnotations.Schema;

namespace Farm.Models;

public class Therapy{
    public int id {get; set;}
    public string name {get; set;}
    public int animal_id {get; set;}
    public string doctor {get; set;}
    public string medicine {get; set;}
    public int quantity {get; set;}
    public string note {get; set;}
    public DateTime created_at {get; set;}

}