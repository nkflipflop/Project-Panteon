using UnityEngine;
using System.Collections.Generic;

public class PoolObject : MonoBehaviour {
    public string poolName;
    public bool isPooled;       // True, when the object is waiting in pool
}

class Pool {
    private Stack<PoolObject> availableObjStack = new Stack<PoolObject>();

    private GameObject _rootObj; //the root obj for unused obj
    private string _poolName;

    public Pool(string cellPrefabName, GameObject poolObjectPrefab) {
        _poolName = cellPrefabName;
        _rootObj = new GameObject(cellPrefabName + "Pool");

        PoolObject po = GameObject.Instantiate(poolObjectPrefab).AddComponent<PoolObject>(); 
        po.poolName = cellPrefabName;
        AddObjectToPool(po);

        //populate the pool
        populatePool();
    }

    //o(1)
    private void AddObjectToPool(PoolObject po) {
        //add to pool
        po.gameObject.SetActive(false);
        po.gameObject.name = _poolName;
        availableObjStack.Push(po);
        po.isPooled = true;
        //add to a root obj
        po.gameObject.transform.SetParent(_rootObj.transform, false);
    }

    private void populatePool() {
        for (int i = 0; i < 50; i++) {
            PoolObject po = GameObject.Instantiate(availableObjStack.Peek());
            AddObjectToPool(po);
        }
    }

    public GameObject NextAvailableObject() {
        PoolObject po = null;
        if (availableObjStack.Count > 1)
            po = availableObjStack.Pop();

        GameObject result = null;
        po.isPooled = false;
        result = po.gameObject;
        result.SetActive(true);

        return result;
    }

    public void ReturnObjectToPool(PoolObject po) {
        if (_poolName.Equals(po.poolName))
            AddObjectToPool(po);
    }
}