using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionMenu : MonoBehaviour
{
    public ScrollBarController ScrollBar;       // Produciton Menu

    private GameManager _manager;               // Game Manager
    private GameObject _buildingObject;         // Current Building Template
    private BuildingTemplate _building; // BuildingTemplate of _templateOnControl object

    public void InitProductionMenu(GameManager manager){
        _manager = manager;
        ScrollBar.CreateScrollBar(_manager.GameConfig.Pool, this);  // Creating the produciton menu 
    }

    // Creates BuildingTemplate to place
    public void CreateBuildingTemplate(int buildingIndex) {
        // Deselecting buildingSolid selected for information, if there is
        _manager.InformationMenu.DeselectBuilding();
        
        // Destroying buildingTempla selected to place, if there is
        if (_buildingObject)
            Destroy(_buildingObject);

        // Creating a building gameObject
        _buildingObject = Instantiate(_manager.GameConfig.BuildingTemplate, Vector3.back, Quaternion.identity) as GameObject;
        _building = _buildingObject.GetComponent<BuildingTemplate>(); 
        _building.CreateBuilding(buildingIndex, _manager);
        _manager.GameConfig.BuildingOnControl = _building;
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
