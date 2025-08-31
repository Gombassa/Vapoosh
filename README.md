# **Vapoosh\! \- A Unity Game**

Welcome to the project folder for Vapoosh\!, a strategic, educational board game built in Unity, for web, Android and iOS. This document outlines the rules of the game and the basic technical architecture of this prototype.

## **The Game**

Vapoosh\! is a turn-based board game for 1-4 players. The game is played on a 12x12 grid that also serves as a multiplication table. Players roll four dice to determine two possible coordinates on the board where they can place their counter. The goal is to be the first to create a line or square of four of your own counters.

## **Game Rules**

### **Objective**

The primary goal is to create a "four" by placing four of your counters in one of the following patterns:

* A row of four, horizontally.  
* A row of four, vertically.  
* A row of four, diagonally.  
* A 2x2 square.

### **Vapoosh\! (Instant Win)**

If a player rolls four sixes on their turn, this is a **"Vapoosh\!"**. The player wins the game instantly, regardless of the board state.

### **Player Turns**

1. Each turn, a player rolls four 6-sided dice (2 UV Pink, 2 UV Green).  
2. The sum of the two pink dice and the sum of the two green dice create a coordinate pair (e.g., Pink Total \= 5, Green Total \= 8).  
3. The player is given two choices for where to place their counter:  
   * **Option A:** (Pink Sum, Green Sum) \-\> e.g., Column 5, Row 8  
   * **Option B:** (Green Sum, Pink Sum) \-\> e.g., Column 8, Row 5  
4. The player must mentally calculate the multiplication result for their chosen square (e.g., 5 x 8 \= 40\) and click on the correct tile on the board.

### **Special Rules**

* **Capture:** If a player chooses a tile that is already occupied by an *opponent's* counter, the opponent's counter is removed from the board and replaced.  
* **Blocked Move:** If both possible tiles from a dice roll are already occupied by the player's *own* counters, the player cannot make a move. As a result, the next player in the turn order misses their go.

## **Technical Overview**

This project is built using Unity and follows a manager-based architecture.

### **Scene Structure**

The game flow is managed across three separate scenes:

1. **MainMenu**: The entry point of the game. Players can choose to play a multiplayer game (2-4 players) or select the single-player option.  
2. **DifficultyMenu**: Appears after selecting single-player. Players choose between Easy, Medium, or Hard AI opponents.  
3. **GameScene**: The main scene where all gameplay takes place.

### **Core Logic & Scripts**

* **GameSettings.cs**: A static class that acts as a temporary data container. It stores the player's choices from the MainMenu and DifficultyMenu (like number of players and AI difficulty) and carries them over when the GameScene is loaded.  
* **GameManager.cs**: The central "brain" of the game. It operates as a state machine, controlling the game flow, managing whose turn it is, and checking for win conditions. It receives data from other managers to make decisions.  
* **BoardManager.cs**: Responsible for all logic related to the game board itself. This includes procedurally generating the 12x12 grid of tiles, placing and removing counters, and keeping track of which player owns which tile.  
* **UIManager.cs**: Manages all on-screen UI elements. It updates the status text to give players instructions and feedback, and it handles the logic for the in-game "Restart" and "Back to Menu" buttons.  
* **AIController.cs**: Contains the logic for the computer opponent. It receives the possible moves from the GameManager and, based on the selected difficulty, decides which move to make.  
* **PlayerInput.cs**: A simple script that listens for mouse clicks or screen taps from the human player, determines which tile was clicked, and passes that information to the GameManager.

### **How to Run**

1. Open the project in the Unity Editor.  
2. Go to File \> Build Settings.  
3. Drag the MainMenu, DifficultyMenu, and GameScene from the \_Scenes folder into the "Scenes In Build" list.  
4. Ensure MainMenu is at the top of the list (index 0).  
5. Open the MainMenu scene and press the Play button in the editor.