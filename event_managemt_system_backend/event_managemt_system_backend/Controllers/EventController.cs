using event_managemt_system_backend.Common;
using event_managemt_system_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace event_managemt_system_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly IConfiguration _configuration;

        public EventController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("AddEvent")]
        public IActionResult EventRegistration(Event events)
        {
            Response response = new Response();

            try
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string insertEventQuery = "INSERT INTO event_tbl (User_Id, Event_Name, Event_Description, Event_Location, Event_DateTime, Ticket_Price, Ticket_Count, Commission_Rate) " +
                                                                 "VALUES (@User_Id, @Event_Name, @Event_Description, @Event_Location, @Event_DateTime, @Ticket_Price, @Ticket_Count, @Commission_Rate)";

                    using (MySqlCommand cmd = new MySqlCommand(insertEventQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@User_Id", events.User_Id);
                        cmd.Parameters.AddWithValue("@Event_Name", events.Event_Name);
                        cmd.Parameters.AddWithValue("@Event_Description", events.Event_Description);
                        cmd.Parameters.AddWithValue("@Event_Location", events.Event_Location);
                        cmd.Parameters.AddWithValue("@Event_DateTime", events.Event_DateTime);
                        cmd.Parameters.AddWithValue("@Ticket_Price", events.Ticket_Price);
                        cmd.Parameters.AddWithValue("@Ticket_Count", events.Ticket_Count);
                        cmd.Parameters.AddWithValue("@Commission_Rate", events.Commission_Rate);

                        int jobSeekerRowsAffected = cmd.ExecuteNonQuery();

                        if (jobSeekerRowsAffected > 0)
                        {
                            response.StatusCode = 200;
                            response.StatusMessage = "Event added successful";
                        }
                        else
                        {
                            response.StatusCode = 100;
                            response.StatusMessage = "Event added failed";
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
        [Route("Event")]
        public IActionResult GetEvent()
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");
            List<GetEvent> events = new List<GetEvent>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = "SELECT * FROM event_tbl WHERE status = 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int status = Convert.ToInt32(reader["Status"]);
                                string statusType = status == 1 ? "Active" : "Deactive";

                                var eventItem = new GetEvent
                                {
                                    Event_id = Convert.ToInt32(reader["Event_id"]),
                                    User_Id = Convert.ToInt32(reader["User_Id"]),
                                    Event_Name = reader["Event_Name"].ToString(),
                                    Event_Description = reader["Event_Description"].ToString(),
                                    Event_Location = reader["Event_Location"].ToString(),
                                    Event_DateTime = Convert.ToDateTime(reader["Event_DateTime"]),
                                    Ticket_Price = Convert.ToDecimal(reader["Ticket_Price"]),
                                    Ticket_Count = Convert.ToInt32(reader["Ticket_Count"]),
                                    Commission_Rate = Convert.ToInt32(reader["Commission_Rate"]),
                                    Status = status,
                                    Status_Type = statusType
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


        [HttpDelete]
        [Route("Event/{id}")]
        public IActionResult DeleteEvent(int id)
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string query = "UPDATE event_tbl SET Status = 0, updated_at = CURRENT_TIMESTAMP() WHERE User_Id = @EventId";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EventId", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("Event deleted successfully!");
                    }
                    else
                    {
                        return NotFound("Event delete failed.");
                    }
                }
            }
        }

        [HttpPut]
        [Route("Event/{id}")]
        public IActionResult UpdateEvent(int id, UpdateEvent events)
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string query = "UPDATE event_tbl SET Event_Name = @Event_Name, Event_Description = @Event_Description, Event_Location = @Event_Location, Event_DateTime = @Event_DateTime, Ticket_Price = @Ticket_Price, Ticket_Count = @Ticket_Count, Commission_Rate = @Commission_Rate, updated_at = CURRENT_TIMESTAMP() WHERE Event_id   = @Event_id ";


                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Event_id", events.Event_id);
                    cmd.Parameters.AddWithValue("@Event_Name", events.Event_Name);
                    cmd.Parameters.AddWithValue("@Event_Description", events.Event_Description);
                    cmd.Parameters.AddWithValue("@Event_Location", events.Event_Location);
                    cmd.Parameters.AddWithValue("@Event_DateTime", events.Event_DateTime);
                    cmd.Parameters.AddWithValue("@Ticket_Price", events.Ticket_Price);
                    cmd.Parameters.AddWithValue("@Ticket_Count", events.Ticket_Count);
                    cmd.Parameters.AddWithValue("@Commission_Rate", events.Commission_Rate);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("Event details updated successfully!");
                    }
                    else
                    {
                        return NotFound("Event details update failed.");
                    }
                }
            }
        }

        [HttpGet]
        [Route("GetSales")]
        public IActionResult GetSales()
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");
            List<GetSales> sales = new List<GetSales>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = "SELECT e.Event_id, e.Event_Name, e.Ticket_Price, e.Ticket_Count, e.Commission_Rate, COUNT(t.Ticket_Id) AS Sell_ticket_count, (e.Ticket_Count - COUNT(t.Ticket_Id)) AS Availble_ticket_count, (e.Ticket_Price * COUNT(t.Ticket_Id)) As Total_Sales_Amount, (e.Ticket_Price * COUNT(t.Ticket_Id) * e.Commission_Rate / 100) As Commission_Amount FROM event_tbl e LEFT JOIN ticket_tbl t ON e.Event_id = t.Event_Id WHERE e.status = 1 GROUP BY e.Event_id, e.Event_Name, e.Ticket_Price, e.Ticket_Count";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                var salesitems = new GetSales
                                {
                                    Event_id = Convert.ToInt32(reader["Event_id"]),
                                    Event_Name = reader["Event_Name"].ToString(),
                                    Ticket_Price = Convert.ToDecimal(reader["Ticket_Price"]),
                                    Ticket_Count = Convert.ToInt32(reader["Ticket_Count"]),
                                    Commission_Rate = Convert.ToInt32(reader["Commission_Rate"]),
                                    Sell_ticket_count = Convert.ToInt32(reader["Sell_ticket_count"]),
                                    Availble_ticket_count = Convert.ToInt32(reader["Availble_ticket_count"]),
                                    Total_Sales_Amount = Convert.ToDecimal(reader["Total_Sales_Amount"]),
                                    Commission_Amount = Convert.ToDecimal(reader["Commission_Amount"])
                                };
                                sales.Add(salesitems);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return Ok(sales);
        }


    }
}
