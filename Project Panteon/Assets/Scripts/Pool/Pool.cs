using UnityEngine;
using System.Collections.Generic;

public class PoolObject : MonoBehaviour {
    public string poolName;     // Pool name of this object
}

public class Pool : MonoBehaviour {
    public string poolName;                                         // Pool Name
    public GameObject poolCellPrefab;                               // One cell of the pool

    private GameObject _rootObj;                                    // Root for unused obj
    private Stack<PoolObject> _poolStack = new Stack<PoolObject>(); // Stack for pool

    public void InitPool() {             // Initiliazing the pool with given name
        _rootObj = new GameObject(poolName + " Pool");

        PoolObject po = GameObject.Instantiate(poolCellPrefab).AddComponent<PoolObject>(); 
        po.poolName = poolName;
        PushObject(po);

        // Fill the pool
        for (int i = 0; i < 50; i++) {
            po = GameObject.Instantiate(po);
            PushObject(po);
        }
    }

    // Returns an available object from the pool 
    public GameObject GetObjectFromPool() {
        return PopObject();
    }

    // Return obj to the pool
    public void ReturnObjectToPool(Transform go) {
        PoolObject po = go.gameObject.GetComponent<PoolObject>();
        PushObject(po);
    }

        // Pushes to the pool stack
    public void PushObject(PoolObject po) {
        po.gameObject.SetActive(false);
        po.gameObject.name = "Cell";
        _poolStack.Push(po);
        //add to a root obj
        po.gameObject.transform.SetParent(_rootObj.transform, false);
    }

    // Pops from the pool stack
    public GameObject PopObject() {
        PoolObject po = null;
        po = _poolStack.Pop();

        GameObject result = null;
        result = po.gameObject;
        result.SetActive(true);

        return result;
    }
}
