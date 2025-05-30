using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using EventManagement.Models;

namespace EventManagement.Functions;

public class EventFunctions
{
    private readonly ILogger _logger;
    private static readonly List<Event> _events = new();
    private static int _nextId = 1;

    static EventFunctions()
    {
        // Add some sample data for testing
        _events.AddRange(new[]
        {
            new Event
            {
                Id = _nextId++,
                Name = "Tech Conference 2024",
                Location = "San Francisco, CA",
                Date = DateTime.Parse("2024-06-15"),
                StartTime = TimeSpan.Parse("09:00:00")
            },
            new Event
            {
                Id = _nextId++,
                Name = "Web Development Workshop",
                Location = "New York, NY",
                Date = DateTime.Parse("2024-07-20"),
                StartTime = TimeSpan.Parse("14:00:00")
            },
            new Event
            {
                Id = _nextId++,
                Name = "AI and Machine Learning Summit",
                Location = "Seattle, WA",
                Date = DateTime.Parse("2024-08-10"),
                StartTime = TimeSpan.Parse("10:30:00")
            }
        });
    }

    public EventFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<EventFunctions>();
    }

    private static void AddCorsHeaders(HttpResponseData response)
    {
        response.Headers.Add("Access-Control-Allow-Origin", "*");
        response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
    }

    [Function("GetEvents")]
    public async Task<HttpResponseData> GetEvents([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events")] HttpRequestData req)
    {
        _logger.LogInformation("Getting all events");

        var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        var dateFilter = query["date"];
        var locationFilter = query["location"];

        var filteredEvents = _events.AsEnumerable();

        if (!string.IsNullOrEmpty(dateFilter) && DateTime.TryParse(dateFilter, out var filterDate))
        {
            filteredEvents = filteredEvents.Where(e => e.Date.Date == filterDate.Date);
        }

        if (!string.IsNullOrEmpty(locationFilter))
        {
            filteredEvents = filteredEvents.Where(e => e.Location.Contains(locationFilter, StringComparison.OrdinalIgnoreCase));
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        AddCorsHeaders(response);
        await response.WriteAsJsonAsync(filteredEvents.ToList());
        return response;
    }

    [Function("GetEvent")]
    public async Task<HttpResponseData> GetEvent([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events/{id:int}")] HttpRequestData req,
        int id)
    {
        _logger.LogInformation($"Getting event with ID: {id}");

        var eventItem = _events.FirstOrDefault(e => e.Id == id);
        if (eventItem == null)
        {
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            AddCorsHeaders(notFoundResponse);
            await notFoundResponse.WriteStringAsync($"Event with ID {id} not found");
            return notFoundResponse;
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        AddCorsHeaders(response);
        await response.WriteAsJsonAsync(eventItem);
        return response;
    }

    [Function("CreateEvent")]
    public async Task<HttpResponseData> CreateEvent([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "events")] HttpRequestData req)
    {
        _logger.LogInformation("Creating new event");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var eventItem = JsonSerializer.Deserialize<Event>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (eventItem == null)
        {
            var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            AddCorsHeaders(badRequestResponse);
            await badRequestResponse.WriteStringAsync("Invalid event data");
            return badRequestResponse;
        }

        eventItem.Id = _nextId++;
        _events.Add(eventItem);

        var response = req.CreateResponse(HttpStatusCode.Created);
        AddCorsHeaders(response);
        await response.WriteAsJsonAsync(eventItem);
        return response;
    }

    [Function("UpdateEvent")]
    public async Task<HttpResponseData> UpdateEvent([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "events/{id:int}")] HttpRequestData req,
        int id)
    {
        _logger.LogInformation($"Updating event with ID: {id}");

        var existingEvent = _events.FirstOrDefault(e => e.Id == id);
        if (existingEvent == null)
        {
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            AddCorsHeaders(notFoundResponse);
            await notFoundResponse.WriteStringAsync($"Event with ID {id} not found");
            return notFoundResponse;
        }

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var updatedEvent = JsonSerializer.Deserialize<Event>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (updatedEvent == null)
        {
            var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            AddCorsHeaders(badRequestResponse);
            await badRequestResponse.WriteStringAsync("Invalid event data");
            return badRequestResponse;
        }

        updatedEvent.Id = id;
        var index = _events.IndexOf(existingEvent);
        _events[index] = updatedEvent;

        var response = req.CreateResponse(HttpStatusCode.OK);
        AddCorsHeaders(response);
        await response.WriteAsJsonAsync(updatedEvent);
        return response;
    }

    [Function("DeleteEvent")]
    public async Task<HttpResponseData> DeleteEvent([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "events/{id:int}")] HttpRequestData req,
        int id)
    {
        _logger.LogInformation($"Deleting event with ID: {id}");

        var eventItem = _events.FirstOrDefault(e => e.Id == id);
        if (eventItem == null)
        {
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            AddCorsHeaders(notFoundResponse);
            await notFoundResponse.WriteStringAsync($"Event with ID {id} not found");
            return notFoundResponse;
        }

        _events.Remove(eventItem);

        var response = req.CreateResponse(HttpStatusCode.NoContent);
        AddCorsHeaders(response);
        return response;
    }

    [Function("OptionsEvent")]
    public HttpResponseData OptionsEvent([HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "events")] HttpRequestData req)
    {
        _logger.LogInformation("Handling preflight request for events");

        var response = req.CreateResponse(HttpStatusCode.OK);
        AddCorsHeaders(response);
        return response;
    }

    [Function("OptionsEventId")]
    public HttpResponseData OptionsEventId([HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "events/{id:int}")] HttpRequestData req)
    {
        _logger.LogInformation("Handling preflight request for specific event");

        var response = req.CreateResponse(HttpStatusCode.OK);
        AddCorsHeaders(response);
        return response;
    }
}