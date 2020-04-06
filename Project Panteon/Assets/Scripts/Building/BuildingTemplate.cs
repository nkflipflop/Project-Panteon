using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Building Template to place a new building
public class BuildingTemplate : BuildingMain 
{
    public bool onBoard = false;    // True, when mouse on gameBoard

    private bool _canPlace;         // True, when the building can place
    
    
    public override void Created() {
        // Filling the building with tempCells
        _buildingCells = CellHelper.SpawnCells(_buildingData.GetCellMatrix(CellType.Temp), _manager.GameConfig, CellContainer);
    }

    private void Update() {
        MovingBuildingTemplate();
    }

    // Moves the buildingTemplate with mouse position
    private void MovingBuildingTemplate() {
        var pos = _manager.GameCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = transform.position.z;
        transform.position = new Vector3 (Mathf.Round(pos.x), Mathf.Round(pos.y), pos.z);

        // Checking for available place
        _canPlace = CheckPlace();

        // When Mouse left-click
        if (Input.GetMouseButtonDown(0)) {
            // If there is no collision on grid with another building
            if (_canPlace && onBoard)     
                CreateBuildingSolid();
        }

        // Destroys thi object, When Mouse right-click
        if (Input.GetMouseButtonDown(1)) {
            Destroy(gameObject);
            _manager.GameConfig.BuildingOnControl = null;
        }
    }
    
    // Checks whether there is colision with another building
    private bool CheckPlace() {
        bool canPlace = true; 
        foreach (var buildingCell in _buildingCells){    // Checking each cell of the building for collision
            RaycastHit2D hit = Physics2D.Raycast(buildingCell.transform.position, Vector3.forward, Mathf.Infinity);
            buildingCell.SetValid();                    
            
            if (hit.collider != null && hit.collider.CompareTag("Solid")) {
                buildingCell.SetInvalid();    
                canPlace = false;
            }
        }
        return canPlace;
    }

    // Places a building into current mouse position
    public void CreateBuildingSolid() {
        Vector3 pos = transform.position;
        pos.z = 0;

        // Creating a building gameObject
        GameObject buildingObject = Instantiate(_manager.GameConfig.BuildingSolid, pos, Quaternion.identity) as GameObject;
        BuildingSolid building = buildingObject.GetComponent<BuildingSolid>(); 
        building.CreateBuilding(_buildingData, _manager);
    }
}
