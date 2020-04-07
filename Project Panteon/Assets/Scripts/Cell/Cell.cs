using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cell types for different objects
public enum CellType {
    Blank, Solid, Temp
}

public class Cell : MonoBehaviour 
{
    public Color NoCollisionColor;
    public Color CollisinColor;
    private SpriteRenderer _renderer;

    private void Start() {
        _renderer = transform.GetComponent<SpriteRenderer>();
    }

    // Calls the root (belonging building) of the cell
   private void OnMouseDown() {
        BuildingSolid building = transform.parent.parent.GetComponent<BuildingSolid>();
        building.CallBaseBuilding();
    }
    

    // When there is no collision with cell
    public void SetValid() {
        _renderer.color = NoCollisionColor;
    }

    // When there is a collision with cell
    public void SetInvalid() {
        _renderer.color = CollisinColor;
    }
}
