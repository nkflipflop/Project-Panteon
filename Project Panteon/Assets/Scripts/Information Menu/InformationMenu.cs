using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationMenu : MonoBehaviour
{
    public Image BuildingImage;     // Image of the selectedBuilding
    public Text BuildingName;       // Name of the selectedBuilding
    public InformationMenuCell InformationMenuCell; // A cell object to create unit production panel 
    public Transform SpawnParent;

    public event Action<int, Transform> SpawnUnit;  // To send spawn request


    [SerializeField]
    protected RectTransform _content;           // Rectangle of Content object of ScrollBar
    private GameManager _manager;               // Gane Manager
    private List<MilitaryUnitData> _productionUnits;// Unit object samples of selected building to spawn
    private BuildingData _buildingData;     // BuildingData on showing 
    
    // Inits the menu
    public void InitInformationMenu(GameManager manager) {
        _manager = manager;
        this.gameObject.SetActive(false);
    }

    // Shows information of the selected building on the menu
    public void ShowBuildingInfo(BuildingData buildingData) {
        // Deselecting previously selected building
        if(_buildingData)
            RemoveInformation();

        // Unhiding the information menu
        this.gameObject.SetActive(true);
        _content.gameObject.SetActive(false);

        // Assigning newly selected building
        _buildingData = buildingData;
        
        // Setting Information Panel
        BuildingName.text = _buildingData.name;
        BuildingImage.sprite = _buildingData.BuildingImage;

        // Listing the units
        if (_buildingData.CanProductUnit)
            ListUnits();
    }

    // Listing productable units on information panel
    private void ListUnits(){  
        // Unhiding the production content
        _content.gameObject.SetActive(true);
        _productionUnits = new List<MilitaryUnitData>();

        // Listing the units
        int unitCount = _buildingData.ProductionUnits.Length;
        for (int i = 0; i < unitCount; i++) {
            var unitData = _buildingData.ProductionUnits[i];
            
            // Listing military units on information bar
            InformationMenuCell cell =  Instantiate(InformationMenuCell, Vector3.zero, Quaternion.identity, _content.transform);
            cell.SetUnitCell(i, unitData.UnitIcon, this);

            // Adding unit data to list
            _productionUnits.Add(unitData);
        }
    }

    // Deselects current selectedBuilding
    public void RemoveInformation() {
        // Hiding Objects
        this.gameObject.SetActive(false);
        _content.gameObject.SetActive(true);

        // Destroying Old data
        foreach (Transform child in _content.transform)
            Destroy(child.gameObject);
    }

    // Sending spawn request to selected building
    public void SendSpawnRequest(int unitIndex) {
        SpawnUnit?.Invoke(unitIndex, SpawnParent);
    }
}
