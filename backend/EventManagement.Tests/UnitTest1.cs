using EventManagement.Models;

namespace EventManagement.Tests;

public class EventModelTests
{
    [Fact]
    public void Event_Should_Have_Correct_Properties()
    {
        // Arrange
        var eventItem = new Event
        {
            Id = 1,
            Name = "Test Event",
            Location = "Test Location",
            Date = new DateTime(2024, 6, 15),
            StartTime = new TimeSpan(9, 0, 0)
        };

        // Assert
        Assert.Equal(1, eventItem.Id);
        Assert.Equal("Test Event", eventItem.Name);
        Assert.Equal("Test Location", eventItem.Location);
        Assert.Equal(new DateTime(2024, 6, 15), eventItem.Date);
        Assert.Equal(new TimeSpan(9, 0, 0), eventItem.StartTime);
    }

    [Fact]
    public void EventRegistration_Should_Have_Correct_Properties()
    {
        // Arrange
        var registration = new EventRegistration
        {
            Id = 1,
            EventId = 1,
            Name = "John Doe",
            Email = "john@example.com",
            Pronouns = "he/him",
            OptInForCommunication = true,
            RegistrationDate = DateTime.UtcNow
        };

        // Assert
        Assert.Equal(1, registration.Id);
        Assert.Equal(1, registration.EventId);
        Assert.Equal("John Doe", registration.Name);
        Assert.Equal("john@example.com", registration.Email);
        Assert.Equal("he/him", registration.Pronouns);
        Assert.True(registration.OptInForCommunication);
    }
}