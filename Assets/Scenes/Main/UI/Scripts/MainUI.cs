using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

public class MainUI : UI {
	[Serializable]
	public class TelescopeSettings {
		public RectTransform selection;
		[Label("Index Indicator")] public TextMeshProUGUI indicator;
		public Button prevButton;
		public Button nextButton;
		public Image hud;
		[Range(0, 5)] public int index;
		[Serializable]
		public struct TelescopeAnchor {
			public Telescope telescope;
			public RectTransform anchor;
		}
		[SerializeField] public TelescopeAnchor[] telescopeAnchors;
		public Telescope currentTelescope => telescopeAnchors[index].telescope;

		public int indexRange => telescopeAnchors.Length;
		public int Index {
			get => index;
			set {
				if(value < 0 || value >= indexRange)
					return;
				index = value;
				var segment = telescopeAnchors[index];
				selection.position = segment.anchor.position;
				selection.SetSize(segment.anchor.rect.size);
				hud.sprite = segment.telescope.hudSprite;
				indicator.text = segment.telescope.name;
				FindObjectOfType<AstroManager>().UpdateAstros();
			}
		}
		public void Prev() => --Index;
		public void Next() => ++Index;

		public void Init() {
			prevButton.onClick.AddListener(Prev);
			nextButton.onClick.AddListener(Next);
		}
	}
	public TelescopeSettings telescopeSettings;

	private IEnumerator<object> RefreshSpectrum() {
		yield return new WaitForEndOfFrame();
		yield return telescopeSettings.Index = telescopeSettings.Index;
	}

	[Serializable]
	public class Distance {
		public Slider slider;
		public TextMeshProUGUI indicator;
		[MinMaxSlider(1, 100)] public Vector2Int range;

		public int Value {
			get {
				int value = (int)slider.value;
				if(value <= 30)
					return 300 * value;
				if(value < 60)
					return (int)Mathf.Pow(value / Mathf.Sqrt(2), 4);
				return (int)Mathf.Pow(value - 30, 5);
			}
		}

		public string ValueExpr {
			get {
				return $"{Value} lightyears";
			}
		}

		private void OnSliderValueChange(float value) {
			slider.value = (int)value;
			indicator.text = ValueExpr;
			FindObjectOfType<AstroManager>().UpdateAstros();
		}

		public void Init() {
			slider.onValueChanged.AddListener(OnSliderValueChange);
			slider.wholeNumbers = true;
			slider.minValue = range.x;
			slider.maxValue = range.y;
			slider.value = range.x;
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
		telescopeSettings.Init();
		distanceSettings.Init();
		guidanceSettings.Init();
		bannerSettings.Init();
	}

	public void OnEnable() {
		StartCoroutine(RefreshSpectrum());
	}
}
