using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    public Transform CellContainer;     // All Cell objects of the building

    private GameConfigData _config;
    private List<Cell> BuildingCells;   // All Cells of the building 
    private BuildingData _buildingData; // Building information 

    public void CreateBuilding(BuildingData buildingData, GameConfigData config, Camera camera, GameBoard gameBoard) {

        _config = config;
        _buildingData = buildingData;

        BuildingCells = CellHelper.SpawnCells(_buildingData.GetCellMatrix(CellType.Solid), _config, CellContainer);

        foreach (var buildingCell in BuildingCells){
            buildingCell.GetComponent<SpriteRenderer>().color = _buildingData.colr;
        }
    }
}
