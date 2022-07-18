using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageFader : MonoBehaviour {
	[NonSerialized] public Image image;
	Button button;
	[Range(0, 1)] public float targetAlpha = 0;
	public float alphaChangingRate = 1;
	const float alphaThreshold = .2f;

	public void Start() {
		image = GetComponent<Image>();
		button = GetComponent<Button>();
	}

	public void Update() {
		Color color = image.color;
		color.a += (targetAlpha - color.a) * alphaChangingRate * Time.deltaTime;
		image.color = color;
		if(button != null)
			button.enabled = color.a >= alphaThreshold;
	}
}
