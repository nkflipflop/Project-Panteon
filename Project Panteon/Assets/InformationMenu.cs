using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationMenu : MonoBehaviour
{
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
        BuildingName.text = _selectedBuilding.name;
    }
}
