using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PoolConfig", menuName = "PoolConfig")]
public class PoolConfig : ScriptableObject {
    private Dictionary<string, Pool> poolDict = new Dictionary<string, Pool>();     // Pool dictionary

    public void InitPool(string cellPrefabName) {             // Initiliazing the pool with given name
        if (poolDict.ContainsKey(cellPrefabName))
                return;
        else {
            GameObject pb = Resources.Load<GameObject>(cellPrefabName);
            poolDict[cellPrefabName] = new Pool(cellPrefabName, pb);
        }
    }

    // Returns an available object from the pool 
    public GameObject GetObjectFromPool(string poolName) {
        GameObject result = null;

        if (poolDict.ContainsKey(poolName)) {
            Pool pool = poolDict[poolName];
            result = pool.NextAvailableObject();
        }

        return result;
    }

    // Return obj to the pool
    public void ReturnObjectToPool(GameObject go) {
        PoolObject po = go.GetComponent<PoolObject>();
        if (po != null) {
            Pool pool = null;
            if (poolDict.TryGetValue(po.poolName, out pool))
                pool.ReturnObjectToPool(po);
        }
    }
}
