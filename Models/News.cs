using System.ComponentModel.DataAnnotations.Schema;

namespace Farm.Models;

public class News{
    public int id {get; set;}
    public string image {get; set;}
    [NotMapped]
    public IFormFile file {get; set;}

    public string title {get; set;}
    public string subtit {get; set;}
    public string content {get; set;}
    public DateTime created_at {get; set;}
}