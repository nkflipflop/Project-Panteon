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

        if (_buildingData.CanProductUnit)
            CreateSpawnPoint();
    }

    private void CreateSpawnPoint() {
        Vector3 pos = _buildingCells[_buildingCells.Capacity - 1].transform.position;
        
        // Spawn position of unit, when created
        pos.x += 2;
        GameObject spawnPoint = new GameObject();
        spawnPoint.transform.position = pos;
        spawnPoint.transform.parent = transform;    
        spawnPoint.name = "Spawn Point";

        // Target position to go for unit after spawning
        pos.y -= 4;
        GameObject spawnTarget = Instantiate(spawnPoint, pos, Quaternion.identity, transform);
        spawnTarget.name = "Spawn Target";
    }
    public void CallBaseBuilding() {
        _manager.InformationMenu.RequestSelection(this);
    }

    // When the building is selected to get information
    public void Selected() {
        // Coloring the cells with selection color
        foreach (var buildingCell in _buildingCells)
            buildingCell.GetComponent<SpriteRenderer>().color = SelectedColor;
    }


    // When the building is deselected
    public void Deselect() {
        // Coloring the cells with color of building
        foreach (var buildingCell in _buildingCells)
            buildingCell.GetComponent<SpriteRenderer>().color = _buildingData.BuildingColor;
    }
}
