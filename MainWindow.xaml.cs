using PartProg3;
using System;
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
        private string userName = "";
        private bool isAskingName = true;
        private bool waitingForTopic = false;
        private bool quizActive = false;
        private bool postQuizPrompt = false; // New flag to track quiz end prompt

        private int quizIndex = 0;
        private int quizScore = 0;
        private List<(string Question, List<string> Options, int CorrectIndex, string Explanation)> quizQuestions = new();

        private static bool hasPlayedIntroAudio = false;
        private SoundPlayer introPlayer = null;
        private SoundPlayer sendClickPlayer = null;

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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PlayIntroAudio();
            AddBotMessage("Welcome to the Cybersecurity Chatbot! 🛡️");
            AppendAsciiArtWithMenu();
            AddBotMessage("Please enter your name to get started:");
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

            PlaySendClickSound();
            AddUserMessage(input);
            InputTextBox.Clear();

            if (isAskingName)
            {
                try
                {
                    chatbotInterface.SetName(input);
                    userName = chatbotInterface.Name;
                    isAskingName = false;
                    AddBotMessage($"🎉 Welcome {userName}! Would you like to learn about cybersecurity topics? (yes/no)");
                    return;
                }
                catch (ArgumentException ex)
                {
                    AddBotMessage(ex.Message);
                    return;
                }
            }

            if (quizActive)
            {
                HandleQuizAnswer(input);
                return;
            }

            string lowInput = input.ToLower();

            if (postQuizPrompt)
            {
                postQuizPrompt = false;
                if (lowInput == "yes" || lowInput == "y")
                {
                    AppendAsciiArtWithMenu();
                    AddBotMessage("Great! Please select a topic to learn more.");
                    waitingForTopic = true;
                    return;
                }
                else
                {
                    AddBotMessage("Alright! You can always type 'menu' or a topic name later.");
                    return;
                }
            }

            if (lowInput.Contains("definition") || lowInput.Contains("learn") || lowInput.Contains("menu") || lowInput.Contains("topic") || Regex.IsMatch(lowInput, "^\\d+$"))
            {
                AppendAsciiArtWithMenu();
                AddBotMessage("You can choose any topic above to learn more. Type the name of a topic to begin.");
                waitingForTopic = true;
                return;
            }

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
            else if (lowInput == "quiz")
            {
                StartQuiz();
                return;
            }

            if (waitingForTopic)
            {
                HandleTopicRequest(input);
                return;
            }

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

        private void StartQuiz()
        {
            quizQuestions = CybersecurityQuestions.GetRandomQuizSet(5, 5);
            quizActive = true;
            quizIndex = 0;
            quizScore = 0;
            AskQuizQuestion();
        }


        private void AskQuizQuestion()
        {
            if (quizIndex >= quizQuestions.Count)
            {
                EndQuiz();
                return;
            }

            var q = quizQuestions[quizIndex];
            string questionText = $"Question {quizIndex + 1}: {q.Question}\n";

            for (int i = 0; i < q.Options.Count; i++)
            {
                char optionChar = (char)('A' + i);
                questionText += $"{optionChar}) {q.Options[i]}\n";
            }

            AddBotMessage(questionText.Trim());
        }

        private void HandleQuizAnswer(string input)
        {
            var q = quizQuestions[quizIndex];
            input = input.ToUpper();

            if (input.Length != 1 || input[0] < 'A' || input[0] >= 'A' + q.Options.Count)
            {
                AddBotMessage("Please answer with the letter corresponding to your choice (e.g., A, B, C, or D).");
                return;
            }

            int chosenIndex = input[0] - 'A';

            if (chosenIndex == q.CorrectIndex)
            {
                quizScore++;
                AddBotMessage($"✅ Correct! {q.Explanation}");
            }
            else
            {
                AddBotMessage($"❌ Incorrect. {q.Explanation}");
            }

            quizIndex++;

            if (quizIndex == 2)
            {
                AddBotMessage("Would you like to learn more about cybersecurity? (yes/no)");
                waitingForTopic = true;
                quizActive = false;
                return;
            }

            if (quizIndex == quizQuestions.Count)
            {
                EndQuiz();
                return;
            }
        }

        private void EndQuiz()
        {
            quizActive = false;
            double percent = (quizScore * 100.0) / quizQuestions.Count;

            string feedback = percent >= 80 ? "🎉 Great job! You're a cybersecurity pro!" :
                              percent >= 50 ? "🙂 Not bad! Keep learning to stay safe online." :
                              "😟 Keep learning to improve your cybersecurity knowledge.";

            AddBotMessage($"Quiz complete! You scored {quizScore} out of {quizQuestions.Count}. {feedback}");
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
            string ascii = @"╔══════════════════════════════════════════════════════════════════════════════╗
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

📚 Cybersecurity Topics You Can Ask About:
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
20. Email Security";

            TextBlock asciiBlock = new TextBlock
            {
                Text = ascii,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 13.5,
                Foreground = Brushes.LightGray,
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                MaxWidth = 680
            };

            Border border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(20, 20, 20)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                Child = asciiBlock,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 5,
                    ShadowDepth = 2,
                    Opacity = 0.3,
                    Color = Colors.Black
                }
            };

            AddBubbleWithAnimation(border);
        }


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

        private void PlaySendClickSound()
        {
            try
            {
                sendClickPlayer?.Play();
            }
            catch { }
        }
    }
}
