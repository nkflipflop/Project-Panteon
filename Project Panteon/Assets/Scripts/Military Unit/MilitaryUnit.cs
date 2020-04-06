using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryUnit : MonoBehaviour
{
    private MilitaryUnitData _militaryUnitData;

    // Start is called before the first frame update
    public void ProduceUnit(MilitaryUnitData militaryUnitData) {
        _militaryUnitData = militaryUnitData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
