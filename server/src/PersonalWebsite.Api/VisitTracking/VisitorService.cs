using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace PersonalWebsite.Api.VisitTracking;

public class VisitorService
{
    private readonly ConcurrentDictionary<string, DateTime> _recentVisitors;
    private readonly TimeSpan _visitWindow;

    public VisitorService(TimeSpan visitWindow)
    {
        _recentVisitors = new ConcurrentDictionary<string, DateTime>();
        _visitWindow = visitWindow;
    }

    public bool IsRecentVisitor(string? ipAddress)
    {
        if (ipAddress is null)
        {
            return false;
        }

        if (_recentVisitors.TryGetValue(ipAddress, out var lastVisitTime))
        {
            if (DateTime.UtcNow - lastVisitTime <= _visitWindow)
            {
                return true;
            }
        }
        Console.WriteLine($"Visit at {DateTime.UtcNow} from {ipAddress}");
        _recentVisitors[ipAddress] = DateTime.UtcNow;
        RemoveOutdatedVisitors();

        return false;
    }

    public bool IsWebCrawler(string? ipAddress)
    {
        if (ipAddress is null)
        {
            return false;
        }

        var pattern = @"^66\.249\.79\.\d{1,3}$";
        var isCrawler = Regex.IsMatch(ipAddress, pattern);

        if (isCrawler)
        {
            Console.WriteLine($"Crawler visit at {DateTime.UtcNow} from {ipAddress}");
        }

        return isCrawler;
    }

    private void RemoveOutdatedVisitors()
    {
        var threshold = DateTime.UtcNow - _visitWindow;
        var visitorsToRemove = _recentVisitors
            .Where(v => v.Value < threshold)
            .Select(v => v.Key)
            .ToList();

        foreach (var key in visitorsToRemove)
        {
            _recentVisitors.TryRemove(key, out _);
        }
    }
}
