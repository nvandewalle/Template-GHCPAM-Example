namespace EventManagement.Models;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
}

public class EventRegistration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Pronouns { get; set; } = string.Empty;
    public bool OptInForCommunication { get; set; }
    public DateTime RegistrationDate { get; set; }
}