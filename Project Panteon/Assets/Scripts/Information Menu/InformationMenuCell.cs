using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InformationMenuCell : MonoBehaviour
{
    public Image UnitIcon;
    
    private InformationMenu _informationMenu;
    private int _unitIndex;

    // Sets the Cell by icon of military unit
    public void SetUnitCell(int unitIndex, Sprite unitIcon, InformationMenu informationMenu) {
        _unitIndex = unitIndex;
        UnitIcon.sprite = unitIcon;
        _informationMenu = informationMenu;
    }

    // Send Cloning Request when pressed the unit button
    public void CloneRequest() { 
        _informationMenu.CloneUnit(_unitIndex);
    }

}
