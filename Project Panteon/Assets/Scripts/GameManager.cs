using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Camera GameCamera;               // Main Camera
    public GameConfigData Config;           // Game Config
    public GameBoard GameBoard;             // Game Board
    public ProductionMenu ProductionMenu;   // Produciton Menu

    private void Start() {
        GameBoard.CreateGrid();                 // Creating the game board
        ProductionMenu.CreateProductionMenu();  // Creating the produciton menu 
    }
}
