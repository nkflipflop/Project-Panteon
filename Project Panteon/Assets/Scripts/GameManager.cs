using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Camera GameCamera;               // Main Camera
    public GameConfigData Config;           // Game Config
    public GameBoard GameBoard;             // Game Board
    public ScrollBarController scrollBar;   // Produciton Menu

    private void Start() {
        GameBoard.CreateGrid();                 // Creating the game board
        scrollBar.CreateScrollBar(Config.pool);  // Creating the produciton menu 
    }
}
