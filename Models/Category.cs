using System.ComponentModel.DataAnnotations.Schema;

namespace Farm.Models;

public class Category{
    public int id {get; set;}
    public string name {get; set;}
    public int parent_id {get; set;}
    public DateTime created_at {get; set;}
}