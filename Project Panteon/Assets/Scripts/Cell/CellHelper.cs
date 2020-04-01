using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CellHelper {
    public static List<Cell> SpawnCells(CellType[,] cellData, GameConfigData  config, Transform container) {
        var cellList = new List<Cell>();
            
        var width = cellData.GetLength(0);
        var height = cellData.GetLength(1);
            
        var startingPoint = Vector2.zero - new Vector2((width * config.CellSize - 1 * config.CellSize) / 2f,
                                                       (height * config.CellSize - 1 * config.CellSize) / 2f);
        startingPoint = new Vector2 (Mathf.Round(startingPoint.x), Mathf.Round(startingPoint.y));
        
        var prefab = config.GetCellPrefabByType(cellData[0, 0]);
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                if(prefab != null) {
                    var obj = Object.Instantiate(prefab, container) as GameObject;
                    obj.transform.localPosition = startingPoint + new Vector2(x * config.CellSize, y * config.CellSize);

                    var cellComponent = obj.GetComponent<Cell>();
                    cellList.Add(cellComponent);
                }
            }
        }
        return cellList;
    }
}
