using PartProg3;
using System;
using System.Text.RegularExpressions;

namespace PartProg3
{
    public class ChatbotInterface
    {
        private readonly Chatbot _chatbot;
        public string Name { get; private set; } = "User";
        public string UserInterest { get; private set; } = "";

        // 🔁 Track the last topic and current detail level
        private string lastTopic = "";
        private int detailLevel = 0;

        public ChatbotInterface()
        {
            _chatbot = new Chatbot();
        }
        //new 
        public void SetName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("❌ Name cannot be empty.");

            if (!Regex.IsMatch(value, @"^[A-Za-z]+$"))
                throw new ArgumentException("❌ Name must only contain letters.");

            Name = char.ToUpper(value[0]) + value.Substring(1).ToLower();
        }

        public string InitialGreeting()
        {
            return $"Hi {Name}, would you like to take a quick cybersecurity quiz? (yes/no)";
        }

        public string GenerateAnswer(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return $"😕 {Name}, I didn’t catch that. Could you try asking again?";

            question = question.Trim().ToLower();

            // 🔁 Handle "more" follow-up logic
            if (question == "more" || question == "tell me more")
            {
                if (string.IsNullOrEmpty(lastTopic))
                    return $"❓ {Name}, there's no topic selected yet. Try asking about something like 'phishing' or 'encryption' first.";

                detailLevel++;

                if (detailLevel > 3)
                    return $"📚 {Name}, you've seen all the details I have on that topic.";

                return GetMoreInfo(lastTopic, detailLevel);
            }

            // Save user interest if present
            var match = Regex.Match(question, @"interested in\s+(.*)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                UserInterest = match.Groups[1].Value.Trim('.', ' ', '?');
            }

            // Detect sentiment
            string sentiment = DetectSentiment(question);
            if (sentiment == "negative")
                return $"😟 {Name}, I'm sorry you're feeling that way. Cybersecurity can be frustrating, but I'm here to help!";
            if (sentiment == "positive")
                return $"😊 {Name}, I'm glad to hear that! Let’s keep you safe online.";

            // 📤 Call the chatbot's main generator
            string initialResponse = _chatbot.GenerateAnswer(question, Name, UserInterest);

            // 🧠 Detect the topic from the question
            string topic = ExtractTopicFrom(question);
            if (!string.IsNullOrEmpty(topic))
            {
                lastTopic = topic;
                detailLevel = 1;
            }

            // Process the response to personalize and handle fallback messages
            string finalResponse = ProcessChatbotResponse(initialResponse, question);

            return finalResponse;
        }

        public string ProcessChatbotResponse(string initialResponse, string userInput)
        {
            string response = initialResponse;

            if (string.IsNullOrWhiteSpace(response) || response.Contains("I'm not sure"))
            {
                if (string.IsNullOrWhiteSpace(response))
                {
                    response = $"**Hi {Name}, I didn't find a matching topic for \"{userInput}\".**";
                }
                else
                {
                    response = $"**Hi {Name}**, {response.Replace("I'm not sure", "I'm not sure what you mean by that")}";
                }

                response += "\n🤔 Try asking about a specific topic like phishing, firewalls, or password safety.";
            }

            return response;
        }

        /// <summary>
        /// Proxy method to call Chatbot's GetMoreInfo.
        /// </summary>
        public string GetMoreInfo(string topic, int level)
        {
            return _chatbot.GetMoreInfo(topic, level);
        }

        private string DetectSentiment(string input)
        {
            string lowered = input.ToLower();
            string[] positiveWords = { "thank", "great", "awesome", "cool", "good", "helpful", "appreciate" };
            string[] negativeWords = { "bad", "terrible", "hate", "annoying", "useless", "frustrated", "stupid", "sad" };

            foreach (var word in positiveWords)
                if (lowered.Contains(word)) return "positive";

            foreach (var word in negativeWords)
                if (lowered.Contains(word)) return "negative";

            return "neutral";
        }

        // ✅ Extract topic keyword from user input
        private string ExtractTopicFrom(string input)
        {
            string[] knownTopics = {
                "phishing", "encryption", "firewalls", "malware", "ransomware", "backup", "password safety",
                "social engineering", "cloud security", "wifi security", "incident responses", "mobile security",
                "email security", "two-factor authentication", "physical security", "software updates", "insider threats"
            };

            foreach (string topic in knownTopics)
            {
                if (input.Contains(topic))
                    return topic;
            }

            return "";
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
