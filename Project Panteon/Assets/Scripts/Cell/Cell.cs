using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cell types for different objects
public enum CellType {
    Blank, Solid, Temp
}

public class Cell : MonoBehaviour {
    private SpriteRenderer _renderer;
    private Color _defaultColor;

    private void Start() {
        _renderer = transform.GetComponent<SpriteRenderer>();
        _defaultColor = _renderer.color;
    }

    // Calls the root (belonging building) of the cell
    private void OnMouseDown() {   
        BuildingSolid building = transform.parent.parent.GetComponent<BuildingSolid>();
        building.Selected();
    }

    // When there is no collision with cell
    public void SetValid() {
        _renderer.color = _defaultColor;
    }

    // When there is a collision with cell
    public void SetInvalid() {
        _renderer.color = new Color32(255, 100, 100, 200);  
    }


}
