﻿using System.Collections.Concurrent;

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
        if (ipAddress == null)
        {
            return false;
        }

        if (_recentVisitors.TryGetValue(ipAddress, out var lastVisitTime))
        {
            Console.WriteLine(ipAddress);
            if (DateTime.UtcNow - lastVisitTime <= _visitWindow)
            {
                return true;
            }
        }
        _recentVisitors[ipAddress] = DateTime.UtcNow;
        RemoveOutdatedVisitors();

        return false;
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
