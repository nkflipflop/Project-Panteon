using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main Building Script
public class BuildingTemplate : MonoBehaviour {
    public Transform CellContainer;     // All Cell objects of the building

    private GameConfigData _config;
    private Camera _camera;         // Main Camera
    private MapGrid _mapGrid;           // MapGrid
    private BuildingData _buildingData; // Building information
    private List<Cell> BuildingCells;   // All Cells of the building     

    private bool HasBuildingPlaced;
    private bool _canPlace;

    // Initializes the building
    public void InitializeBuilding(BuildingData buildingData, GameConfigData config, Camera camera, MapGrid mapGrid) {
        HasBuildingPlaced = false;

        _config = config;
        _camera = camera;
        _mapGrid = mapGrid;
        _buildingData = buildingData;

        BuildingCells = CellHelper.SpawnCells(_buildingData.GetCellMatrix(CellType.Temp), _config, CellContainer);

        foreach (var buildingCell in BuildingCells){
            buildingCell.GetComponent<SpriteRenderer>().color = _buildingData.colr;
        }
    }

    private void Update() {
        if (!HasBuildingPlaced){        // if the building is not be placed
            var pos = _camera.ScreenToWorldPoint(Input.mousePosition);
            pos.z = transform.position.z;
            transform.position = new Vector3 (Mathf.Round(pos.x), Mathf.Round(pos.y), pos.z);

            _canPlace = CheckPlace();   // Checking for available place

            if (Input.GetMouseButtonDown(0)){
                if (_canPlace)
                    CreateBuilding();
            }
        }
    }
    
    // Checks whether there is colision with another building
    private bool CheckPlace() {
        bool canPlace = true;
        foreach (var buildingCell in BuildingCells){
            RaycastHit2D hit = Physics2D.Raycast(buildingCell.transform.position, Vector3.forward, Mathf.Infinity);
            buildingCell.GetComponent<SpriteRenderer>().color = _buildingData.colr; 
            
            if (hit.collider != null && hit.collider.CompareTag("Solid")) {
                buildingCell.GetComponent<SpriteRenderer>().color = Color.red;      
                canPlace = false;
            }
        }
        return canPlace;
    }

    // Places the building into current mouse position
    public void CreateBuilding() {
        Vector3 pos = transform.position;
        pos.z = 0;

        GameObject newBuilding = Instantiate(_config.Building, pos, Quaternion.identity) as GameObject;
        Building building = newBuilding.GetComponent<Building>();
        building.CreateBuilding(_buildingData, _config, _camera, _mapGrid);
    }
}
