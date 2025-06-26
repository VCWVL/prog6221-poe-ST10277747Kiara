using System;
using System.Collections.Generic;
using System.Linq;

namespace PartProg3
{
    public class ActivityLogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Message { get; set; } = "";

        public override string ToString() => $"[{Timestamp:g}] {Message}";
    }

    public class ActivityLogger
    {
        private readonly List<ActivityLogEntry> logEntries = new();
        private const int PageSize = 5;

        public void Add(string message)
        {
            logEntries.Add(new ActivityLogEntry { Message = message });
        }

        public List<ActivityLogEntry> GetRecent(int page = 0)
        {
            return logEntries
                .OrderByDescending(entry => entry.Timestamp)
                .Skip(page * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public int TotalEntries => logEntries.Count;
        public int PageSizeValue => PageSize;
    }
}
