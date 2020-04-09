using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryUnit : MonoBehaviour
{
    public CircleCollider2D Collider;           // Unit Collider
    public SpriteRenderer Sprite;               // Unit Sprite
    public Color SelectedColor;
    public AStarPathfinding aStar;           

    private GameManager _manager;               // Game Manager
    private MilitaryUnitData _militaryUnitData; // Unit information

	[SerializeField] private float _speed = 1f;
	private Vector2 _targetPos;
    private bool _start = false;

	// Update is called once per frame
	void FixedUpdate() {
        if(_start) 
    		Movement();
	}

	private void Movement() {
		/*_distanceBtwTarget = _targetPos - transform.position;

		if (_distanceBtwTarget.magnitude < 0.6f)
			_targetPos = Target.transform.position;*/
	
		if (aStar.Path != null && aStar.Path.Count > 0) {

			if (_targetPos == new Vector2Int(1000, 0) || transform.position == (Vector3) _targetPos) {
				_targetPos = aStar.Path.Pop();
			}
		}else if(aStar.Path.Count == 0)
            _start = false;

		if (_targetPos != new Vector2Int(1000, 0)) 
			transform.position = Vector2.MoveTowards(transform.position, _targetPos, Time.deltaTime * _speed);      // moving the enemy towards to target
	}

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
    private void SelectUnitSubsc(RaycastHit2D hit, List<MilitaryUnit> selectedUnits) {
        if (hit.collider == Collider)
            SelectMe(selectedUnits);
    }

    // Selection Listener
    private void SelectUnitsSubsc(Vector2 min, Vector2 max, List<MilitaryUnit> selectedUnits) {
        Vector3 screenPos = _manager.GameCamera.WorldToScreenPoint(transform.position);
        if (screenPos.x > min.x && screenPos.x < max.x) {
            if (screenPos.y > min.y && screenPos.y < max.y )
                SelectMe(selectedUnits);
        }
    }

    // Selection
    private void SelectMe(List<MilitaryUnit> selectedUnits) {
        _manager.SelectionManager.AStarOrder += StartAStar;
        Sprite.color = SelectedColor;    
        selectedUnits.Add(this);
    }

    // Deselection event Listener
    private void DeselectMe() {
        _manager.SelectionManager.AStarOrder -= StartAStar;
        Sprite.color = _militaryUnitData.UnitColor;
    }

    // AStar Listener
    private void StartAStar(Vector2Int targetPos) {
        aStar.Current = null;
		aStar.StartPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
		aStar.GoalPos = targetPos;
        aStar.Path = null;
		_targetPos = new Vector2Int(1000, 0);        // null value
        aStar.PathFinding(_manager.GameBoard);
		
        _start = true;
    }
}
