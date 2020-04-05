using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Building Template to place a new building
public class BuildingTemplate : MonoBehaviour {
    public Transform CellContainer;     // All Cell objects of the building
    public bool onBoard = false;

    private Camera _camera;             // Main Camera
    private GameConfigData _config;     // Game Config
    private List<Cell> _buildingCells;  // All Cells of the building    
    private BuildingData _buildingData; // Building information on Matrix form 
    private bool _canPlace;             // True, when the building can place
    
    // Initializes the building
    public void CreateBuildingTemplate(BuildingData buildingData, GameConfigData config, Camera camera) {
        _config = config;
        _camera = camera;
        _buildingData = buildingData;
        _buildingCells = CellHelper.SpawnCells(_buildingData.GetCellMatrix(CellType.Temp), _config, CellContainer);
    }

    private void Update() {
        MovingBuildingTemplate();
    }

    // Moves the buildingTemplate with mouse position
    private void MovingBuildingTemplate() {
        var pos = _camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = transform.position.z;
        transform.position = new Vector3 (Mathf.Round(pos.x), Mathf.Round(pos.y), pos.z);

        _canPlace = CheckPlace();           // Checking for available place

        if (Input.GetMouseButtonDown(0)) {   // When Mouse left-click
            if (_canPlace && onBoard)                  // If there is no collision on grid with another building
                CreateBuilding();
        }
        if (Input.GetMouseButtonDown(1))
            Destroy(gameObject);
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
    public void CreateBuilding() {
        Vector3 pos = transform.position;
        pos.z = 0;

        // Creating a building gameObject
        GameObject newBuilding = Instantiate(_config.BuildingSolid, pos, Quaternion.identity) as GameObject;
        BuildingSolid building = newBuilding.GetComponent<BuildingSolid>();
        building.CreateBuilding(_buildingData, _config);
    }
}
