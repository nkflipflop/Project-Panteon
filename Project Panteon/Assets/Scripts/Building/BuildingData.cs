using System.Collections;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName= "Building")]
public class BuildingData : ScriptableObject
{
    public Sprite sprite;    
    public string buildingName;
    public int rows = 1;
    public int cols = 1;
    public Color buildingColor = Color.white;

    // Fills the CellType matrix with given cellType
    public CellType[,] GetCellMatrix(CellType cellType) {
        var output = new CellType[cols, rows];
            
        for (var y = 0; y < rows; y++) {
            for (var x = 0; x < cols; x++)
                output[x, y] = cellType;
        }
        return output;
    }
}
