using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PoolCell : MonoBehaviour
{
    public GameConfigData GameConfig;   // Game Config

    [SerializeField]
    protected Text text;            // Text on cell
    [SerializeField]
    protected Image image;
    [SerializeField]
    protected Sprite defaultSprite;

    private int _index;             // Index of building
    private ProductionMenu _productionMenu;   

    // Indexing cells
    public void CellIndexing(int index, ProductionMenu productionMenu) {
        _productionMenu = productionMenu;

        int buildingCount = GameConfig.Buildings.Length;
        if (index < buildingCount){
            _index = index;
            BuildingData building = GameConfig.Buildings[_index];
            text.text = building.BuildingName;
            image.sprite = building.BuildingSprite;
        }
        else {
            _index = buildingCount;
            text.text = "";
            image.sprite = defaultSprite;
        }
    }

    // Creates Building Template
    public void GenerateBuildingTemplate(){
        if (text.text != "")
            _productionMenu.GenerateBuildingTemplate(_index);
    }
}
