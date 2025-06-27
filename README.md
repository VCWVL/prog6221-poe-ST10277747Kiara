# 🛡️ PartProg3 – Cybersecurity Awareness Chatbot (WPF Application)

## 📖 Project Overview

PartProg3 is a desktop chatbot application built with C# and WPF using .NET 9.0. It aims to educate users about cybersecurity concepts through interactive conversations. The chatbot detects keywords in user input, explains over 20 cybersecurity topics with layered detail, and supports additional features like task management, reminders, sentiment recognition, and a quiz mini-game. This project combines AI-inspired dialogue with practical productivity tools to engage users in learning about online safety.

## 🌟 Key Features and Functionality

- **Keyword-Based Topic Recognition:** The chatbot understands user questions by detecting keywords like “phishing,” “malware,” or “VPN,” providing concise, emoji-enhanced explanations.
- **Multi-Level Information:** Users can request “tell me more” to get deeper, progressively detailed knowledge on each cybersecurity topic, supporting up to 3 levels of detail.
- **Task and Reminder Management:** Users can create, list, and delete tasks with optional reminders, allowing simple personal task tracking inside the chatbot interface.
- **Sentiment Detection:** The chatbot identifies emotional phrases such as “I feel sad” or “I’m happy” and responds empathetically.
- **Interactive Quiz:** A 10-question quiz tests user knowledge of cybersecurity topics with immediate feedback, score tracking, and motivational messages.
- **Activity Log:** Keeps a history of recent interactions, including tasks created, quiz attempts, and topics discussed, with commands to view or expand the log.
- **User-Friendly Interface:** Includes ASCII art banner, emoji usage, and clean, color-coded responses for an engaging user experience.

## 🛠️ How to Set Up and Run

1. **Prerequisites:**
   - Windows OS (10 or later)
   - .NET 9.0 SDK installed
   - Visual Studio 2022 or later with WPF support

2. **Running the Application:**
   - Clone or download the project repository.
   - Open the solution file (`PartProg3.sln`) in Visual Studio.
   - Build the project (`Build > Build Solution` or press `F6`).
   - Start the application (`Debug > Start Debugging` or press `F5`).
   - When prompted, enter your name and begin chatting with the bot.

3. **Example Interactions:**
   - Ask questions like:  
     `What is phishing?`  
     `Tell me more about ransomware.`  
     `Create task Finish assignment tomorrow at 5pm.`  
     `Start quiz`  
     `Show activity log`

## 📂 Project Structure Overview

- **MainWindow.xaml & MainWindow.xaml.cs:** User interface and event handling.
- **Chatbot.cs:** Core chatbot logic including keyword detection, responses, and memory.
- **CybersecurityQuestions.cs:** Contains quiz questions, answers, and scoring.
- **TaskManager.cs & Task.cs:** Manage user tasks and reminders.
- **ActivityLogger.cs:** Records recent user and chatbot actions.

Video Of Code Explanation and Running Code Explanation:
https://youtu.be/9cDex5zfg5A

## 👩‍💻 Author and Module

Developed by **Kiara Israel** as part of the PROG6221 Advanced Programming module, focusing on building an intelligent chatbot to promote cybersecurity awareness through engaging user interaction.
This project showcases a full-featured WPF desktop chatbot that doesn’t just inform — it teaches, quizzes, tracks, and supports users on their cybersecurity journey.

Thank you for your time. I’d be happy to answer any questions.
---

