using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionMenu : MonoBehaviour
{
    public ScrollBarController scrollBar;       // Produciton Menu

    private GameManager _Manager;               // Game Manager
    private GameObject _buildingObject;         // Current Building Template
    private BuildingTemplate _building; // BuildingTemplate of _templateOnControl object

    public void InitProductionMenu(GameManager Manager){
        _Manager = Manager;
        scrollBar.CreateScrollBar(_Manager.GameConfig.Pool, this);  // Creating the produciton menu 
    }

    // Creates BuildingTemplate to place
    public void CreateBuildingTemplate(int buildingIndex) {
        if (_buildingObject)
            Destroy(_buildingObject);

        // Creating a building gameObject
        _buildingObject = Instantiate(_Manager.GameConfig.BuildingTemplate, Vector3.back, Quaternion.identity) as GameObject;
        _building = _buildingObject.GetComponent<BuildingTemplate>();
        _building.CreateBuilding(_Manager.GameConfig.Buildings[buildingIndex], _Manager);
        _Manager.GameConfig.BuildingOnControl = _building;
    }

    // Enables placig action, when mouse is in gameBoard
    public void OnBoard(){
        if (_building) _building.onBoard = true;
    }

    // Avoids placig action, when mouse is in gameBoard
    public void OnHUD(){
        if (_building) _building.onBoard = false;
    }
}
