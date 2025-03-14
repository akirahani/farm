using System.ComponentModel.DataAnnotations.Schema;

namespace Farm.Models;

public class Deal{
    public int id {get; set;}
    public string customer {get; set;}
    public string animal_id {get; set;}
    public double total_price {get; set;}
    public DateTime date_out {get; set;}
    public int status {get; set;}
    public DateTime created_at {get; set;}

}