using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Camera GameCamera;
    public GameConfigData Config;
    public GameBoard GameBoard;
    public ProductionMenu ProductionMenu;

    private void Start() {
        GameBoard.CreateGrid();
        ProductionMenu.CreateProductionMenu();
    }
}
