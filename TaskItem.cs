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
