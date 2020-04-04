using UnityEngine;
using System.Collections.Generic;

public class PoolObject : MonoBehaviour {}

public class Pool : MonoBehaviour 
{
    public string poolName;                                         // Pool Name
    public GameObject poolCellPrefab;                               // One cell of the pool

    private GameObject _parent;                                     // Root for unused obj
    private Stack<PoolObject> _poolStack = new Stack<PoolObject>(); // Stack for pool

    // Initiliazes the pool with given name
    public void InitPool(Transform parent) {
        _parent = new GameObject(poolName);
        _parent.transform.SetParent(parent);

        // First Push
        PoolObject poolObject = GameObject.Instantiate(poolCellPrefab).AddComponent<PoolObject>();
        PushObject(poolObject);

        // Filling the pool with first object
        for (int i = 0; i < 25; i++) {
            poolObject = GameObject.Instantiate(poolObject);
            PushObject(poolObject);
        }
    }

    // Returns object to the pool
    public void ReturnObjectToPool(Transform go) {
        PoolObject poolObject = go.gameObject.GetComponent<PoolObject>();
        PushObject(poolObject);
    }

    // Pushes object to the pool stack
    public void PushObject(PoolObject poolObject) {
        poolObject.gameObject.SetActive(false);
        poolObject.gameObject.name = "Cell";
        _poolStack.Push(poolObject);
        poolObject.gameObject.transform.SetParent(_parent.transform, false);
    }

    // Pops object from the pool stack
    public GameObject PopObject() {
        GameObject result = _poolStack.Pop().gameObject;
        result.SetActive(true);
        return result;
    }
}
