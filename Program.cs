using sensors.src.Services;
using sensors.src.UI;

// Create player manager and set up player
PlayerManager playerManager = new();
Console.Write("Enter your name: ");
string playerName = Console.ReadLine()?.Trim() ?? "Anonymous";
playerManager.CreatePlayer(playerName);

UserInterface.ShowMessage($"Welcome, {playerName}!");
playerManager.ShowPlayerStats();
UserInterface.WaitForKeyPress();

// Main game loop - continue until player chooses to quit
bool continueGame = true;
while (continueGame)
{
    // Create game manager with player manager dependency
    GameManager gameManager = new(playerManager);
    continueGame = gameManager.StartInvestigation();
}
