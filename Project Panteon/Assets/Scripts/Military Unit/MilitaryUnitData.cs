using System.Collections;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Soldier", menuName= "Military Unit")]
public class MilitaryUnitData : ScriptableObject
{
    public string UnitName;
    public Sprite UnitIcon;    
    public Color UnitColor = Color.white;


}