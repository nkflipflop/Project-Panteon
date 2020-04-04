using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PoolCell : MonoBehaviour
{
    public Text text;
    private PoolObject poolObject;
    
    void CellIndex(int index) {
        string name = "Cell " + index.ToString();
        text.text = name;
    }
}
