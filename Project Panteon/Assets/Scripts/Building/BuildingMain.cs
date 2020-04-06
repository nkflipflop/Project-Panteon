using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMain : MonoBehaviour
{
    public Transform CellContainer;         // All Cell objects of the building

    protected GameManager _manager;         // Game Manager
    protected List<Cell> _buildingCells;    // All Cells of the building 
    protected BuildingData _buildingData;   // Building information on Matrix form 


    public void CreateBuilding(BuildingData buildingData, GameManager manager) {
        _manager = manager;
        _buildingData = buildingData;
        // Setting name of objet as building name
        name = _buildingData.name;
        Created();
    }

    public virtual void Created(){}
}
