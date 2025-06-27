using System;
using System.Collections.Generic;
using System.Linq;

public class Task
{
    public string Title { get; }
    public string Description { get; }
    public DateTime Reminder { get; }
    public bool IsCompleted { get; private set; }

    public Task(string title, string description, DateTime reminder)
    {
        Title = title;
        Description = description;
        Reminder = reminder;
        IsCompleted = false;
    }

    public void MarkComplete()
    {
        IsCompleted = true;
    }

    public override string ToString()
    {
        string status = IsCompleted ? "✅ Completed" : "❌ Pending";
        return $"{Title} - {Description} (Reminder: {Reminder:g}) [{status}]";
    }
}

public class TaskManager
{
    private readonly List<Task> tasks = new();

    /// <summary>
    /// Adds a new task and returns the created Task object.
    /// </summary>
    public Task AddTask(string title, string description, DateTime reminder)
    {
        var task = new Task(title, description, reminder);
        tasks.Add(task);
        return task;
    }

    /// <summary>
    /// Deletes a task by zero-based index. Returns true if successful.
    /// </summary>
    public bool DeleteTask(string title)
    {
        var task = tasks.FirstOrDefault(t => string.Equals(t.Title, title, StringComparison.OrdinalIgnoreCase));
        if (task != null)
        {
            tasks.Remove(task);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Marks a task as completed by index.
    /// </summary>
    public bool CompleteTask(int index)
    {
        if (index < 0 || index >= tasks.Count)
            return false;

        tasks[index].MarkComplete();
        return true;
    }

    /// <summary>
    /// Returns all tasks, optionally filtered by completion status.
    /// </summary>
    public IEnumerable<Task> GetTasks(bool? isCompleted = null)
    {
        if (isCompleted == null)
            return tasks;

        return tasks.Where(t => t.IsCompleted == isCompleted.Value);
    }

    /// <summary>
    /// Gets tasks that are due based on the current time.
    /// </summary>
    public IEnumerable<Task> GetDueReminders()
    {
        DateTime now = DateTime.Now;
        return tasks.Where(t => !t.IsCompleted && t.Reminder <= now);
    }

    /// <summary>
    /// Gets the total number of tasks.
    /// </summary>
    public int Count => tasks.Count;
}
