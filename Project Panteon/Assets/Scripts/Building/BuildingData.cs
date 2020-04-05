using System.Collections;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName= "Building")]
public class BuildingData : ScriptableObject
{
    public string BuildingName;
    public Sprite BuildingSprite;    
    public Color BuildingColor = Color.white;
    public int Rows = 1;
    public int Cols = 1;


    // Fills the CellType matrix with given cellType
    public CellType[,] GetCellMatrix(CellType cellType) {
        var output = new CellType[Cols, Rows];
            
        for (var y = 0; y < Rows; y++) {
            for (var x = 0; x < Cols; x++)
                output[x, y] = cellType;
        }
        return output;
    }
}
