using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game Config")]
public class GameConfigData : ScriptableObject 
{
        // Grid dimensions
        public int MapGridWidth;
        public int MapGridHeight;
        
        // Cell Prefabs
        public CellTypePrefabPair[] CellPrefabs;    // Which object belongs to which type
        private Dictionary<CellType, GameObject> _cellTypePrefabMap;
        
        // Buildings
        public GameObject BuildingTemplate;     // Template to place selected building
        public GameObject BuildingSolid;        // Building that will be placed
        public BuildingData[] Buildings;        // All distinct buildings on the game

        // Pool
        public Pool pool;                       // Pool of Production Menu

        private void Setup() {
            _cellTypePrefabMap = new Dictionary<CellType, GameObject>();

            foreach (var cellTypePrefabPair in CellPrefabs) {
                if (!_cellTypePrefabMap.ContainsKey(cellTypePrefabPair.CellType))
                    _cellTypePrefabMap.Add(cellTypePrefabPair.CellType, cellTypePrefabPair.GameObject);
            }
        }

        // Returns gameObject according to CellType
        public GameObject GetCellPrefabByType(CellType type) {
            if (_cellTypePrefabMap == null)
                Setup();
            
            return _cellTypePrefabMap[type];
        }
 
        public BuildingData GetBuildingData(int i) {
            return Buildings[i];
        }
    }
