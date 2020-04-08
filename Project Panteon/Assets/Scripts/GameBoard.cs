using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

    public Transform Grid;          // All Cells of the grid

    private GameManager _manager;     // Game Manager        
    private int _gridWidth;             // Columns of the grid
    private int _gridHeight;            // Rows of the grid
    private CellType[,] _gridContent;   // Content of each cell

    // Inits the Game Board
    public void InitGameBoard(GameManager manager) {
        _manager = manager;

        // Creating Grid system
        _gridHeight = _manager.GameConfig.MapGridHeight;
        _gridWidth = _manager.GameConfig.MapGridWidth;
        _gridContent = new CellType[_gridWidth, _gridHeight];

        // Filling the grid with blank cell    
        for (var y = 0; y < _gridHeight; y++) {
            for (var x = 0; x < _gridWidth; x++)
                _gridContent[x, y] = CellType.Blank;
        }
        
        var cells = CellHelper.SpawnCells(_gridContent, _manager.GameConfig, Grid);
    }
}
