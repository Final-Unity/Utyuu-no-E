using System;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

[ExecuteAlways]
public class AstroFilter : MonoBehaviour {
	[Expandable] public Astro astro;
	Image image;
	[NonSerialized] [Range(0, 1)] public float targetAlpha = 1;
	[NonSerialized] public float alphaChangingRate = .1f;

	public void OnEdit() {
		name = astro?.name;
		image = GetComponent<Image>();
		if(image != null) {
			image.sprite = astro?.sprite;
			if(image.sprite != null) {
				Vector2 size = image.sprite.rect.size;
				float ratio = size.y / size.x;
				var fitter = GetComponent<AspectRatioFitter>();
				if(fitter)
					fitter.aspectRatio = ratio;
			}
		}
	}

	void UpdateColor() {
		Color color = image.color;
		color.a += (targetAlpha - color.a) * alphaChangingRate;
		image.color = color;
	}

	public void Update() {
		if(Application.isEditor && !Application.isPlaying) {
			OnEdit();
			return;
		}
		UpdateColor();
	}

	public void Start() {
		if(!Application.isPlaying)
			return;
		OnEdit();
		gameObject.SetActive(false);
	}
}
