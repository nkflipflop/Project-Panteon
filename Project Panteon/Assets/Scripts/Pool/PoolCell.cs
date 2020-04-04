using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PoolCell : MonoBehaviour
{
    public Text text;
    private PoolObject poolObject;
    void ScrollCellIndex(int idx) {
        string name = "Cell " + idx.ToString();
        text.text = name;
    }
}
