namespace event_managemt_system_backend.Models
{
    public class GetEventList
    {
        public int Event_id { get; set; }
        public string Event_Name { get; set; }
        public string Event_Description { get; set; }
        public DateTime Event_DateTime { get; set; }
        public decimal Ticket_Price { get; set; }
        public int Ticket_Count { get; set; }
        public int sell_ticket_count { get; set; }
        public int availble_ticket_count { get; set; }
    }

    public class Ticket
    {
        public int User_Id { get; set; }
        public int Event_Id { get; set; }
        public long Customer_Tel_No { get; set; }
        public string Customer_FName { get; set; }
        public string Customer_LName { get; set; }
        public string Customer_NIC { get; set; }
        public string Customer_Email { get; set; }
    }

    public class GetTicketList
    {
        public int Ticket_Id { get; set; }
        public string Event_Name { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Email { get; set; }
        public long Contact_No { get; set; }
        public string Status { get; set; }
    }

}
