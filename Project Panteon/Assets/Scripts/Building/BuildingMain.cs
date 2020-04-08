using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMain : MonoBehaviour
{
    public int BuildingIndex;
    public Transform CellContainer;         // All Cell objects of the building

    protected GameManager _manager;         // Game Manager
    protected List<Cell> _buildingCells;    // All Cells of the building 
    protected BuildingData _buildingData;   // Building information on Matrix form 


    public void CreateBuilding(int buildingIndex, GameManager manager) {
        _manager = manager;
        BuildingIndex = buildingIndex;
        _buildingData = manager.GameConfig.Buildings[BuildingIndex];
        // Setting name of objet as building name
        name = _buildingData.name;
        Created();
    }

    public virtual void Created(){}
}
