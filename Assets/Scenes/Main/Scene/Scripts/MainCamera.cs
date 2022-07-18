using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCamera : MonoBehaviour {
	bool dragging = false;
	Vector2 lastDrag;
	static int uiLayer;
	new Camera camera;
	RectTransform rt;
	public RectTransform scene;

	public void Start() {
		uiLayer = LayerMask.NameToLayer("UI");
		camera = GetComponent<Camera>();
		rt = transform as RectTransform;
		Vector3 vmin = camera.ViewportToWorldPoint(Vector2.one * -.5f),
			vmax = camera.ViewportToWorldPoint(Vector2.one * .5f),
			diagonal = vmax - vmin;
		Vector2 size = transform.worldToLocalMatrix.MultiplyVector(diagonal);
		rt.SetSize(size);
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

	static float RangeOffset(Vector2 range, float x) {
		return x - Mathf.Clamp(x, range.x, range.y);
	}

	public void OnDrag(Vector2 delta) {
		var from = camera.ScreenToWorldPoint(Vector2.zero);
		var to = camera.ScreenToWorldPoint(delta);
		transform.position += from - to;
		Rect sceneRect = scene.WorldRect(), selfRect = rt.WorldRect();
		Vector2 xRange = new Vector2(sceneRect.xMin, sceneRect.xMax);
		Vector2 yRange = new Vector2(sceneRect.yMin, sceneRect.yMax);
		float xComp = -(RangeOffset(xRange, selfRect.xMin) + RangeOffset(xRange, selfRect.xMax));
		float yComp = -(RangeOffset(yRange, selfRect.yMin) + RangeOffset(yRange, selfRect.yMax));
		transform.position += new Vector3(xComp, yComp, 0);
	}

	public void Update() {
		UpdateDraggingState();
		CheckDrag();
	}
}
