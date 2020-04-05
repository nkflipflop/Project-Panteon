using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

    public GameManager Manager;     // Game Manager
    public Transform Grid;          // All Cells of the grid
        
    private int _gridWidth;             // Columns of the grid
    private int _gridHeight;            // Rows of the grid
    private CellType[,] _gridContent;   // Content of each cell

    // Creates the grid
    public void CreateGrid() {
        _gridHeight = Manager.GameConfig.MapGridHeight;
        _gridWidth = Manager.GameConfig.MapGridWidth;
        _gridContent = new CellType[_gridWidth, _gridHeight];

        // Filling the grid with blank cell    
        for (var y = 0; y < _gridHeight; y++) {
            for (var x = 0; x < _gridWidth; x++)
                _gridContent[x, y] = CellType.Blank;
        }
        
        var cells = CellHelper.SpawnCells(_gridContent, Manager.GameConfig, Grid);
    }
}
