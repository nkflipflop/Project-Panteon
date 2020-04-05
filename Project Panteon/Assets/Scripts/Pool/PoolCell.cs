using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PoolCell : MonoBehaviour
{
    public GameConfigData Config;   // Game Config
    [SerializeField]
    protected Text text;               // Text on cell
    [SerializeField]
    protected Image image;
    [SerializeField]
    protected Sprite defaultSprite;

    private int _index;             // Index of building
    private ProductionController _productionController;   

    public void CellIndexing(int index, ProductionController productionController) {
        _productionController = productionController;

        int buildingCount = Config.Buildings.Length;
        if (index < buildingCount){
            _index = index;
            BuildingData building = Config.Buildings[_index];
            text.text = building.buildingName;
            image.sprite = building.sprite;
        }
        else {
            _index = buildingCount;
            text.text = "Null";
            image.sprite = defaultSprite;
        }
    }

    // Creates Building Template
    public void GenerateBuildingTemplate(){
        if (text.text != "Null")
            _productionController.GenerateBuildingTemplate(_index);
    }
}
