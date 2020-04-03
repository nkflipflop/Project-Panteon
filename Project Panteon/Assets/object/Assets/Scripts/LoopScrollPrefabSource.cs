using UnityEngine;

[System.Serializable]
public class LoopScrollPrefabSource {
    public PoolConfig poolConfig;
    public string prefabName;

    private bool _inited = false;
    public virtual GameObject GetObject() {
        if(!_inited) {
            poolConfig.InitPool(prefabName);
            _inited = true;
        }
        return poolConfig.GetObjectFromPool(prefabName);
    }

    public virtual void ReturnObject(Transform go) {
        poolConfig.ReturnObjectToPool(go.gameObject);
    }

}
