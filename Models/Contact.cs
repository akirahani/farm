using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Farm.Models;

public class Contact{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id {get; set;}
    public string name {get; set;}
    public string email {get; set;}
    public string phone {get; set;}
    public string address {get; set;}
    public string note {get; set;}
    public DateTime created_at {get; set;}

}