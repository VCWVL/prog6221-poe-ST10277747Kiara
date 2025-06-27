using PartProg3;
using System;
using System.Globalization;
using System.Media;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ProgPart3
{
    public partial class MainWindow : Window
    {
        private readonly ChatbotInterface chatbotInterface;
        private TaskManager taskManager = new TaskManager(); // ✅ Add this line
        private string userName = "";
        private bool isAskingName = true;
        private bool waitingForTopic = false;
        private bool quizActive = false;
        private bool postQuizPrompt = false; // New flag to track quiz end prompt
        private bool hasAskedAboutTopics = false;




        private int quizIndex = 0;
        private int quizScore = 0;
        private List<(string Question, List<string> Options, int CorrectIndex, string Explanation)> quizQuestions = new();
        private List<string> activityLog = new List<string>();
        private int activityLogIndex = 0;




        private static bool hasPlayedIntroAudio = false;
        private SoundPlayer introPlayer = null;
        private SoundPlayer sendClickPlayer = null;
        // 2FA and Authentication
        private bool isAuthenticating = true;
        private bool awaitingPasswordSetup = true;
        private bool awaiting2FACode = false;
        private bool awaitingNameInput = false;
        private string? userSetPassword = null;
        private string? generated2FACode = null;

        private void ShowFullTaskReport()
        {
            var allTasks = taskManager.GetTasks().ToList(); // ✅ Correct method


            if (allTasks.Count == 0)
            {
                AddBotMessage("📭 No tasks to report.");
                return;
            }

            AddBotMessage("📋 Full Task Report:");

            foreach (var task in allTasks)
            {
                string reminder = task.Reminder != DateTime.MinValue ? $"🕓 {task.Reminder:g}" : "⏳ No reminder set";
                string status = task.IsCompleted ? "✅ Completed" : "❌ Pending"; // ✅ Correct property name

                AddBotMessage($"• {task.Title}\n   {reminder}\n   Status: {status}");
            }
        }
         public MainWindow()
        {
            InitializeComponent();
            chatbotInterface = new ChatbotInterface();
            Loaded += MainWindow_Loaded;

            string introAudioPath = "ElevenLabs_Text_to_Speech_audio.wav";
            if (System.IO.File.Exists(introAudioPath))
                introPlayer = new SoundPlayer(introAudioPath);

            string sendClickPath = "send_click.wav";
            if (System.IO.File.Exists(sendClickPath))
                sendClickPlayer = new SoundPlayer(sendClickPath);
        }

        private void ShowActivityLog()
        {
            if (activityLog.Count == 0)
            {
                AddBotMessage("🗂️ I haven’t done anything yet.");
                return;
            }

            int itemsToShow = 5;
            var recentActions = activityLog.Skip(activityLogIndex).Take(itemsToShow).ToList();

            AddBotMessage("📜 Here's a summary of recent actions:");
            for (int i = 0; i < recentActions.Count; i++)
            {
                AddBotMessage($"{activityLogIndex + i + 1}. {recentActions[i]}");
            }

            if (activityLogIndex + itemsToShow < activityLog.Count)
            {
                AddBotMessage("🔄 Type 'show more log' to see additional history.");
                activityLogIndex += itemsToShow;
            }
            else
            {
                activityLogIndex = 0; // Reset if we're at the end
            }
        }
           private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PlayIntroAudio();
            // AddBotMessage("Welcome to the Cybersecurity Chatbot! 🛡️");
            AddBotMessage("🔐 Please create a password to get started:");
            AppendAsciiArtWithMenu();
            //  AddBotMessage("Please enter your name to get started:");

        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
                e.Handled = true;
            }
        }


        private void ProcessUserInput()
        {
            string input = InputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            PlaySendClickSound(input); // ✅ Pass the current user input
            AddUserMessage(input);
            InputTextBox.Clear();

            // 🔐 2FA Authentication Flow using Authenticator class
            if (isAuthenticating)
            {
                // Step 1: User creates password
                if (awaitingPasswordSetup)
                {
                    userSetPassword = input;
                    Authenticator.SetPassword(input); // ✅ Store user's password in Authenticator
                    awaitingPasswordSetup = false;
                    AddBotMessage("✅ Password saved! Please type it again to log in:");
                    return;
                }

                // Step 2: Confirm password & generate 2FA
                // This block executes if password setup is done, and we are not yet awaiting 2FA code
                if (!awaiting2FACode && userSetPassword != null)
                {
                    if (!Authenticator.VerifyPassword(input)) // ✅ Check using Authenticator
                    {
                        AddBotMessage("❌ Incorrect password. Please try again:");
                        return;
                    }

                    // Password is correct, now generate and send 2FA
                    generated2FACode = Authenticator.Generate2FACode();
                    awaiting2FACode = true; // Set flag to true, indicating we are now waiting for 2FA
                    AddBotMessage($"📨 2FA code sent to your (simulated) email: **{generated2FACode}**");
                    AddBotMessage("Please enter the 2FA code to complete login:");
                    return; // Important: Return after prompting for 2FA
                }

                // Step 3: Verify 2FA code (after it has been sent)
                if (awaiting2FACode)
                {
                    // Verify the entered 2FA code matches the generated one
                    // Or, as per your screenshot, accept any 6-digit code for simulation.
                    // I'll stick to the screenshot's logic for simplicity: "Accept any 6-digit code"
                    if (Regex.IsMatch(input, @"^\d{6}$"))
                    {
                        // If you want to check against the 'generated2FACode':
                        // if (input == generated2FACode) { ... }
                        // For the "Accept any 6-digit code" as per screenshot:
                        AddBotMessage("✅ 2FA successful!");
                        awaiting2FACode = false; // 2FA is complete

                        // Now, transition to asking for the user's name
                        AddBotMessage("Please enter your name to get started:");
                        awaitingNameInput = true; // Set new flag for name input
                        isAuthenticating = false; // Authentication process (password + 2FA) is done
                                                  // Set isAuthenticating to false here, as name input is more of a profile setup.
                    }
                    else
                    {
                        AddBotMessage("❌ Invalid 2FA code. Please enter a 6-digit code.");
                    }
                    return; // Important: Return after handling 2FA input
                }
            }

            // Handle name input after successful 2FA
            if (awaitingNameInput)
            {
                // Validate the name (e.g., must contain only letters as per your screenshot)
                if (Regex.IsMatch(input, @"^[a-zA-Z]+$")) // Only letters, one or more
                {
                    userName = input; // Store the user's name
                    awaitingNameInput = false; // Name input is complete

                    AddBotMessage($"🎉 Welcome {userName}! Would you like to learn about cybersecurity topics? (yes/no)");
                    // You might set a flag here to prompt for "yes/no" if that's the next step
                    // e.g., waitingForInitialTopicPrompt = true;
                    return; // Important: Return after handling name input
                }
                else
                {
                    AddBotMessage("❌ Name must only contain letters. Please try again.");
                    return; // Keep awaitingNameInput true, so it prompts again.
                }
            }
            string lowInput = input.ToLower();


            // 🧠 NLP: Smart Reminder Extraction
            Match fullReminder = Regex.Match(lowInput, @"remind me (to|about)? (.+?)( on (\w+)| at (\d{1,2}(:\d{2})?\s?(am|pm)?)| tomorrow| next week)?");

            if (fullReminder.Success)
            {
                string taskTitle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fullReminder.Groups[2].Value.Trim());
                string hourPart = fullReminder.Groups[5].Value;
                string period = fullReminder.Groups[7].Value.ToLower();

                DateTime reminderTime = DateTime.Now;

                if (lowInput.Contains("tomorrow"))
                {
                    reminderTime = DateTime.Now.Date.AddDays(1).AddHours(9);
                }
                else if (lowInput.Contains("next week"))
                {
                    reminderTime = DateTime.Now.Date.AddDays(7).AddHours(9);
                }
                else if (!string.IsNullOrEmpty(hourPart) && DateTime.TryParse(hourPart, out DateTime parsedTime))
                {
                    reminderTime = DateTime.Today.Add(parsedTime.TimeOfDay);
                }
                // ✅ Add the task and log it
                var task = taskManager.AddTask(taskTitle, "", reminderTime);
                activityLog.Add($"🔔 Reminder added: {task.Title} - {reminderTime:g}");

                // 📌 Show summary and full report
                AddBotMessage($"⏰ Reminder set for: **{task.Title}** on {reminderTime:dddd, MMM dd @ HH:mm}.");
                AddBotMessage("📌 Task Summary:");
                AddBotMessage($"• Task: {task.Title}\n• Reminder Time: {reminderTime:g}");

                ShowFullTaskReport(); // 🧾 Display all tasks after reminder set
                return;
            }
            // Handle showing full task report
            if (lowInput.Contains("view task") || lowInput.Contains("view tasks") ||
            lowInput.Contains("show task") || lowInput.Contains("show tasks") ||
            lowInput.Contains("list task") || lowInput.Contains("list tasks"))
            {
                {
                    if (hasAskedAboutTopics && quizIndex >= 1)  // Your condition to allow viewing tasks
                    {
                        AddBotMessage("📋 Here is your FULL task report:");
                        ShowFullTaskReport();

                    }
                    else
                    {
                        AddBotMessage("📚 Please learn at least one cybersecurity topic before viewing your tasks.");
                    }
                    ShowFullTaskReport(); // Or ShowAllTasks() depending on your implementation
                    return;
                }
            // 🗑️ NLP: Delete Task
Match deleteMatch = Regex.Match(lowInput, @"(delete|remove) task (.+)");
if (deleteMatch.Success)
{
                    string titleToDelete = deleteMatch.Groups[2].Value.Trim();
                    bool deleted = taskManager.DeleteTask(titleToDelete);

                    if (deleted)
                    {
                        AddBotMessage($"🗑️ Task '{titleToDelete}' deleted successfully.");
                        activityLog.Add($"🗑️ Task deleted: {titleToDelete}");
                    }
                    else
                    {
                        AddBotMessage($"⚠️ Could not find a task titled '{titleToDelete}'.");
                    }

                    return;
}

            }
        

// ✅ Handle Activity Log Commands
if (lowInput.Contains("show activity log") || lowInput.Contains("what have you done") || lowInput.Contains("show log"))
{
    ShowActivityLog(); // Shows last 5-10 activity items
    return;
}

// ✅ Handle "Show More Log" for pagination
if (lowInput.Contains("show more log") || lowInput.Contains("more actions"))
{
    ShowActivityLog(); // Shows next 5 logs (if available)
    return;
}
          // ======== Inserted NLP Task & Reminder Detection Start ========

// ✅ NLP: "Remind me to [something] tomorrow"
  Match remindTomorrowMatch = Regex.Match(lowInput, @"remind me to (.+) tomorrow");
            if (remindTomorrowMatch.Success)
            {
                string title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(remindTomorrowMatch.Groups[1].Value.Trim());
                DateTime reminder = DateTime.Now.Date.AddDays(1).AddHours(9);

                var task = taskManager.AddTask(title, "", reminder);
                AddBotMessage($"⏰ Reminder set for '{task.Title}' on {reminder:dddd, MMMM dd} at {reminder:HH:mm}.");
                activityLog.Add($"🔔 Reminder: {task.Title} - {reminder:g}");
                return;
            }

            // ✅ NLP: "Add a task to [something]" or "Create a task to [something]"
            Match taskMatch = Regex.Match(lowInput, @"(add|create) a task (to|for) (.+)");
            if (taskMatch.Success)
            {
                string title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(taskMatch.Groups[3].Value.Trim());
                var task = taskManager.AddTask(title, "", DateTime.MinValue);

                AddBotMessage($"📝 Task added: '{task.Title}'. Would you like me to remind you about it later?");
                activityLog.Add($"📝 Task created: {task.Title}");
                return;
            }//tasks

            // ✅ NLP: "What have you done for me" or "summary"
            if (lowInput.Contains("what have you done") || lowInput.Contains("summary"))
            {
                if (activityLog.Count == 0)
                {
                    AddBotMessage("🗂️ I haven’t done anything yet.");
                }
                else
                {
                    AddBotMessage("📋 Here's a summary of recent actions:");
                    foreach (var action in activityLog.TakeLast(5))
                    {
                        AddBotMessage(action);
                    }
                }
                return;
            }

            // ======== Inserted NLP Task & Reminder Detection End ========


            // ✅ Handle quiz answer if quiz is active
            if (quizActive)
            {
                HandleQuizAnswer(input); // This method checks if the answer is correct
                return;
            }

            // ✅ Start quiz if user types "quiz"
            if (lowInput == "quiz")
            {
                quizQuestions = CybersecurityQuestions.GetAlternatingQuizSet(10);
                quizIndex = 0;
                quizScore = 0;
                quizActive = true;

                AddBotMessage("🧠 Starting the Cybersecurity Quiz! Let's begin:");
                AskQuizQuestion(); // This method sends the first question
                activityLog.Add($"🧠 Quiz started - {quizQuestions.Count} questions.");
                activityLog.Add($"📘 Quiz question answered (Q{quizIndex + 1}): '{input}'");
                activityLog.Add($"🧠 Quiz progress: {quizIndex}/{quizQuestions.Count} questions completed.");

                return;
            }


          //  if (lowInput == "menu")
           // {
             //   AppendAsciiArtWithMenu();
              //  AddBotMessage("Please select a topic to learn more.");
            //    waitingForTopic = true;
                //return;
          //}

            if (postQuizPrompt)
            {
                postQuizPrompt = false;
                if (lowInput == "yes" || lowInput == "y")
                {
                    AppendAsciiArtWithMenu();
                    AddBotMessage("Great! Please select a topic to learn more.");
                    waitingForTopic = true;
                }
                else
                {
                    AddBotMessage("Alright! You can always type 'menu' or a topic name later.");
                }
                return;
            }

            if (lowInput.Contains("definition") || lowInput.Contains("learn") || lowInput.Contains("menu") || lowInput.Contains("topic") || Regex.IsMatch(lowInput, "^\\d+$"))
            {
                AppendAsciiArtWithMenu();
                AddBotMessage("You can choose any topic above to learn more. Type the name of a topic to begin.");
                waitingForTopic = true;
                return;
            }

            // This 'yes/no' block should now be the *initial* prompt for learning topics
            // after the name is collected. You might need a new flag like `initialTopicPromptReady`
            // to control when these 'yes/no' responses are expected, otherwise they might trigger
            // incorrectly. For simplicity, I'm assuming it handles the general "yes/no" after name.
            if (lowInput == "yes" || lowInput == "y")
            {
                AddBotMessage("Great! Please type the cybersecurity topic you want to learn about (e.g., phishing, firewalls):");
                waitingForTopic = true;
                return;
            }
            else if (lowInput == "no" || lowInput == "n")
            {
                AddBotMessage("Okay! You can always type 'quiz' to start the cybersecurity quiz or 'menu' to see topic list.");
                return;
            }

            if (waitingForTopic)
            {
                // Check if the input starts with a number (e.g., "1", "3 phishing")
                Match numberedMatch = Regex.Match(input.Trim(), @"^(\d{1,2})(.*)?");
                if (numberedMatch.Success)
                {
                    int number = int.Parse(numberedMatch.Groups[1].Value);
                    if (numberedTopics.TryGetValue(number, out string topic))
                    {
                        HandleTopicRequest(topic); // Use mapped topic
                        return;
                    }
                }

                // Fallback to regular topic detection
                HandleTopicRequest(input);
                return;
            }


            // Default response if no other state matches
            string response = chatbotInterface.GenerateAnswer(input);
            AddBotMessage(response);
        }

        private void HandleTopicRequest(string input)
        {
            string topicResponse = chatbotInterface.GenerateAnswer(input);
            AddBotMessage(topicResponse);
            AddBotMessage("Would you like to learn about another cybersecurity topic? (yes/no)");
            waitingForTopic = false;
        }

        private void AskQuizQuestion()
        {
            if (quizIndex < quizQuestions.Count)
            {
                var question = quizQuestions[quizIndex];
                int questionNumber = quizIndex + 1; // Convert 0-based to 1-based for display

                string message = $"🧠 Question {questionNumber}:\n{question.Question}\n";

                for (int i = 0; i < question.Options.Count; i++)
                {
                    message += $"{i + 1}. {question.Options[i]}\n";
                }

                AddBotMessage(message);
            }
            else
            {
                AddBotMessage("✅ All questions completed!");
            }
        }


        private void ProcessQuizAnswer(string userAnswer)
        {
            if (!quizActive)
                return;

            if (quizIndex < 0 || quizIndex >= quizQuestions.Count)
            {
                AddBotMessage("⚠️ Error: Question index out of range.");
                return;
            }

            var currentQuestion = quizQuestions[quizIndex];
            string correctAnswer = currentQuestion.Options[currentQuestion.CorrectIndex];

            // Check if the answer is correct
            bool correct = string.Equals(userAnswer.Trim(), correctAnswer.Trim(), StringComparison.OrdinalIgnoreCase);

            if (correct)
            {
                quizScore++;
                AddBotMessage("✅ Correct!");
            }
            else
            {
                AddBotMessage($"❌ Incorrect. The correct answer was: {correctAnswer}");
            }

            // ✅ INCREMENT quizIndex before progress or next question
            quizIndex++;

            // ✅ Show score & feedback after at least 3 questions answered, but before last
            if (quizIndex >= 3 && quizIndex < quizQuestions.Count)
            {
                double percent = (quizScore * 100.0) / quizIndex;
                string feedback = percent >= 80 ? "🎉 Great job! You're a cybersecurity pro!" :
                                  percent >= 50 ? "🙂 Not bad! Keep learning to stay safe online." :
                                  "😟 Keep learning to improve your cybersecurity knowledge.";

                AddBotMessage($"📊 Progress: {quizScore}/{quizIndex} correct.\n{feedback}");
            }

            // ✅ End the quiz after 10 questions
            if (quizIndex == quizQuestions.Count)
            {
                EndQuiz();
                return;
            }

            // ✅ Show next question
            AskQuizQuestion();
        }


        private void HandleQuizAnswer(string input)
        {
            if (quizIndex >= quizQuestions.Count)
            {
                EndQuiz();
                return;
            }

            var q = quizQuestions[quizIndex];
            input = input.Trim().ToUpper();

            if (string.IsNullOrEmpty(input)) return;

            int chosenIndex = input[0] - 'A';
            // Increment questions answered count regardless of correct or incorrect answer
            if (chosenIndex == q.CorrectIndex)
            {
                quizScore++;
                AddBotMessage($"✅ Correct! {q.Explanation}");

                quizIndex++;
                // ✅ Log current quiz progress
                activityLog.Add($"🧠 Quiz progress: {quizIndex}/{quizQuestions.Count} questions completed.");

                if (quizIndex < quizQuestions.Count)
                {
                    AskQuizQuestion();
                }
                else
                {
                    AddBotMessage($"🎉 Quiz completed! Your score: {quizScore}/{quizQuestions.Count}.");
                    quizActive = false;
                    postQuizPrompt = true;
                }
            

            if (quizIndex < quizQuestions.Count)
                {
                    AddBotMessage("👉 Next question coming up...");
                    AskQuizQuestion(); // continue quiz
                }
                else
                {
                    EndQuiz();
                }
            }
            else
            {
                quizActive = false; // break the quiz
                AddBotMessage($"❌ Incorrect. {q.Explanation}");
                AddBotMessage("Would you like another quiz question? (type 'quiz') or go back to the menu (type 'menu')");
            }
        }
      



        private void EndQuiz()
        {
            quizActive = false;
            int totalQuestions = quizIndex; // assuming quizIndex tracks number answered, e.g. 10
            int incorrect = totalQuestions - quizScore;
            double percent = (quizScore * 100.0) / totalQuestions;

            string feedback = percent >= 80 ? "🎉 Great job! You're a cybersecurity pro!" :
                              percent >= 50 ? "🙂 Not bad! Keep learning to stay safe online." :
                              "😟 Keep learning to improve your cybersecurity knowledge.";

            AddBotMessage($"Quiz complete! You scored {quizScore} out of {totalQuestions}.");
            AddBotMessage($"Correct answers: {quizScore}\nIncorrect answers: {incorrect}");
            AddBotMessage(feedback);
            AddBotMessage("Would you like to see the topic definitions again? (yes/no)");
            postQuizPrompt = true;
        }

        // ... rest of the code unchanged (AddUserMessage, AddBotMessage, etc.)

        // Keep everything after this point as it was

        private void AddUserMessage(string message)
        {
            var textBlock = new TextBlock
            {
                Text = $"🙋 {userName}: {message}",
                FontSize = 16,
                Foreground = Brushes.Aqua,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(15, 10, 15, 5)
            };

            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(20, 20, 40)),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Right,
                Child = textBlock,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 4,
                    ShadowDepth = 1,
                    Opacity = 0.4,
                    Color = Colors.Black
                }
            };

            AddBubbleWithAnimation(border);
        }

        private void AddBotMessage(string message, bool isError = false)
        {
            var bgColor = isError ? Colors.IndianRed : Color.FromRgb(30, 30, 30);
            var fgColor = isError ? Brushes.White : Brushes.LightGray;

            var stack = new StackPanel
            {
                Orientation = Orientation.Vertical,
                MaxWidth = 550,
                Margin = new Thickness(10)
            };

            var textBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Foreground = fgColor,
                FontSize = 16,
                Margin = new Thickness(15, 10, 15, 5),
                TextAlignment = TextAlignment.Left
            };

            stack.Children.Add(textBlock);

            var border = new Border
            {
                Background = new SolidColorBrush(bgColor),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left,
                Child = stack,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 4,
                    ShadowDepth = 1,
                    Opacity = 0.4,
                    Color = Colors.Black
                }
            };

            AddBubbleWithAnimation(border);
        }

        private void AddBubbleWithAnimation(Border bubble)
        {
            bubble.Opacity = 0;
            ChatStackPanel.Children.Add(bubble);
            ScrollToEnd();
            var fadeIn = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(350)));
            bubble.BeginAnimation(OpacityProperty, fadeIn);
        }

        private void ScrollToEnd()
        {
            ChatScrollViewer?.ScrollToEnd();
        }
        private void AppendAsciiArtWithMenu()
        {
            string menuContent = @"
╔══════════════════════════════════════════════════════════════════════════════╗
║                    🛡️ CYBERSECURITY CHATBOT 🛡️                                 ║
╠══════════════════════════════════════════════════════════════════════════════╣
                              /\     /\
                             {  `---'  }
                             {  O   O  }
                             ~~>  V  <~~
                              \  \|/  /
                               `-----'____
                               /     \    \_
                              {       }\  )_\_   _
                              |  \_/  |/ /  \_\_/ )
                               \__/  /(_/     \__/
                              (__/

                        🐾 Mittens the Cyber Cat welcomes you! Meow! 🐾

                        📚 Cybersecurity Topics You Can Ask About:
                    ——————————————————————————————————————————————————————
                             1. Cybersecurity
                             2. Backup and Recovery
                             3. Phishing
                             4. Ransomware
                             5. Malware
                             6. Data Encryption
                             7. Firewalls
                             8. Two-factor Authentication
                             9. Password Safety
                            10. Password Security
                            11. Social Engineering
                            12. Safe Browsing
                            13. WiFi Security
                            14. Software Updates
                            15. Incident Responses
                            16. Physical Security
                            17. Insider Threats
                            18. Cloud Security
                            19. Mobile Security
                            20. Email Security
";

            TextBlock asciiBlock = new TextBlock
            {
                Text = menuContent,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Foreground = Brushes.White,
                Background = Brushes.Transparent,
                Padding = new Thickness(20),
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                MaxWidth = 700,
                LineHeight = 20,
                LineStackingStrategy = LineStackingStrategy.BlockLineHeight
            };

            Border border = new Border
            {
                Background = new LinearGradientBrush(
                    Color.FromRgb(25, 25, 112),
                    Color.FromRgb(65, 105, 225),
                    new Point(0, 0),
                    new Point(1, 1)),
                BorderThickness = new Thickness(2),
                BorderBrush = Brushes.CornflowerBlue,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(15),
                HorizontalAlignment = HorizontalAlignment.Center,
                Child = asciiBlock,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.CornflowerBlue,
                    BlurRadius = 12,
                    ShadowDepth = 0,
                    Opacity = 0.8
                }
            };

            AddBubbleWithAnimation(border);
        }
        private readonly Dictionary<int, string> numberedTopics = new()
{
    { 1, "cybersecurity" },
    { 2, "backup and recovery" },
    { 3, "phishing" },
    { 4, "ransomware" },
    { 5, "malware" },
    { 6, "data encryption" },
    { 7, "firewalls" },
    { 8, "two-factor authentication" },
    { 9, "password safety" },
    {10, "password security" },
    {11, "social engineering" },
    {12, "safe browsing" },
    {13, "wifi security" },
    {14, "software updates" },
    {15, "incident responses" },
    {16, "physical security" },
    {17, "insider threats" },
    {18, "cloud security" },
    {19, "mobile security" },
    {20, "email security" }
};


        private void PlayIntroAudio()
        {

            if (hasPlayedIntroAudio) return;

            try
            {
                introPlayer?.Load();
                introPlayer?.Play();
                hasPlayedIntroAudio = true;
            }
            catch (Exception ex)
            {
                AddBotMessage("🔇 Audio playback failed: " + ex.Message, isError: true);
            }
        }

        private void PlaySendClickSound(string input)
        {
            try
            {
                sendClickPlayer?.Play();
            }
            catch { }

            if (input.ToLower() == "exit")
            {
                string botName = string.IsNullOrEmpty(chatbotInterface.Name) ? "Cyber Bot" : chatbotInterface.Name;
                string catName = "CyberCat";

                AddBotMessage($"👋 Goodbye, {botName}! {catName} says 'Meow!' Stay safe! 🧑‍💻🐾");
                AddBotMessage("════════════════════════════════════════════════════════════════════════════════════════════");

                System.Threading.Tasks.Task.Run(async () =>
                {
                    await System.Threading.Tasks.Task.Delay(1500);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Application.Current.Shutdown();
                    });
                });

                return;
            }
        }
    }}