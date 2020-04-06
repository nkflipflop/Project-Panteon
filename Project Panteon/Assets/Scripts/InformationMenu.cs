using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationMenu : MonoBehaviour
{
    public Image BuildingImage; 
    public Text BuildingName;       // Header of UI

    private GameManager _manager;   // Gane Manager
    private BuildingSolid _selectedBuilding;
    
    // Inits the menu
    public void InitInformationMenu(GameManager manager) {
        _manager = manager;
        this.gameObject.SetActive(false);
    }

    public void RequestSelection(BuildingSolid building) {
        if (!_manager.GameConfig.BuildingOnControl)
            SelectBuilding(building);
    }

    private void SelectBuilding(BuildingSolid building) {
        this.gameObject.SetActive(true);
        if(_selectedBuilding)
            _selectedBuilding.Deselect();

        _selectedBuilding = building;
        _selectedBuilding.Selected();
        BuildingData buildingData = _manager.GameConfig.Buildings[_selectedBuilding.BuildingIndex];
        BuildingName.text = buildingData.name;
        BuildingImage.sprite = buildingData.BuildingImage;

        //MilitaryUnit soldier = Instantiate(_manager.GameConfig.MilitaryUnit, Vector3.zero, Quaternion.identity);
        // soldier.ProduceUnit(buildingData.ProductionUnits[0]);
        //foreach(var unit in buildingData.ProductionUnits)
    }

    public void DeselectBuilding() {
        this.gameObject.SetActive(false);
        if(_selectedBuilding)
            _selectedBuilding.Deselect();
        _selectedBuilding = null;

    }
}
