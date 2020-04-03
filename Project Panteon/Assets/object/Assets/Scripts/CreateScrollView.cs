using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateScrollView : MonoBehaviour {
    public int totalCount = -1;
    void Start() {
        var ls = GetComponent<LoopVerticalScrollRect>();
        ls.totalCount = totalCount;
        ls.RefillCells();
    }
}