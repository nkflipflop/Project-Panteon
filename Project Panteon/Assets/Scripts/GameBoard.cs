using UnityEngine;

public class GameBoard : MonoBehaviour 
{
    public int[,] GridContent;      // Content of each cell
    public Vector2 Dimensions 
    {   
        get { return new Vector2(_gridWidth, _gridHeight);}
    }

    private GameManager _manager;   // Game Manager        
    private int _gridWidth;         // Columns of the grid
    private int _gridHeight;        // Rows of the grid


    // Inits the Game Board
    public void InitGameBoard(GameManager manager) {
        _manager = manager;

        // Creating Grid system
        _gridHeight = _manager.GameConfig.MapGridHeight;
        _gridWidth = _manager.GameConfig.MapGridWidth;
        GridContent = new int[_gridWidth, _gridHeight];

        // Filling the grid with blank cell    
        // If the index is 0, soldiers can walk
        for (var y = 0; y < _gridHeight; y++) {
            for (var x = 0; x < _gridWidth; x++)
                GridContent[x, y] = 1;
        }
        
        //var cells = CellHelper.SpawnCells(Dimensions, _manager.GameConfig.Cell, Grid);
    }
}
