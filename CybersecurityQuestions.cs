using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgPart3
{
    public static class CybersecurityQuestions
    {
        // Multiple-choice question bank
        private static readonly List<(string Question, List<string> Options, int CorrectIndex, string Explanation)> multipleChoice = new()
        {
            ("What is phishing?", new() { "A type of firewall", "An email scam", "A secure password", "A malware virus" }, 1, "Phishing is a scam where attackers trick people via emails."),
            ("What does 2FA stand for?", new() { "Two-Factor Authentication", "Two Firewall Access", "Twice Fast Authorization", "Top File Access" }, 0, "2FA adds an extra step of security beyond just a password."),
            ("Which of these is a strong password?", new() { "123456", "qwerty", "Summer2023!", "password" }, 2, "Strong passwords mix letters, numbers, and symbols."),
            ("What is ransomware?", new() { "A software update", "A firewall feature", "Malware that demands payment", "A type of backup" }, 2, "Ransomware locks your files and demands money."),
            ("Which one is a safe browsing habit?", new() { "Clicking unknown links", "Using HTTPS websites", "Ignoring software updates", "Sharing personal info on public WiFi" }, 1, "HTTPS ensures secure communication online."),
            ("Which is a recommended way to back up data?", new() { "USB only", "Cloud + local", "CD-ROM", "No backup needed" }, 1, "Using both cloud and local backups is safest."),
            ("What does encryption do?", new() { "Deletes data", "Scrambles data to protect it", "Stores data offline", "Makes files larger" }, 1, "Encryption secures data by encoding it."),
            ("Which device can be hacked?", new() { "Smartphone", "Smart TV", "Laptop", "All of the above" }, 3, "Any smart device connected to the internet can be hacked."),
            ("Which tool helps prevent network attacks?", new() { "Notepad", "Firewall", "Task Manager", "Recycle Bin" }, 1, "Firewalls block unauthorized network access."),
            ("Which action improves password safety?", new() { "Using the same password everywhere", "Writing it on a sticky note", "Using a password manager", "Never changing it" }, 2, "Password managers store strong passwords securely.")
        };

        // True/False question bank
        private static readonly List<(string Question, List<string> Options, int CorrectIndex, string Explanation)> trueFalse = new()
        {
            ("Using 'password' as your password is safe.", new() { "True", "False" }, 1, "Common passwords are very insecure."),
            ("You should update your software regularly.", new() { "True", "False" }, 0, "Updates fix bugs and patch security holes."),
            ("Hackers never target mobile phones.", new() { "True", "False" }, 1, "Mobile phones can be hacked too."),
            ("Firewalls can help block malicious traffic.", new() { "True", "False" }, 0, "Firewalls filter incoming and outgoing traffic."),
            ("Cybersecurity is only the IT department's job.", new() { "True", "False" }, 1, "Everyone plays a role in cybersecurity."),
            ("Backing up your data is optional.", new() { "True", "False" }, 1, "Regular backups are essential to avoid data loss."),
            ("Clicking pop-ups is a safe practice.", new() { "True", "False" }, 1, "Pop-ups can often be malicious."),
            ("Public Wi-Fi is always safe to use.", new() { "True", "False" }, 1, "Avoid entering sensitive info on public Wi-Fi."),
            ("Antivirus software can help detect malware.", new() { "True", "False" }, 0, "Antivirus scans for and removes threats."),
            ("Cyberbullying is not related to cybersecurity.", new() { "True", "False" }, 1, "It is a digital safety issue tied to cybersecurity.")
        };

        /// <summary>
        /// Returns a randomized mix of MCQ and T/F quiz questions.
        /// </summary>
        /// <param name="mcqCount">Number of multiple-choice questions</param>
        /// <param name="tfCount">Number of true/false questions</param>
        /// <returns>List of combined quiz questions</returns>
        public static List<(string Question, List<string> Options, int CorrectIndex, string Explanation)> GetRandomQuizSet(int mcqCount, int tfCount)
        {
            var random = new Random();

            var selectedMCQ = multipleChoice.OrderBy(_ => random.Next()).Take(mcqCount).ToList();
            var selectedTF = trueFalse.OrderBy(_ => random.Next()).Take(tfCount).ToList();

            return selectedMCQ.Concat(selectedTF).ToList();
        }
    }
}
