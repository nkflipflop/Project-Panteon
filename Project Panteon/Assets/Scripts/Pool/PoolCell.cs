using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PoolCell : MonoBehaviour
{
    public Text text;
    private int _index;
    private ProductionController _productionController;   

    public void CellIndex(int index, ProductionController productionController) {
        _productionController = productionController;
        _index = index;
        text.text = "Cell " + _index;
    }

    public void SayHello(){
        Debug.Log(text.text);
        _productionController.GenerateBuildingTemplate(_index);
    }
}
