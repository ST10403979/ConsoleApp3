using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Media;

class CyberSecurityChatbot
{
    static void Main()
    {
        // Display ASCII Art Logo
        DisplayAsciiArt();

        // Play Greeting Audio (if available)
        PlayGreeting();

        // Greet the user
        Console.Write("Hello! What's your name? ");
        string userName = Console.ReadLine();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nCyberBot: Hey {userName}! Great to meet you. I'm your friendly cyber security assistant.");
        Console.WriteLine("You can ask me about phishing, password safety, and safe browsing.");
        Console.WriteLine("Type 'exit' anytime if you need to go.");
        Console.ResetColor();

        // Chatbot Loop
        while (true)
        {
            Console.Write("\nYou: ");
            string userInput = Console.ReadLine()?.ToLower().Trim();

            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("CyberBot: Hmm... I didn't catch that. Could you try again?");
                continue;
            }

            if (userInput == "exit")
            {
                Console.WriteLine("\nCyberBot: Stay safe online! Have a great day. 👋");
                break;
            }

            string response = GetResponse(userInput);
            SimulateTyping(response);
        }
    }

    // Function to Play a Greeting Sound (if available)
    static void PlayGreeting()
    {
        string relativePath = @"C:\Users\RC_Student_lab\source\repos\CHATBOT console\voice_greeting.wav";

        try
        {
            if (File.Exists(relativePath))
            {
                SoundPlayer player = new SoundPlayer(relativePath);
                player.Load();
                player.PlaySync(); // Synchronous play
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

    // Function to Display ASCII Art Logo
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

    // Function to Handle Chatbot Responses
    static string GetResponse(string input)
    {
        Dictionary<string, string> responses = new Dictionary<string, string>()
        {
            { "how are you", "I'm just a bot, but I'm feeling great! How can I help you stay safe online?" },
            { "what's your purpose", "I educate users about cybersecurity risks and how to stay protected." },
            { "what can i ask", "You can ask me about phishing, password safety, and safe browsing." },
            { "phishing", "Phishing is a cyber attack where hackers trick you into giving away sensitive info. Watch out for fake emails and suspicious links!" },
            { "identify phishing", "Signs of phishing: Urgent language, poor grammar, fake email addresses, and unexpected attachments. Stay alert!" },
            { "prevent phishing", "Never click on unknown links, verify email senders, use two-factor authentication (2FA), and enable spam filters." },
            { "strong password", "A strong password has 12+ characters, uses uppercase, lowercase, numbers, and symbols. Never reuse passwords!" },
            { "password manager", "A password manager stores your passwords securely so you don’t have to remember them all. Super useful!" },
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

        return "Hmm... I’m not sure about that. Could you ask me something about cybersecurity?";
    }

    // Function to Simulate a Typing Effect
    static void SimulateTyping(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(25);
        }
        Console.WriteLine();
        Console.ResetColor();
    }
}