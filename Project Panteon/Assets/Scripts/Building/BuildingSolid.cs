using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingSolid : MonoBehaviour {
    public Transform CellContainer;     // All Cell objects of the building

    private GameConfigData _gameConfig; // Game Config
    private List<Cell> _buildingCells;  // All Cells of the building 
    private BuildingData _buildingData; // Building information on Matrix form 

    // Creates the placed building 
    public void CreateBuilding(BuildingData buildingData, GameConfigData gameConfig) {
        _gameConfig = gameConfig;
        _buildingData = buildingData;

        // Setting name of objet as building name
        name = _buildingData.name;

        // Filling the building with solid cell
        _buildingCells = CellHelper.SpawnCells(_buildingData.GetCellMatrix(CellType.Solid), _gameConfig, CellContainer);

        // Coloring the cells with color of selected building
        foreach (var buildingCell in _buildingCells)
            buildingCell.GetComponent<SpriteRenderer>().color = _buildingData.BuildingColor;
    }

    public void Selected() {
        // Coloring the cells with color of selected building
        foreach (var buildingCell in _buildingCells)
            buildingCell.GetComponent<SpriteRenderer>().color = Color.black;
    }
}
