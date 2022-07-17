using System;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

[ExecuteAlways]
public class AstroFilter : MonoBehaviour {
	[Expandable] public Astro astro;
	Image image;
	Button button;
	bool clicked = false;

	[NonSerialized] [Range(0, 1)] public float targetAlpha = 0;
	[NonSerialized] public float alphaChangingRate = 1;
	const float alphaThreshold = .5f;

	public void OnEdit() {
		name = astro?.name;
		image = GetComponent<Image>();
		if(image != null) {
			image.sprite = astro?.sprite;
			if(image.sprite != null) {
				Vector2 size = image.sprite.texture.texelSize;
				float ratio = size.y / size.x;
				var fitter = GetComponent<AspectRatioFitter>();
				if(fitter)
					fitter.aspectRatio = ratio;
			}
		}
		(transform as RectTransform).SetSize(new Vector2(0, astro.height));
	}

	public void OnClick() {
		var banner = FindObjectOfType<MainUI>()?.bannerSettings;
		if(banner == null)
			return;
		if(astro.isTarget && !clicked)
			banner.Prompt(astro.banner);
		clicked = true;
		banner.Prompt(astro.description);
	}

	public void Start() {
		if(!Application.isPlaying)
			return;
		OnEdit();
		image = GetComponent<Image>();
		image.color = new Color(1, 1, 1, 0);
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
	}

	void UpdateColor() {
		Color color = image.color;
		color.a += (targetAlpha - color.a) * alphaChangingRate * Time.deltaTime;
		image.color = color;
		button.enabled = color.a >= alphaThreshold;
	}

	public void Update() {
		if(Application.isEditor && !Application.isPlaying) {
			OnEdit();
			return;
		}
		UpdateColor();
	}
}
