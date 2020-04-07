using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryUnit : MonoBehaviour
{
    public SpriteRenderer Sprite;

    private MilitaryUnitData _militaryUnitData;

    public void InitMilitaryUnit(MilitaryUnitData militaryUnitData) {
        _militaryUnitData = militaryUnitData;
        name = militaryUnitData.name;
        Sprite.color = militaryUnitData.UnitColor;
   }
}
