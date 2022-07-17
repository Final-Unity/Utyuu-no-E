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
		public Button leftButton;
		public Button rightButton;
		[Range(0, 5)] public int index;

		[Serializable]
		public struct Segment {
			public string name;
			public RectTransform area;
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
				selection.position = segment.area.position;
				selection.sizeDelta = segment.area.sizeDelta;
				indicator.text = segment.name;
			}
		}
		public void Prev() => --Index;
		public void Next() => ++Index;

		public void Init() {
			leftButton.onClick.AddListener(Prev);
			rightButton.onClick.AddListener(Next);
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

	public new void Start() {
		base.Start();
		spectrumSettings.Init();
		distanceSettings.Init();
	}

	public void OnEnable() {
		StartCoroutine(RefreshSpectrum());
	}
}
