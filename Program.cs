using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Media;
using System.Linq;

class CyberSecurityChatbot
{
    // Memory storage for user information
    private static Dictionary<string, string> userMemory = new Dictionary<string, string>();
    private static List<string> userInterests = new List<string>();
    private static Random random = new Random();

    // Sentiment analysis keywords
    private static Dictionary<string, string> sentimentKeywords = new Dictionary<string, string>()
    {
        {"worried", "worried|concerned|anxious|nervous|scared"},
        {"happy", "happy|excited|great|good|awesome"},
        {"frustrated", "frustrated|angry|annoyed|mad|upset"},
        {"confused", "confused|unsure|don't know|not sure"}
    };

    static void Main()
    {
        // Display ASCII Art Logo
        DisplayAsciiArt();

        // Play Greeting Audio (if available)
        PlayGreeting();

        // Greet the user with input validation
        string userName = GetValidatedInput("Hello! What's your name? ",
            input => !string.IsNullOrWhiteSpace(input) && !input.Any(char.IsDigit),
            "Please enter a valid name (no numbers allowed).");

        StoreUserInfo("name", userName);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nCyberBot: Hey {userName}! Great to meet you. I'm your friendly cyber security assistant.");
        Console.WriteLine("You can ask me about phishing, password safety, safe browsing, privacy, scams, and more.");
        Console.WriteLine("Type 'exit' anytime if you need to go.");
        Console.ResetColor();

        // Chatbot Loop
        while (true)
        {
            Console.Write("\nYou: ");
            string userInput = Console.ReadLine()?.ToLower().Trim();

            if (string.IsNullOrEmpty(userInput))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("CyberBot: Hmm... I didn't catch that. Could you try again?");
                Console.ResetColor();
                continue;
            }

            if (userInput == "exit")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nCyberBot: Stay safe online! Have a great day. 👋");
                Console.ResetColor();
                break;
            }

            // Validate input contains at least some letters
            if (userInput.All(c => !char.IsLetter(c)))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("CyberBot: Please enter text with at least some words, not just numbers or symbols.");
                Console.ResetColor();
                continue;
            }

            string response = GetResponse(userInput);
            SimulateTyping(response);
        }
    }

    // Helper method to get validated input
    static string GetValidatedInput(string prompt, Func<string, bool> validationFunc, string errorMessage)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim();

            if (validationFunc(input))
            {
                return input;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ResetColor();
        }
    }

    static void PlayGreeting()
    {
        string relativePath = @"C:\Users\RC_Student_lab\source\repos\CHATBOT console\voice_greeting.wav";

        try
        {
            if (File.Exists(relativePath))
            {
                SoundPlayer player = new SoundPlayer(relativePath);
                player.Load();
                player.PlaySync();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("CyberBot: (No greeting sound found, but I'm still happy to chat!)");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"CyberBot: Oops! Couldn't play the greeting sound. Error: {ex.Message}");
            Console.ResetColor();
        }
    }

    static void DisplayAsciiArt()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(@"
          _____                _____                    _____                    _____                    _____          
         /\    \              |\    \                  /\    \                  /\    \                  /\    \         
        /::\    \             |:\____\                /::\    \                /::\    \                /::\    \        
       /::::\    \            |::|   |               /::::\    \              /::::\    \              /::::\    \       
      /::::::\    \           |::|   |              /::::::\    \            /::::::\    \            /::::::\    \      
     /:::/\:::\    \          |::|   |             /:::/\:::\    \          /:::/\:::\    \          /:::/\:::\    \     
    /:::/  \:::\    \         |::|   |            /:::/__\:::\    \        /:::/__\:::\    \        /:::/__\:::\    \    
   /:::/    \:::\    \        |::|   |           /::::\   \:::\    \      /::::\   \:::\    \      /::::\   \:::\    \   
  /:::/    / \:::\    \       |::|___|______    /::::::\   \:::\    \    /::::::\   \:::\    \    /::::::\   \:::\    \  
 /:::/    /   \:::\    \      /::::::::\    \  /:::/\:::\   \:::\ ___\  /:::/\:::\   \:::\    \  /:::/\:::\   \:::\____\ 
/:::/____/     \:::\____\    /::::::::::\____\/:::/__\:::\   \:::|    |/:::/__\:::\   \:::\____\/:::/  \:::\   \:::|    |
\:::\    \      \::/    /   /:::/~~~~/~~      \:::\   \:::\  /:::|____|\:::\   \:::\   \::/    /\::/   |::::\  /:::|____|
 \:::\    \      \/____/   /:::/    /          \:::\   \:::\/:::/    /  \:::\   \:::\   \/____/  \/____|:::::\/:::/    / 
  \:::\    \              /:::/    /            \:::\   \::::::/    /    \:::\   \:::\    \            |:::::::::/    /  
   \:::\    \            /:::/    /              \:::\   \::::/    /      \:::\   \:::\____\           |::|\::::/    /   
    \:::\    \           \::/    /                \:::\  /:::/    /        \:::\   \::/    /           |::| \::/____/    
     \:::\    \           \/____/                  \:::\/:::/    /          \:::\   \/____/            |::|  ~|          
      \:::\    \                                    \::::::/    /            \:::\    \                |::|   |          
       \:::\____\                                    \::::/    /              \:::\____\               \::|   |          
        \::/    /                                     \::/____/                \::/    /                \:|   |          
         \/____/                                       ~~                       \/____/                  \|___|          
        ");
        Console.ResetColor();
    }

    static string GetResponse(string input)
    {
        // Check for sentiment first
        string sentiment = DetectSentiment(input);

        // Handle memory recall
        if (input.Contains("remember") || input.Contains("recall"))
        {
            return HandleMemoryRecall();
        }

        // Check for specific keywords with multiple response options
        if (input.Contains("phishing"))
        {
            return GetPhishingResponse(sentiment);
        }

        if (input.Contains("password"))
        {
            return GetPasswordResponse(sentiment);
        }

        if (input.Contains("privacy") || input.Contains("private"))
        {
            StoreUserInterest("privacy");
            return GetPrivacyResponse(sentiment);
        }

        if (input.Contains("scam") || input.Contains("fraud"))
        {
            StoreUserInterest("scams");
            return GetScamResponse(sentiment);
        }

        if (input.Contains("wifi") || input.Contains("wi-fi"))
        {
            return GetWifiResponse(sentiment);
        }

        // General responses
        Dictionary<string, string> responses = new Dictionary<string, string>()
        {
            { "how are you", GetHowAreYouResponse(sentiment) },
            { "what's your purpose", "I educate users about cybersecurity risks and how to stay protected." },
            { "what can i ask", "You can ask me about phishing, password safety, safe browsing, privacy, scams, and more!" },
            { "identify phishing", "Signs of phishing: Urgent language, poor grammar, fake email addresses, and unexpected attachments. Stay alert!" },
            { "browse safely", "Always check for HTTPS (lock icon), avoid sketchy websites, and update your browser regularly." },
            { "public wifi", "Public Wi-Fi is risky! Avoid logging into sensitive accounts and use a VPN to protect your data." }
        };

        foreach (var key in responses.Keys)
        {
            if (input.Contains(key))
            {
                return responses[key];
            }
        }

        // If no keyword matches, check if we can personalize based on interests
        if (userInterests.Count > 0)
        {
            string interest = userInterests[random.Next(userInterests.Count)];
            return $"Since you're interested in {interest}, you might want to know that " +
                   GetRelatedTip(interest, sentiment);
        }

        // Unrecognized input - return colorful response
        List<string> unrecognizedResponses = new List<string>()
        {
            "I'm not entirely sure what you mean. Could you ask me about cybersecurity topics?",
            "Hmm, that doesn't sound like a cybersecurity question. Try asking about passwords, phishing, or online safety!",
            "I'm focused on cybersecurity topics. Maybe ask me about how to stay safe online?",
            "That's an interesting thought! I specialize in cybersecurity though - ask me about online safety."
        };

        string unrecognizedResponse = unrecognizedResponses[random.Next(unrecognizedResponses.Count)];

        // Return with different color for unrecognized responses
        return $"[UNRECOGNIZED] {unrecognizedResponse}";
    }

    static string DetectSentiment(string input)
    {
        foreach (var sentiment in sentimentKeywords)
        {
            foreach (var keyword in sentiment.Value.Split('|'))
            {
                if (input.Contains(keyword))
                {
                    return sentiment.Key;
                }
            }
        }
        return "neutral";
    }

    static string GetPhishingResponse(string sentiment)
    {
        StoreUserInterest("phishing");

        List<string> responses = new List<string>()
        {
            "Phishing is a cyber attack where hackers trick you into giving away sensitive info. Watch out for fake emails and suspicious links!",
            "Phishing scams often pretend to be from trusted companies. Always verify the sender's email address before clicking links.",
            "A common phishing tactic is creating urgency ('Your account will be closed!'). Legitimate companies don't pressure you like this."
        };

        string baseResponse = responses[random.Next(responses.Count)];

        return AddSentimentResponse(baseResponse, sentiment, "phishing");
    }

    static string GetPasswordResponse(string sentiment)
    {
        StoreUserInterest("password safety");

        List<string> responses = new List<string>()
        {
            "A strong password has 12+ characters, uses uppercase, lowercase, numbers, and symbols. Never reuse passwords!",
            "Consider using a passphrase (like 'PurpleTurtleJumped42!') instead of a password - longer and easier to remember!",
            "Password managers can generate and store strong passwords for you. This is much safer than using simple memorable passwords."
        };

        string baseResponse = responses[random.Next(responses.Count)];

        return AddSentimentResponse(baseResponse, sentiment, "password safety");
    }

    static string GetPrivacyResponse(string sentiment)
    {
        List<string> responses = new List<string>()
        {
            "Protecting your privacy starts with reviewing app permissions. Only grant access to what's absolutely necessary.",
            "Browser extensions can track your activity. Regularly review and remove ones you don't use or trust.",
            "Social media privacy settings should be checked regularly as platforms often change their policies."
        };

        string baseResponse = responses[random.Next(responses.Count)];

        return AddSentimentResponse(baseResponse, sentiment, "privacy");
    }

    static string GetScamResponse(string sentiment)
    {
        List<string> responses = new List<string>()
        {
            "Scammers often pressure you to act quickly. Remember: legitimate businesses give you time to think.",
            "If an offer seems too good to be true, it probably is. Research before engaging with unexpected opportunities.",
            "Tech support scams are common. No legitimate company will call you out of the blue about computer problems."
        };

        string baseResponse = responses[random.Next(responses.Count)];

        return AddSentimentResponse(baseResponse, sentiment, "scams");
    }

    static string GetWifiResponse(string sentiment)
    {
        List<string> responses = new List<string>()
        {
            "Public Wi-Fi networks can be dangerous. Avoid accessing sensitive accounts unless you're using a VPN.",
            "When using public Wi-Fi, look for networks with official names. Hackers often create fake ones like 'Free Airport WiFi'.",
            "Your phone's mobile hotspot is generally safer than public Wi-Fi when you need to do sensitive transactions."
        };

        string baseResponse = responses[random.Next(responses.Count)];

        return AddSentimentResponse(baseResponse, sentiment, "Wi-Fi safety");
    }

    static string GetHowAreYouResponse(string sentiment)
    {
        if (sentiment == "happy")
        {
            return "I'm glad to hear you're doing well! How can I help you with cybersecurity today?";
        }
        else if (sentiment == "worried")
        {
            return "I'm here to help with your cybersecurity concerns. What's on your mind?";
        }
        else if (sentiment == "frustrated")
        {
            return "I understand cybersecurity can be frustrating sometimes. Let me help make it clearer for you.";
        }

        return "I'm just a bot, but I'm functioning well! How can I help you stay safe online?";
    }

    static string AddSentimentResponse(string baseResponse, string sentiment, string topic)
    {
        if (sentiment == "worried")
        {
            return $"I understand you're concerned about {topic}. {baseResponse} Remember, being aware is the first step to protection!";
        }
        else if (sentiment == "frustrated")
        {
            return $"I know {topic} can be frustrating. {baseResponse} Don't worry, we'll take it one step at a time.";
        }
        else if (sentiment == "confused")
        {
            return $"Let me clarify {topic} for you. {baseResponse} Would you like me to explain anything in more detail?";
        }
        else if (sentiment == "happy")
        {
            return $"Great to see you're enthusiastic about {topic}! {baseResponse}";
        }

        return baseResponse;
    }

    static void StoreUserInfo(string key, string value)
    {
        if (userMemory.ContainsKey(key))
        {
            userMemory[key] = value;
        }
        else
        {
            userMemory.Add(key, value);
        }
    }

    static void StoreUserInterest(string interest)
    {
        if (!userInterests.Contains(interest))
        {
            userInterests.Add(interest);
        }
    }

    static string HandleMemoryRecall()
    {
        if (userMemory.Count == 0 && userInterests.Count == 0)
        {
            return "I don't have anything stored in memory yet. Tell me something about yourself!";
        }

        string response = "I remember that ";
        bool hasInfo = false;

        if (userMemory.ContainsKey("name"))
        {
            response += $"your name is {userMemory["name"]}";
            hasInfo = true;
        }

        if (userInterests.Count > 0)
        {
            if (hasInfo) response += " and ";
            response += "you're interested in " + string.Join(", ", userInterests);
            hasInfo = true;
        }

        response += ". " + GetRelatedTip(userInterests.Count > 0 ? userInterests.Last() : "general", "neutral");

        return response;
    }

    static string GetRelatedTip(string topic, string sentiment)
    {
        switch (topic.ToLower())
        {
            case "phishing":
                return GetPhishingResponse(sentiment);
            case "password safety":
                return GetPasswordResponse(sentiment);
            case "privacy":
                return GetPrivacyResponse(sentiment);
            case "scams":
                return GetScamResponse(sentiment);
            case "wi-fi safety":
                return GetWifiResponse(sentiment);
            default:
                return "Remember to always stay vigilant about your online security!";
        }
    }

    static void SimulateTyping(string message)
    {
        // Check if this is an unrecognized response (starts with [UNRECOGNIZED])
        if (message.StartsWith("[UNRECOGNIZED]"))
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            message = message.Substring("[UNRECOGNIZED] ".Length);
        }
        else if (message.Contains("phishing") || message.Contains("scam") || message.Contains("fraud"))
        {
            Console.ForegroundColor = ConsoleColor.Red; // Warning color for dangerous topics
        }
        else if (message.Contains("password") || message.Contains("privacy"))
        {
            Console.ForegroundColor = ConsoleColor.Blue; // Calm color for security topics
        }
        else if (message.Contains("wifi") || message.Contains("browsing"))
        {
            Console.ForegroundColor = ConsoleColor.Cyan; // Tech color for connectivity topics
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // Default color
        }

        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(25);
        }
        Console.WriteLine();
        Console.ResetColor();
    }
}