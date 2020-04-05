using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionController : MonoBehaviour
{
    public GameManager Manager;             // Game Manager
    
    private GameObject _templateObject;  // Current Building Template
    private BuildingTemplate _templateBuilding; // BuildingTemplate of _templateOnControl object

    // Creates BuildingTemplate to place
    public void GenerateBuildingTemplate(int buildingIndex) {
        if (_templateObject)
            Destroy(_templateObject);

        _templateObject = Instantiate(Manager.Config.BuildingTemplate, Vector3.back, Quaternion.identity) as GameObject;
        _templateBuilding = _templateObject.GetComponent<BuildingTemplate>();
        _templateBuilding.CreateBuildingTemplate(Manager.Config.GetBuildingData(buildingIndex), Manager.Config, Manager.GameCamera);
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
