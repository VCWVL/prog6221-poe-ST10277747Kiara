using System;

public class TaskItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Reminder { get; set; }
    public bool IsComplete { get; set; }

    public TaskItem(string title, string description, DateTime reminder)
    {
        Title = title;
        Description = description;
        Reminder = reminder;
        IsComplete = false;
    }

    public override string ToString()
    {
        string status = IsComplete ? "✅ Completed" : "⏳ Pending";
        return $"{Title} (Reminder: {Reminder:yyyy/MM/dd HH:mm}) - {status}";
    }
}
