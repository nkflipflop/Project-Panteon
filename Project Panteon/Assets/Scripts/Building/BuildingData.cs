using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName= "Building")]
public class BuildingData : ScriptableObject {
    public int rows = 1;
    public int cols = 1;
    public Color colr = Color.black;

    public CellType[,] GetCellMatrix(CellType cellType) {
        var output = new CellType[cols, rows];
            
        for (var y = 0; y < rows; y++) {
            for (var x = 0; x < cols; x++)
                output[x, y] = cellType;
        }
        return output;
    }
}
