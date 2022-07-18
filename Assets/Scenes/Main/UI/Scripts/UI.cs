using UnityEngine;

public static class UIAux {
	public static void SetSize(this RectTransform rt, Vector2 size) {
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
	}

	public static Rect WorldRect(this RectTransform rt) {
		Vector3[] corners = new Vector3[4];
		rt.GetWorldCorners(corners);
		// Get the bottom left corner.
		Vector3 position = corners[0];

		Vector2 size = new Vector2(
			rt.lossyScale.x * rt.rect.size.x,
			rt.lossyScale.y * rt.rect.size.y);

		return new Rect(position, size);
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
