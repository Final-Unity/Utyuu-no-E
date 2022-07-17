using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCamera : MonoBehaviour {
	bool dragging = false;
	Vector2 lastDrag;
	static int uiLayer;
	new Camera camera;

	public void Start() {
		uiLayer = LayerMask.NameToLayer("UI");
		camera = GetComponent<Camera>();
	}

	static bool PointerOverUI() {
		var es = EventSystem.current;
		if(!es.IsPointerOverGameObject())
			return false;
		var ev = new PointerEventData(es);
		ev.position = Input.mousePosition;
		var res = new List<RaycastResult>();
		es.RaycastAll(ev, res);
		return res.Any(hit => hit.gameObject.layer == uiLayer);
	}

	void UpdateDraggingState() {
		if(Input.GetMouseButtonUp(0))
			dragging = false;
		if(PointerOverUI())
			return;
		if(Input.GetMouseButtonDown(0)) {
			dragging = true;
			lastDrag = Input.mousePosition;
		}
	}

	void CheckDrag() {
		if(!dragging)
			return;
		Vector2 dragDelta = (Vector2)Input.mousePosition - lastDrag;
		SendMessage("OnDrag", dragDelta, SendMessageOptions.DontRequireReceiver);
		lastDrag = Input.mousePosition;
	}

	public void OnDrag(Vector2 delta) {
		var from = camera.ScreenToWorldPoint(Vector2.zero);
		var to = camera.ScreenToWorldPoint(delta);
		transform.position += from - to;
	}

	public void Update() {
		UpdateDraggingState();
		CheckDrag();
	}
}
