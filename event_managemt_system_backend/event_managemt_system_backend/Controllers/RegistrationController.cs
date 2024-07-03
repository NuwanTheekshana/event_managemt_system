using event_managemt_system_backend.Common;
using event_managemt_system_backend.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace event_managemt_system_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : Controller
    {
        private readonly IConfiguration _configuration;

        public RegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("registration")]
        public IActionResult Registration(Registration registration)
        {
            Response response = new Response();

            try
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string encryptedPassword = CommonMethods.ConvertToEncrypt(registration.Password);

                    string insertUserQuery = "INSERT INTO users (FName, LName, Email, Tel_No, Password, Permission) " +
                                                                 "VALUES (@FName, @LName, @Email, @Tel_No, @Password, @Permission)";

                    using (MySqlCommand cmd = new MySqlCommand(insertUserQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@FName", registration.FName);
                        cmd.Parameters.AddWithValue("@LName", registration.LName);
                        cmd.Parameters.AddWithValue("@Email", registration.Email);
                        cmd.Parameters.AddWithValue("@Tel_No", registration.Tel_No);
                        cmd.Parameters.AddWithValue("@Password", encryptedPassword);
                        cmd.Parameters.AddWithValue("@Permission", registration.Permission);

                        int jobSeekerRowsAffected = cmd.ExecuteNonQuery();

                        if (jobSeekerRowsAffected > 0)
                        {
                            response.StatusCode = 200;
                            response.StatusMessage = "Registration successful";
                        }
                        else
                        {
                            response.StatusCode = 100;
                            response.StatusMessage = "Registration failed";
                        }
                    }
                }


            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return Ok(response);

        }

        [HttpGet]
        [Route("Users")]
        public IActionResult GetUsers()
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");
            List<GetUsers> users = new List<GetUsers>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT * FROM users";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int Permission = Convert.ToInt32(reader["Permission"]);
                            string Permission_Type = "";

                            if (Permission == 1)
                            {
                                Permission_Type = "Partner";
                            }
                            else if (Permission == 2)
                            {
                                Permission_Type = "Admin";
                            }
                            

                            int Status = Convert.ToInt32(reader["Status"]);
                            string Status_Type = "";

                            if (Status == 1)
                            {
                                Status_Type = "Active";
                            }
                            else
                            {
                                Status_Type = "Deactive";
                            }


                            var user = new GetUsers
                            {
                                User_Id = Convert.ToInt32(reader["User_Id"]),
                                UserName = reader["FName"].ToString()+" "+ reader["LName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Permission = Convert.ToInt32(reader["Permission"]),
                                Permission_Type = Permission_Type,
                                Status_Type = Status_Type,
                            };
                            users.Add(user);
                        }
                    }
                }
            }

            return Ok(users);
        }

        [HttpDelete]
        [Route("Users/{id}")]
        public IActionResult DeleteUsers(int id)
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string query = "UPDATE users SET Status = 0, updated_at = CURRENT_TIMESTAMP() WHERE User_Id = @User_Id";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@User_Id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("User deleted successfully!");
                    }
                    else
                    {
                        return NotFound("User delete failed.");
                    }
                }
            }
        }


        [HttpPut]
        [Route("Users/{id}")]
        public IActionResult UpdateUsers(int id, UpdateUser user)
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string query = "UPDATE users SET Email = @Email, Permission = @Permission, updated_at = CURRENT_TIMESTAMP() WHERE User_Id  = @User_Id";


                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@User_Id", id);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Permission", user.Permission);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("User details updated successfully!");
                    }
                    else
                    {
                        return NotFound("User details update failed.");
                    }
                }
            }
        }
    }
            
}
