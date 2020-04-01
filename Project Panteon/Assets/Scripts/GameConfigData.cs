using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game Config")]
public class GameConfigData : ScriptableObject {
        //GRID SIZE
        public int MapGridWidth;
        public int MapGridHeight;
        
        //BLOCKS
        public float CellSize;
        
        //PREFABS
        public CellTypePrefabPair[] CellPrefabs;
        private Dictionary<CellType, GameObject> _cellTypePrefabMap;
        
        //PIECES
        public int MaximumBuildings;
        public GameObject BuildingTemplate;
        public GameObject Building;
        public BuildingData[] Buildings;

        private void Setup() {
            _cellTypePrefabMap = new Dictionary<CellType, GameObject>();

            foreach (var cellTypePrefabPair in CellPrefabs) {
                if (!_cellTypePrefabMap.ContainsKey(cellTypePrefabPair.CellType))
                    _cellTypePrefabMap.Add(cellTypePrefabPair.CellType, cellTypePrefabPair.GameObject);
            }
        }

        public GameObject GetCellPrefabByType(CellType type) {
            if (_cellTypePrefabMap == null)
                Setup();
            
            return _cellTypePrefabMap[type];
        }
        
        public BuildingData GetBuildingData(int i) {
            return Buildings[i];
        }
    }
