using System.Collections.Concurrent;

namespace Booking.Business.Application.Services;

public class BookingLog
{
    private readonly ConcurrentQueue<string> _logQueue = new();

    public void AddLog(string logEntry)
    {
        _logQueue.Enqueue($"{DateTime.Now}: {logEntry}");
    }

    public string[] GetLogs()
    {
        return _logQueue.ToArray();
    }
}