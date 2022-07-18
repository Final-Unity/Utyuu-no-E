public class FinUI : UI {
	ImageFader fader;

	public void OnEnable() {
		fader = GetComponent<ImageFader>();
		fader.Start();
		var color = fader.image.color;
		color.a = 0;
		fader.image.color = color;
		fader.targetAlpha = 1;
	}
}
