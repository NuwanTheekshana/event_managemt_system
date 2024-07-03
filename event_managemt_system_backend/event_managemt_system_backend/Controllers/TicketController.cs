using event_managemt_system_backend.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace event_managemt_system_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : Controller
    {
        
        private readonly IConfiguration _configuration;

        public TicketController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        [Route("GetEventList")]
        public IActionResult GetEventList()
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");
            List<GetEventList> events = new List<GetEventList>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = "SELECT e.Event_id, e.Event_Name, e.Event_Description, e.Event_DateTime, e.Ticket_Price, e.Ticket_Count, COUNT(t.Ticket_Id) AS sell_ticket_count, (e.Ticket_Count - COUNT(t.Ticket_Id)) AS availble_ticket_count FROM event_tbl e LEFT JOIN ticket_tbl t ON e.Event_id = t.Event_Id WHERE e.status = 1 GROUP BY e.Event_id, e.Event_Name, e.Event_Description, e.Event_DateTime, e.Ticket_Price, e.Ticket_Count";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                var eventItem = new GetEventList
                                {
                                    Event_id = Convert.ToInt32(reader["Event_id"]),
                                    Event_Name = reader["Event_Name"].ToString(),
                                    Event_Description = reader["Event_Description"].ToString(),
                                    Event_DateTime = Convert.ToDateTime(reader["Event_DateTime"]),
                                    Ticket_Price = Convert.ToDecimal(reader["Ticket_Price"]),
                                    Ticket_Count = Convert.ToInt32(reader["Ticket_Count"]),
                                    sell_ticket_count = Convert.ToInt32(reader["sell_ticket_count"]),
                                    availble_ticket_count = Convert.ToInt32(reader["availble_ticket_count"])
                                };
                                events.Add(eventItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return Ok(events);
        }

        [HttpPost]
        [Route("AddTicket")]
        public IActionResult TicketRegistration(Ticket ticket)
        {
            Response response = new Response();

            try
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string insertEventQuery = "INSERT INTO ticket_tbl (User_Id, Event_Id, Customer_FName, Customer_LName, Customer_NIC, Customer_Email, Customer_Tel_No) " +
                                                                 "VALUES (@User_Id, @Event_Id, @Customer_FName, @Customer_LName, @Customer_NIC, @Customer_Email, @Customer_Tel_No)";

                    using (MySqlCommand cmd = new MySqlCommand(insertEventQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@User_Id", ticket.User_Id);
                        cmd.Parameters.AddWithValue("@Event_Id", ticket.Event_Id);
                        cmd.Parameters.AddWithValue("@Customer_FName", ticket.Customer_FName);
                        cmd.Parameters.AddWithValue("@Customer_LName", ticket.Customer_LName);
                        cmd.Parameters.AddWithValue("@Customer_NIC", ticket.Customer_NIC);
                        cmd.Parameters.AddWithValue("@Customer_Email", ticket.Customer_Email);
                        cmd.Parameters.AddWithValue("@Customer_Tel_No", ticket.Customer_Tel_No);

                        int jobSeekerRowsAffected = cmd.ExecuteNonQuery();

                        if (jobSeekerRowsAffected > 0)
                        {
                            response.StatusCode = 200;
                            response.StatusMessage = "Ticket added successful";
                        }
                        else
                        {
                            response.StatusCode = 100;
                            response.StatusMessage = "Ticket added failed";
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
        [Route("GetTicket/{user_id}")]
        public IActionResult GetTicketList(int user_id)
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");
            List<GetTicketList> tickets = new List<GetTicketList>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    // Check user permission
                    string permissionCheckQuery = "SELECT Permission FROM users WHERE User_Id = @User_Id";
                    int permissionId;

                    using (MySqlCommand permissionCmd = new MySqlCommand(permissionCheckQuery, con))
                    {
                        permissionCmd.Parameters.AddWithValue("@User_Id", user_id);
                        object permissionObj = permissionCmd.ExecuteScalar();

                        if (permissionObj != null && int.TryParse(permissionObj.ToString(), out permissionId))
                        {
                            if (permissionId == 2)
                            {
                                
                                string query = "SELECT t.Ticket_Id, e.Event_Name, CONCAT(t.Customer_FName, ' ', t.Customer_LName) AS Customer_Name, " +
                                               "t.Customer_Email AS Customer_Email, t.Customer_Tel_No AS Contact_No, t.Status " +
                                               "FROM event_tbl e LEFT JOIN ticket_tbl t ON e.Event_id = t.Event_Id " +
                                               "WHERE e.status = 1;";

                                using (MySqlCommand cmd = new MySqlCommand(query, con))
                                {
                                    using (MySqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            int Status = Convert.ToInt32(reader["Status"]);
                                            string Status_Type = (Status == 1) ? "Active" : "Deactive";

                                            var eventItem = new GetTicketList
                                            {
                                                Ticket_Id = Convert.ToInt32(reader["Ticket_Id"]),
                                                Event_Name = reader["Event_Name"].ToString(),
                                                Customer_Name = reader["Customer_Name"].ToString(),
                                                Customer_Email = reader["Customer_Email"].ToString(),
                                                Contact_No = Convert.ToInt32(reader["Contact_No"]),
                                                Status = Status_Type
                                            };
                                            tickets.Add(eventItem);
                                        }
                                    }
                                }
                            }
                            else
                            {
                              
                                string query = "SELECT t.Ticket_Id, e.Event_Name, CONCAT(t.Customer_FName, ' ', t.Customer_LName) AS Customer_Name, " +
                                               "t.Customer_Email AS Customer_Email, t.Customer_Tel_No AS Contact_No, t.Status " +
                                               "FROM event_tbl e LEFT JOIN ticket_tbl t ON e.Event_id = t.Event_Id " +
                                               "WHERE e.status = 1 AND t.User_Id = @User_Id;";

                                using (MySqlCommand cmd = new MySqlCommand(query, con))
                                {
                                    cmd.Parameters.AddWithValue("@User_Id", user_id);

                                    using (MySqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            int Status = Convert.ToInt32(reader["Status"]);
                                            string Status_Type = (Status == 1) ? "Active" : "Deactive";

                                            var eventItem = new GetTicketList
                                            {
                                                Ticket_Id = Convert.ToInt32(reader["Ticket_Id"]),
                                                Event_Name = reader["Event_Name"].ToString(),
                                                Customer_Name = reader["Customer_Name"].ToString(),
                                                Customer_Email = reader["Customer_Email"].ToString(),
                                                Contact_No = Convert.ToInt32(reader["Contact_No"]),
                                                Status = Status_Type
                                            };
                                            tickets.Add(eventItem);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            return NotFound("User not found or invalid permission.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return Ok(tickets);
        }


    }
}
