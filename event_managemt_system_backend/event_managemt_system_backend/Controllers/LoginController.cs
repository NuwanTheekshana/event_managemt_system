using event_managemt_system_backend.Common;
using event_managemt_system_backend.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace event_managemt_system_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {

        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public LoginResult Login(Login login)
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string encryptedPassword = CommonMethods.ConvertToEncrypt(login.Password);

                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM users WHERE Email = '" + login.Email + "' AND Password = '" + encryptedPassword + "' AND Status = 1", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    int userId = Convert.ToInt32(row["User_Id"]);
                    string UserName = row["FName"].ToString()+" "+ row["LName"].ToString();
                    string userEmail = row["Email"].ToString();
                    int userPermission = Convert.ToInt32(row["Permission"]);
                    int userStatus = Convert.ToInt32(row["Status"]);

                    return new LoginResult
                    {
                        Id = userId,
                        UserName = UserName,
                        Email = userEmail,
                        Permission = userPermission,
                        Status = userStatus,
                        Token = CommonMethods.ConvertToEncrypt(userEmail),
                        Message = "Data Found"
                    };
                }
                else
                {
                    return new LoginResult
                    {
                        Message = "Invalid User"
                    };
                }
            }
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
