using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public Camera GameCamera;               // Main Camera
    public GameConfigData GameConfig;           // Game Config
    public GameBoard GameBoard;             // Game Board
    public ProductionMenu ProductionMenu;

    private void Start() {
        GameBoard.CreateGrid();                 // Creating the game board
        ProductionMenu.InitProductionMenu(this);
    }
}
