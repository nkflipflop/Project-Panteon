using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class LoopScrollPrefabSource {
    public PoolConfig resourceManager;
    public string prefabName;
    public int poolSize = 5;

    private bool inited = false;
    public virtual GameObject GetObject() {
        if(!inited) {
            resourceManager.InitPool(prefabName, poolSize);
            inited = true;
        }
        return resourceManager.GetObjectFromPool(prefabName);
    }

    public virtual void ReturnObject(Transform go) {
        resourceManager.ReturnObjectToPool(go.gameObject);
    }

}
