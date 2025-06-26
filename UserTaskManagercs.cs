using System;
using System.Collections.Generic;
using System.Linq;

namespace PartProg3
{
    public class UserTask
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime? Reminder { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            string reminderText = Reminder.HasValue ? $" | Reminder: {Reminder.Value:g}" : "";
            return $"{Title} - {(IsCompleted ? "Completed" : "Pending")}{reminderText}";
        }
    }

    public class UserTaskManager
    {
        private readonly List<UserTask> tasks = new();

        public void AddTask(string title, string description = "", DateTime? reminder = null)
        {
            tasks.Add(new UserTask { Title = title, Description = description, Reminder = reminder, IsCompleted = false });
        }

        public bool CompleteTask(int index)
        {
            if (index >= 0 && index < tasks.Count)
            {
                tasks[index].IsCompleted = true;
                return true;
            }
            return false;
        }

        public bool DeleteTask(int index)
        {
            if (index >= 0 && index < tasks.Count)
            {
                tasks.RemoveAt(index);
                return true;
            }
            return false;
        }

        public List<UserTask> GetAllTasks() => tasks;

        public List<UserTask> GetUpcomingReminders(DateTime now)
        {
            return tasks.Where(t => !t.IsCompleted && t.Reminder.HasValue && t.Reminder.Value <= now).ToList();
        }

        public int TaskCount => tasks.Count;
    }
}
