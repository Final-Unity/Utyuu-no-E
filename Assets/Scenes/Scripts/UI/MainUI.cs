using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

public class MainUI : UI {
	[Serializable]
	public class Spectrum {
		public RectTransform selection;
		[Label("Index Indicator")] public TextMeshProUGUI indicator;
		public Button prevButton;
		public Button nextButton;
		public Image hud;
		[Range(0, 5)] public int index;

		[Serializable]
		public struct Segment {
			public string name;
			public RectTransform anchor;
			public Sprite hud;
		}
		public List<Segment> segments;

		public int indexRange => segments.Count;
		public int Index {
			get => index;
			set {
				if(value < 0 || value >= indexRange)
					return;
				index = value;
				var segment = segments[index];
				selection.position = segment.anchor.position;
				selection.SetSize(segment.anchor.rect.size);
				hud.sprite = segment.hud;
				indicator.text = segment.name;
			}
		}
		public void Prev() => --Index;
		public void Next() => ++Index;

		public void Init() {
			prevButton.onClick.AddListener(Prev);
			nextButton.onClick.AddListener(Next);
		}
	}
	public Spectrum spectrumSettings;

	private IEnumerator<object> RefreshSpectrum() {
		yield return new WaitForEndOfFrame();
		yield return spectrumSettings.Index = spectrumSettings.Index;
	}

	[Serializable]
	public class Distance {
		public Slider slider;
		public TextMeshProUGUI indicator;

		public string ParseIndicator(int value) {
			return value.ToString();
		}

		public int Value {
			get => (int)slider.value;
			set {
				value = (int)(slider.value = value);
				indicator.text = ParseIndicator(value);
			}
		}
		public void SetValue<T>(T value) where T : unmanaged => Value = Convert.ToInt32(value);

		public void Init() {
			slider.onValueChanged.AddListener(SetValue);
		}
	}
	public Distance distanceSettings;

	[Serializable]
	public class Guidance {
		public Button button;
		public RectTransform box;
		public TextMeshProUGUI text;

		public bool Visibility {
			get => box.gameObject.activeSelf;
			set => box.gameObject.SetActive(value);
		}

		public void Prompt(string value) {
			text.text = value;
			Visibility = true;
		}

		public void Init() {
			Visibility = false;
			button.onClick.AddListener(() => Visibility ^= true);
		}
	}
	public Guidance guidanceSettings;


	[Serializable]
	public class Banner {
		public Button banner;
		public TextMeshProUGUI text;

		public bool Visibility {
			get => banner.gameObject.activeSelf;
			set => banner.gameObject.SetActive(value);
		}

		public void Prompt(string value) {
			text.text = value;
			Visibility = true;
		}

		public void Init() {
			Visibility = false;
			banner.onClick.AddListener(() => Visibility ^= true);
		}
	}
	public Banner bannerSettings;

	public new void Start() {
		base.Start();
		spectrumSettings.Init();
		distanceSettings.Init();
		guidanceSettings.Init();
		bannerSettings.Init();
	}

	public void OnEnable() {
		StartCoroutine(RefreshSpectrum());
	}
}
