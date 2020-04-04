using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionMenu : MonoBehaviour {
    public GameManager Manager; // Game Manager
    public ScrollBarController scrollBar;

    public void CreateProductionMenu() {
        scrollBar.InitScrollBar(Manager.Config.pool);
        GenerateBuildings();
    }

    private void GenerateBuildings() {
        GameObject buildingObject = Instantiate(Manager.Config.BuildingTemplate, Vector3.back, Quaternion.identity) as GameObject;
        BuildingTemplate building = buildingObject.GetComponent<BuildingTemplate>();
        building.InitializeBuilding(Manager.Config.GetBuildingData(0), Manager.Config, Manager.GameCamera);
    }
}
