using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgPart3
{
    public static class CybersecurityQuestions
    {
        // Quiz data: T/F and Multiple Choice
        private static readonly List<(string Question, List<string> Options, int CorrectIndex, string Explanation)> questionPool = new()
        {
            // Multiple Choice
            (
                "What should you do if you receive an email asking for your password?",
                new List<string> { "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it" },
                2,
                "You should never share your password via email. Reporting phishing emails helps protect others."
            ),
            (
                "Which of the following is NOT a strong password?",
                new List<string> { "P@ssw0rd123", "123456", "8&*aB!9", "A mix of letters, numbers, and symbols" },
                1,
                "Simple sequences like '123456' are easy to guess and are considered weak passwords."
            ),
            (
                "What does two-factor authentication (2FA) provide?",
                new List<string> { "Only a username and password", "Two passwords", "An additional layer of security", "Faster login" },
                2,
                "2FA adds an extra security step beyond passwords, like a code or fingerprint."
            ),
            (
                "What type of malware locks your files and demands payment?",
                new List<string> { "Virus", "Ransomware", "Spyware", "Adware" },
                1,
                "Ransomware encrypts your files and demands a ransom to unlock them."
            ),
            (
                "What is the main purpose of a firewall?",
                new List<string> { "To monitor network traffic", "To increase internet speed", "To block unauthorized access", "To store passwords" },
                2,
                "Firewalls are used to block unauthorized access while allowing safe traffic."
            ),
            (
                "Which of these is an example of social engineering?",
                new List<string> { "Phishing email", "Antivirus scan", "Software update", "Two-factor authentication" },
                0,
                "Phishing is a social engineering tactic used to trick users into giving information."
            ),

            // True/False
            (
                "Phishing is a technique to trick users into giving sensitive info. True or False?",
                new List<string> { "True", "False" },
                0,
                "Phishing involves fraudulent attempts to obtain personal or financial information."
            ),
            (
                "Using the same password on multiple sites is safe. True or False?",
                new List<string> { "True", "False" },
                1,
                "Using the same password puts all accounts at risk if one is breached."
            ),
            (
                "Software updates help protect your computer from vulnerabilities. True or False?",
                new List<string> { "True", "False" },
                0,
                "Updates patch security holes that could be exploited by hackers."
            ),
            (
                "Public WiFi is always secure for sensitive transactions. True or False?",
                new List<string> { "True", "False" },
                1,
                "Public WiFi can be intercepted, making it unsafe for sensitive tasks."
            )
        };

        private static readonly Random rng = new();

        // ✅ Get a randomized 10-question quiz
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

        // ✅ Detect matched keyword topic in a sentence (for NLP part 3.c, 3.d)
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
            return null; // No match
        }
    }
}
