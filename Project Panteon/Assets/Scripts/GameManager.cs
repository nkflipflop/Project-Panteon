using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public Camera GameCamera;               // Main Camera
    public GameConfigData GameConfig;       // Game Config
    public GameBoard GameBoard;             // Game Board
    public ProductionMenu ProductionMenu;   // Production Menu
    public InformationMenu InformationMenu; // Information Menu
    public SelectionManager SelectionManager;

    private void Awake() {
        GameBoard.InitGameBoard(this);                  // Initing the game board
        ProductionMenu.InitProductionMenu(this);        // Initing
        SelectionManager.InitSelectionManager(this);    // Initing
        InformationMenu.InitInformationMenu(this);      // Initing

    }
}
