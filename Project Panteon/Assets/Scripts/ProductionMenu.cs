using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionMenu : MonoBehaviour
{
    public ScrollBarController scrollBar;   // Produciton Menu

    private GameManager _Manager;             // Game Manager
    private GameObject _templateObject;  // Current Building Template
    private BuildingTemplate _templateBuilding; // BuildingTemplate of _templateOnControl object

    public void InitProductionMenu(GameManager Manager){
        _Manager = Manager;
        scrollBar.CreateScrollBar(_Manager.GameConfig.Pool, this);  // Creating the produciton menu 
    }

    // Creates BuildingTemplate to place
    public void GenerateBuildingTemplate(int buildingIndex) {
        if (_templateObject)
            Destroy(_templateObject);

        _templateObject = Instantiate(_Manager.GameConfig.BuildingTemplate, Vector3.back, Quaternion.identity) as GameObject;
        _templateBuilding = _templateObject.GetComponent<BuildingTemplate>();
        _templateBuilding.CreateBuildingTemplate(_Manager.GameConfig.Buildings[buildingIndex], _Manager);
    }

    // Enables placig action, when mouse is in gameBoard
    public void OnBoard(){
        if (_templateBuilding) _templateBuilding.onBoard = true;
    }

    // Avoids placig action, when mouse is in gameBoard
    public void OnHUD(){
        if (_templateBuilding) _templateBuilding.onBoard = false;
    }
}
