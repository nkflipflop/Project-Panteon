using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Camera GameCamera;
    public GameConfigData Config;
    public MapGrid MapGrid;
    public ProductionMenu ProductionMenu;

    private void Start() {
        MapGrid.CreateGrid();
        ProductionMenu.CreateProductionMenu();
    }
}
