using UnityEngine;

public static class UIAux {
	public static void SetSize(this RectTransform rt, Vector2 size) {
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
	}
}

public class UI : MonoBehaviour {
	public static UI currentActive = null;

	[SerializeField] bool active = false;

	public bool Active {
		get => active;
		set {
			if(value) {
				if(currentActive != null) {
					currentActive.Active = false;
					currentActive = null;
				}
			}
			active = value;
			gameObject.SetActive(active);
			if(active)
				currentActive = this;
		}
	}

	public void Start() {
		gameObject.SetActive(false);
		Active = active;
	}
}
