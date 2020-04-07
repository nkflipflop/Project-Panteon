using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationMenu : MonoBehaviour
{
    public Image BuildingImage;     // Image of the selectedBuilding
    public Text BuildingName;       // Name of the selectedBuilding
    public InformationMenuCell InformationMenuCell; // A cell object to create unit production panel 
    public Transform SpawnedMilitaryUnits;


    [SerializeField]
    protected RectTransform _content;           // Rectangle of Content object of ScrollBar
    private GameManager _manager;               // Gane Manager
    private BuildingSolid _selectedBuilding;    // Building on control 
    private List<MilitaryUnit> _productionUnits;// Unit object samples of selected building to spawn

    
    // Inits the menu
    public void InitInformationMenu(GameManager manager) {
        _manager = manager;
        this.gameObject.SetActive(false);
    }

    // Denies When there is new building placement
    public void RequestSelection(BuildingSolid building) {
        if (!_manager.GameConfig.BuildingOnControl)
            SelectBuilding(building);
    }

    // Selects building to show information on menu
    private void SelectBuilding(BuildingSolid building) {
        // Deselecting previously selected building
        if(_selectedBuilding)
            DeselectBuilding();

        // Unhiding the information menu
        this.gameObject.SetActive(true);
        _content.gameObject.SetActive(false);

        // Assigning newly selected building
        _selectedBuilding = building;

        // Selection adjustments on the buildingSolid
        BuildingData buildingData = _selectedBuilding.Selected();;
        BuildingName.text = buildingData.name;
        BuildingImage.sprite = buildingData.BuildingImage;

        // Listing the units
        if (buildingData.CanProductUnit)
            ListUnits(buildingData);
    }

    private void ListUnits(BuildingData buildingData){  
        // Unhiding the production content
        _content.gameObject.SetActive(true);
        _productionUnits = new List<MilitaryUnit>();

        // Listing the units
        int unitCount = buildingData.ProductionUnits.Length;
        for (int i = 0; i < unitCount; i++) {
            var unitData = buildingData.ProductionUnits[i];
            
            // Listing military units on information bar
            InformationMenuCell cell =  Instantiate(InformationMenuCell, Vector3.zero, Quaternion.identity, _content.transform);
            cell.SetUnitCell(i, unitData.UnitIcon, this);

            // Instantiating temp unit objects to spawn units
            MilitaryUnit tempUnit =  Instantiate(_manager.GameConfig.MilitaryUnit, Vector3.zero, Quaternion.identity, _content.transform);
            tempUnit.InitMilitaryUnit(unitData);
            tempUnit.gameObject.SetActive(false);
            _productionUnits.Add(tempUnit);
        }
    }

    // Spawns the requested MilitaryUnit
    public void CloneUnit(int unitIndex){
        // Spawning on available spawPoint of selectedBuilding
        _selectedBuilding.SpawnUnit(_productionUnits[unitIndex], SpawnedMilitaryUnits);
    }

    // Deselects current selectedBuilding
    public void DeselectBuilding() {
        // Hiding Objects
        this.gameObject.SetActive(false);
        _content.gameObject.SetActive(true);

        // Destroying Old data
        foreach (Transform child in _content.transform)
            Destroy(child.gameObject);

        // Deselection adjustments on the buildingSolid
        if(_selectedBuilding)
            _selectedBuilding.Deselect();

        _selectedBuilding = null;

    }
}
