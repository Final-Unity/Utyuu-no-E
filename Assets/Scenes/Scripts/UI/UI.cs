using UnityEngine;

public class UI : MonoBehaviour {
	public static UI currentActive = null;

	[SerializeField] bool active = false;
	public bool Active {
		get => active;
		set {
			if(value) {
				if(currentActive != null)
					currentActive.Active = false;
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
