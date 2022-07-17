using UnityEngine.UI;

public class StartUI : UI {
	public Button startButton;
	public UI main;

	public new void Start() {
		base.Start();
		startButton.onClick.AddListener(() => main.Active = true);
	}
}
