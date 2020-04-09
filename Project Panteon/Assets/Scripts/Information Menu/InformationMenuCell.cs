using System;
using UnityEngine.UI;
using UnityEngine;

public class InformationMenuCell : MonoBehaviour
{
    public Image UnitIcon;
    
    private InformationMenu _informationMenu;
    private int _unitIndex;

    // Sets the Cell by icon of military unit
    public void SetUnitCell(int unitIndex, Sprite unitIcon, InformationMenu informationMenu) {
        _informationMenu = informationMenu;
        _unitIndex = unitIndex;
        UnitIcon.sprite = unitIcon;
    }

    // Send Spawn Request when pressed the unit button
    public void SpawnRequest() {
        _informationMenu.SendSpawnRequest(_unitIndex);
    }
}
