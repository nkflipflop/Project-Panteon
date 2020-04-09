using System.Collections;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName= "Building")]
public class BuildingData : ScriptableObject
{
    public string BuildingName;
    public Sprite BuildingImage;
    public Sprite BuildingIcon;    
    public Color BuildingColor = Color.white;
    
    public bool CanProductUnit = false;
    public MilitaryUnitData[] ProductionUnits;

    public int Rows = 1;
    public int Cols = 1;

    public Vector2 dimensions 
    {
        get { return new Vector2(Cols, Rows); }
    }

}
