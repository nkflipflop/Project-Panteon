using UnityEngine;

public  class LoopScrollDataSource {
    //LoopScrollDataSource(){}
    
    public void ProvideData(Transform transform, int idx) {
        transform.SendMessage("ScrollCellIndex", idx);
    }

    public static readonly LoopScrollDataSource Instance = new LoopScrollDataSource();

}
