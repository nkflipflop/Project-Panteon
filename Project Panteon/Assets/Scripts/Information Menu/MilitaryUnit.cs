using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryUnit : MonoBehaviour
{
    public CircleCollider2D Collider;           // Unit Collider
    public SpriteRenderer Sprite;               // Unit Sprite
    public Color SelectedColor;

    private GameManager _manager;               // Game Manager
    private MilitaryUnitData _militaryUnitData; // Unit information


    public void InitMilitaryUnit(MilitaryUnitData militaryUnitData, GameManager manager) {
        _manager = manager;
        // Adding this as a subscribers to SelectionManager
        _manager.SelectionManager.SelectUnit += SelectUnitSubsc;
        _manager.SelectionManager.SelectUnits += SelectUnitsSubsc; 
        _manager.SelectionManager.Deselect += DeselectMe;

        // Setting military unit information
        _militaryUnitData = militaryUnitData;
        name = _militaryUnitData.name;
        Sprite.color = _militaryUnitData.UnitColor;
    }

    // Selection Listener
    private void SelectUnitSubsc(RaycastHit2D hit, List<MilitaryUnit> selectedUnit) {
        if (hit.collider == Collider) {
            Sprite.color = SelectedColor;
            selectedUnit.Add(this);
        }
    }

    // Selection Listener
    private void SelectUnitsSubsc(Vector2 min, Vector2 max, List<MilitaryUnit> selectedUnits) {
        Vector3 screenPos = _manager.GameCamera.WorldToScreenPoint(transform.position);
        if (screenPos.x > min.x && screenPos.x < max.x) {
            if (screenPos.y > min.y && screenPos.y < max.y ) {
                Sprite.color = SelectedColor;    
                selectedUnits.Add(this);
            }
        }
    }

    // Deselection event Listener
    private void DeselectMe() {
        Sprite.color = _militaryUnitData.UnitColor;
    }
}
