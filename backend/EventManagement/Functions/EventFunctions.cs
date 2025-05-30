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

    public EventFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<EventFunctions>();
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
            await notFoundResponse.WriteStringAsync($"Event with ID {id} not found");
            return notFoundResponse;
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
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
            await badRequestResponse.WriteStringAsync("Invalid event data");
            return badRequestResponse;
        }

        eventItem.Id = _nextId++;
        _events.Add(eventItem);

        var response = req.CreateResponse(HttpStatusCode.Created);
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
            await notFoundResponse.WriteStringAsync($"Event with ID {id} not found");
            return notFoundResponse;
        }

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var updatedEvent = JsonSerializer.Deserialize<Event>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (updatedEvent == null)
        {
            var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequestResponse.WriteStringAsync("Invalid event data");
            return badRequestResponse;
        }

        updatedEvent.Id = id;
        var index = _events.IndexOf(existingEvent);
        _events[index] = updatedEvent;

        var response = req.CreateResponse(HttpStatusCode.OK);
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
            await notFoundResponse.WriteStringAsync($"Event with ID {id} not found");
            return notFoundResponse;
        }

        _events.Remove(eventItem);

        var response = req.CreateResponse(HttpStatusCode.NoContent);
        return response;
    }
}