namespace event_managemt_system_backend.Models
{
    public class Event
    {
        public int User_Id { get; set; }
        public string Event_Name { get; set; }
        public string Event_Description { get; set; }
        public string Event_Location { get; set; }
        public DateTime Event_DateTime { get; set; }
        public decimal Ticket_Price { get; set; }
        public int Ticket_Count { get; set; }
        public int Commission_Rate { get; set; }
    }

    public class GetEvent { 
        public int Event_id { get; set; }
        public int User_Id { get; set; }
        public string Event_Name { get; set; }
        public string Event_Description { get; set; }
        public string Event_Location { get; set; }
        public DateTime Event_DateTime { get; set; }
        public decimal Ticket_Price { get; set; }
        public int Ticket_Count { get; set; }
        public int Commission_Rate { get; set; }
        public int Status { get; set; }
        public string Status_Type { get; set; }

    }

    public class UpdateEvent
    {
        public int Event_id { get; set; }
        public string Event_Name { get; set; }
        public string Event_Description { get; set; }
        public string Event_Location { get; set; }
        public DateTime Event_DateTime { get; set; }
        public decimal Ticket_Price { get; set; }
        public int Ticket_Count { get; set; }
        public int Commission_Rate { get; set; }

    }

    public class GetSales
    {
        public int Event_id { get; set; }
        public string Event_Name { get; set; }
        public decimal Ticket_Price { get; set; }
        public int Ticket_Count { get; set; }
        public int Commission_Rate { get; set; }
        public int Sell_ticket_count { get; set; }
        public int Availble_ticket_count { get; set; }
        public decimal Total_Sales_Amount { get; set; }
        public decimal Commission_Amount { get; set; }

    }



}
