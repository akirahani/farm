using System.ComponentModel.DataAnnotations.Schema;

namespace Farm.Models;

public class Animal{
    public int id {get; set;}
    public string name {get; set;}
    public int cage_id {get; set;}
    public int category_id {get; set;}
    public double height {get; set;}
    public int status {get; set;}
    public DateTime created_at {get; set;}
    public DateTime date_in {get; set;}
    public DateTime date_out {get; set;}

}