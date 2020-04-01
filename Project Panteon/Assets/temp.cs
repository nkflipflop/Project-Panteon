using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour {
/*
    [SerializeField]
    private GameObject finalObject;
    private Vector2 mousePos;
    private bool _canBuild = true;
    private SpriteRenderer _renderer;

    private void Start() {
        _renderer = gameObject.GetComponent<SpriteRenderer>();

    }
    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2 (Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));

        if (Input.GetMouseButtonDown(0) && _canBuild) {
            Instantiate(finalObject, transform.position, Quaternion.identity);
        }
    }

	private void OnTriggerStay2D(Collider2D other) {  
		if (other.gameObject.CompareTag("Building")) {
            _canBuild = false;
            _renderer.color = Color.red;
            Debug.Log("enter");
		}
    }

    private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag("Building")) {
            _canBuild = true;
            _renderer.color = Color.white;
            Debug.Log("exit");
		}
    }*/
}
