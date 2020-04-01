using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingSolid : MonoBehaviour {
    public Transform CellContainer;     // All Cell objects of the building

    private GameConfigData _config;     // Game Config
    private List<Cell> _buildingCells;  // All Cells of the building 
    private BuildingData _buildingData; // Building information on Matrix form 

    // Creates the placed building 
    public void CreateBuilding(BuildingData buildingData, GameConfigData config) {
        _config = config;
        _buildingData = buildingData;
        // Filling the building with solid cell
        _buildingCells = CellHelper.SpawnCells(_buildingData.GetCellMatrix(CellType.Solid), _config, CellContainer);

        // Coloring the cells with color of selected building
        foreach (var buildingCell in _buildingCells){
            buildingCell.GetComponent<SpriteRenderer>().color = _buildingData.colr;
        }
    }
}
