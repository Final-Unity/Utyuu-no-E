using System;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

[ExecuteAlways]
[RequireComponent(typeof(Image), typeof(Button), typeof(ImageFader))]
[RequireComponent(typeof(AudioSource))]
public class AstroFilter : MonoBehaviour {
	public static AudioClip click, clickRight;

	public void Awake() {
		click = Resources.Load<AudioClip>("Clicking Astro");
		clickRight = Resources.Load<AudioClip>("Clicking Right");
	}

	[Expandable] public Astro astro;
	[NonSerialized] public AstroManager manager;
	Image image;
	Button button;
	ImageFader fader;
	AudioSource audioSource;
	public float targetAlpha {
		get => fader.targetAlpha;
		set => fader.targetAlpha = value;
	}
	bool clicked = false;

	public void OnEdit() {
		name = astro?.name;
		image = GetComponent<Image>();
		fader = GetComponent<ImageFader>();
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
		if(astro.isTarget && !clicked) {
			banner.Prompt(astro.banner, true);
			audioSource.PlayOneShot(clickRight);
		}
		else audioSource.PlayOneShot(click);
		clicked = true;
		banner.Prompt(astro.description);
		manager.FinishAstro(this);
	}

	public void Start() {
		if(!Application.isPlaying)
			return;
		OnEdit();
		image = GetComponent<Image>();
		image.color = new Color(1, 1, 1, 0);
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
		fader = GetComponent<ImageFader>();
		audioSource = GetComponent<AudioSource>();
	}

	public void Update() {
		if(Application.isEditor && !Application.isPlaying) {
			OnEdit();
			return;
		}
	}
}
