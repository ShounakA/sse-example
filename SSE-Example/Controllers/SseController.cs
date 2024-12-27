using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class EventData
{
    public EventData(int id, string message, DateTime time, float delay)
    {
        Id = id;
        Message=message;
        Timestamp=time;
        Delay = delay;
    }

    public int Id {get;set;}
    public string Message {get;set;} = string.Empty;
    public DateTime Timestamp {get;set;}
    public float Delay {get;set;}
}

[Route("api/sse")]
[ApiController]
public class SseController : ControllerBase
{
    [HttpGet]
    public async Task StreamEvents()
    {
        // Set response headers for SSE
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");

        await foreach (var data in GetEventsAsync())
        {
            var json = JsonSerializer.Serialize(data);

            await Response.WriteAsync($"data: {json}\n\n");
            await Response.Body.FlushAsync();
        }
        await Response.WriteAsync("data: \"Complete!\"\n\n");
        await Response.Body.FlushAsync();
        await Response.CompleteAsync();
    }

    private async IAsyncEnumerable<EventData> GetEventsAsync()
    {
        var rnd = new Random();
        var tasks = new List<Task<EventData>>();

        for (int i = 0; i < 100; i++)
        {
            tasks.Add(CreateEventAsync(i, rnd));
        }

        while (tasks.Count > 0)
        {
            var completedTask = await Task.WhenAny(tasks);
            tasks.Remove(completedTask);

            yield return await completedTask;
        }
    }

    private async Task<EventData> CreateEventAsync(int counter, Random rnd)
    {
        var randomDelay = 0;
        if (rnd.Next(1, 3) == 1)
        {
            randomDelay = rnd.Next(0, 10000);
            await Task.Delay(randomDelay);
        }
        return new EventData(counter, $"Event #{counter}", DateTime.UtcNow, randomDelay);
    }

}
