using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CellHelper {
    // Spawns cells into the given container in wanted cell type
    public static List<Cell> SpawnCells(CellType[,] cellData, GameConfigData  config, Transform container) {
        var cellList = new List<Cell>();
        
        // Dimensions(columns, rows)  of the wanted object
        var width = cellData.GetLength(0);
        var height = cellData.GetLength(1);
        
        // Position of the first cell
        var startingPoint = Vector2.zero - new Vector2 ((width - 1 ) / 2f, (height - 1 ) / 2f);
        startingPoint = new Vector2 (Mathf.Round(startingPoint.x), Mathf.Round(startingPoint.y));
        
        // GameObject of wanted cellType
        var prefab = config.GetCellPrefabByType(cellData[0, 0]);
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                var obj = Object.Instantiate(prefab, container) as GameObject;
                obj.transform.localPosition = startingPoint + new Vector2(x, y);

                var cellComponent = obj.GetComponent<Cell>();
                cellList.Add(cellComponent);
            }
        }
        return cellList;
    }
}
