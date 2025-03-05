using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;
using mvcbasic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Farm.Controllers.Admin;

public class UserController : Controller
{

    private readonly MvcBasicDbContext _context;

    public UserController(MvcBasicDbContext context){
        _context = context;
    }

    [Route("/admin/user", Name="admin.user")]
    public  IActionResult List()
    {
        string connectionString = "Server=ADMIN\\SQLEXPRESS;Database=farm;Trusted_Connection=True;TrustServerCertificate=True;";
        SqlConnection conn = new(connectionString);
        String sql = "SELECT * FROM users";
        SqlCommand cmd = new(sql, conn);
        var model = new List<User>();
        conn.Open();

        using(conn)
        {
            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var user = new User();
                        user.name = (string)reader["name"];
                        user.username = (string)reader["username"];
                        user.role = (int)reader["role"];
                        user.id = (int)reader["id"];

                        model.Add(user);
                    }
                }
            }
        }
        conn.Close();

        return View("~/Views/Admin/User/List.cshtml",model);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
