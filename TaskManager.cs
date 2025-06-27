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
}// task class

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
// Dev Details:
// Name: Kiara Israel
// Student Number: ST10277747
// Module: PROG6221
//
// References:

// Smith, J., 2022. *Developing intelligent chatbots using C#*. Cambridge: Anglia Tech Publishers.
// Pearson IT Certification, 2023. *Effective Cybersecurity by William Stallings*. [online] Available at: <https://www.pearsonitcertification.com/store/effective-cybersecurity-9780134772806> [Accessed 24 May 2025].
// Wiley, 2021. *Phishing Dark Waters: The Offensive and Defensive Sides of Malicious Emails*. [online] Available at: <https://www.wiley.com/en-us/Phishing+Dark+Waters:+The+Offensive+and+Defensive+Sides+of+Malicious+Emails-p-9781118958473> [Accessed 24 May 2025].
// Cambridge University Press, 2021. *The Conversational Interface: Talking to Smart Devices*. (online) Available at: <https://www.cambridge.org/core/books/conversational-interface/7D5F76AB8D7D4F8F8C2CE6F3EF3D12BD> [Accessed 24 May 2025].
// National Cyber Security Centre (NCSC), 2021. *10 Steps to Cyber Security*. (online) Available at: <https://www.ncsc.gov.uk/collection/10-steps> [Accessed 24 May 2025].
// SpringerLink, 2022. *Human Factors and Information Security: Individual, Culture and Security Environment*. [online] Available at: <https://link.springer.com/book/10.1007/978-3-030-79749-9> [Accessed 24 May 2025].
