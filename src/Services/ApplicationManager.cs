using sensors.src.UI;

namespace sensors.src.Services
{
    /// <summary>
    /// Main application manager that handles the complete application lifecycle.
    /// Contains all application logic separated from the Main method.
    /// </summary>
    public class ApplicationManager
    {
        private readonly PlayerManager _playerManager;

        public ApplicationManager()
        {
            _playerManager = new PlayerManager();
        }

        /// <summary>
        /// Run the complete application
        /// </summary>
        public void Run()
        {
            // Show welcome screen
            SpectreUI.ShowWelcomeScreen();
            
            // Create player and set up session
            SetupPlayer();
            
            // Show initial player stats
            SpectreUI.ShowPlayerStats(_playerManager);
            
            // Main application loop
            RunMainLoop();
            
            // Application closing message
            ShowClosingMessage();
        }

        /// <summary>
        /// Setup player for the session
        /// </summary>
        private void SetupPlayer()
        {
            string playerName = SpectreUI.GetPlayerName();
            _playerManager.CreatePlayer(playerName);
        }

        /// <summary>
        /// Main application loop - continue until player chooses to quit
        /// </summary>
        private void RunMainLoop()
        {
            bool continueGame = true;
            
            while (continueGame)
            {
                // Create game manager for new investigation
                GameManager gameManager = new(_playerManager);
                continueGame = gameManager.StartInvestigation();
                
                // Ask if player wants to continue
                if (continueGame)
                {
                    continueGame = SpectreUI.ShowContinueGameMenu();
                }
            }
        }

        /// <summary>
        /// Show application closing message
        /// </summary>
        private void ShowClosingMessage()
        {
            Console.WriteLine();
            Console.WriteLine("🎖️ Mission complete. Thanks for your service, Agent!");
            Console.WriteLine("📊 Final statistics saved.");
        }
    }
}
