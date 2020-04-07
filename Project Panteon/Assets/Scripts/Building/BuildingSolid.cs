using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingSolid : BuildingMain 
{
    public Color SelectedColor;

    private Transform _spawnPoint;

    public override void Created() {
        transform.parent = _manager.GameBoard.transform.GetChild(1);
        // Filling the building with solidCells
        _buildingCells = CellHelper.SpawnCells(_buildingData.GetCellMatrix(CellType.Solid), _manager.GameConfig, CellContainer);

        // Coloring the cells with color of selected building
        Deselect();

        // Creating spawnPoint
        if (_buildingData.CanProductUnit)
            CreateSpawnPoint();
    }

    // Created spawnPoint on available space
    private void CreateSpawnPoint() {
        Vector3 pos = _buildingCells[_buildingCells.Capacity - 1].transform.position;
        
        // Spawn position of unit, when created
        pos.x += 2;
        _spawnPoint = new GameObject().transform;
        _spawnPoint.position = pos;
        _spawnPoint.transform.parent = transform;    
        _spawnPoint.name = "Spawn Point";
    }

    // Requests Selection for this building from InformationMenu
    public void CallBaseBuilding() {
        _manager.InformationMenu.RequestSelection(this);
    }

    // When the building is selected to get information
    public BuildingData Selected() {
        // Coloring the cells with selection color
        foreach (var buildingCell in _buildingCells)
            buildingCell.GetComponent<SpriteRenderer>().color = SelectedColor;

        return _buildingData;
    }


    // When the building is deselected
    public void Deselect() {
        // Coloring the cells with color of building
        foreach (var buildingCell in _buildingCells)
            buildingCell.GetComponent<SpriteRenderer>().color = _buildingData.BuildingColor;
    }

    // Spawns new unit on spawnPoint
    public void SpawnUnit(MilitaryUnit militaryUnit, Transform parent) {
        // Positioning on an interval
        Vector3 spawnPosition = _spawnPoint.position;
        spawnPosition.x += Random.Range(-.5f, .5f);
        spawnPosition.y += Random.Range(-.5f, .5f);

        // Creating the unit
        MilitaryUnit unit = Instantiate(militaryUnit, spawnPosition, Quaternion.identity, parent);
        unit.name = militaryUnit.name;
        unit.gameObject.SetActive(true);
    }
}
