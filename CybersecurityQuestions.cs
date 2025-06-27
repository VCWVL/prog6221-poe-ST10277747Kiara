using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgPart3
{
    public static class CybersecurityQuestions
    {
        // Updated and unique cybersecurity quiz questions
        private static readonly List<(string Question, List<string> Options, int CorrectIndex, string Explanation)> questionPool = new()
        {
            // MULTIPLE CHOICE//for quiz
            (
                "You're browsing the internet and a pop-up claims your device is infected. What’s the best action?",
                new List<string> { "Click the link to scan", "Restart the device", "Close the pop-up and run your own antivirus", "Call the number provided" },
                2,
                "Pop-ups with scare tactics often lead to scams. Always use trusted antivirus tools instead of following suspicious prompts."
            ),
            (
                "Which of the following behaviors is the most secure when using a public computer?",
                new List<string> { "Saving passwords in the browser", "Using incognito mode", "Logging into personal bank accounts", "Leaving sessions open for speed" },
                1,
                "Incognito mode prevents the browser from storing history or credentials, which adds a layer of privacy on public machines."
            ),
            (
                "Which practice improves your security posture the most?",
                new List<string> { "Using a long password only", "Changing passwords monthly", "Using a password manager", "Writing down passwords in a notebook" },
                2,
                "A password manager creates and stores complex passwords securely, helping reduce reuse and guessable passwords."
            ),
            (
                "A friend sends you a strange message with a link. What’s your safest move?",
                new List<string> { "Click to see what it is", "Reply and ask", "Open it in another browser", "Avoid clicking and contact them another way" },
                3,
                "It’s safer to verify the message through another method. Friends’ accounts can be compromised and used to spread malware."
            ),
            (
                "Which situation is an example of credential stuffing?",
                new List<string> { "Guessing a password", "Using leaked logins to access accounts", "Phishing scam", "Creating weak passwords" },
                1,
                "Credential stuffing involves using leaked username/password combinations across sites to gain unauthorized access."
            ),

            // TRUE / FALSE
            (
                "Emails that start with 'Dear Customer' instead of your name might be suspicious. True or False?",
                new List<string> { "True", "False" },
                0,
                "Generic greetings are common in phishing emails because attackers don’t know your personal details."
            ),
            (
                "A secure website always starts with 'https' and shows a lock icon. True or False?",
                new List<string> { "True", "False" },
                0,
                "While not foolproof, HTTPS indicates encrypted communication and is essential for safety—especially on login pages."
            ),
            (
                "It's okay to install software from unknown websites if it’s free. True or False?",
                new List<string> { "True", "False" },
                1,
                "Free software from unverified sources may contain malware or unwanted programs. Always use official download sites."
            ),
            (
                "Cybersecurity is only the responsibility of the IT department. True or False?",
                new List<string> { "True", "False" },
                1,
                "Everyone plays a role in cybersecurity—weak links in everyday behavior are often the cause of breaches."
            ),
            (
                "Enabling biometric authentication (like a fingerprint) is safer than typing your password. True or False?",
                new List<string> { "True", "False" },
                0,
                "Biometrics are harder to replicate than passwords, adding a strong layer of identity protection."
            )
        };

        private static readonly Random rng = new();

        // ✅ Get a randomized quiz set
        public static List<(string Question, List<string> Options, int CorrectIndex, string Explanation)> GetAlternatingQuizSet(int numberOfQuestions = 10)
        {
            if (numberOfQuestions > questionPool.Count)
                throw new ArgumentException($"Only {questionPool.Count} questions available.");

            return questionPool.OrderBy(_ => rng.Next()).Take(numberOfQuestions).ToList();
        }

        // ✅ NLP Keyword Detection Helper
        private static readonly Dictionary<string, List<string>> keywordMap = new()
        {
            { "phishing", new List<string> { "phishing", "fraud email", "scam link", "phish" } },
            { "password", new List<string> { "password", "credentials", "login code", "auth" } },
            { "firewall", new List<string> { "firewall", "network block", "packet filter", "access control" } },
            { "ransomware", new List<string> { "ransomware", "file locked", "encrypted files", "ransom demand" } },
            { "2fa", new List<string> { "2fa", "two factor", "multi factor", "code verification" } },
            { "wifi", new List<string> { "wifi", "public network", "wireless", "hotspot" } },
            { "update", new List<string> { "update", "patch", "upgrade", "software fix" } },
            { "social engineering", new List<string> { "social engineering", "deceive", "trick", "manipulate" } }
        };

        // ✅ Detect matched keyword topic in a sentence
        public static string? DetectTopicFromInput(string input)
        {
            string lower = input.ToLower();
            foreach (var pair in keywordMap)
            {
                foreach (var keyword in pair.Value)
                {
                    if (lower.Contains(keyword))
                        return pair.Key;
               }
            }
            return null;
        }
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
