using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

[ExecuteAlways]
public class AstroManager : MonoBehaviour {
	[ReadOnly] public List<AstroFilter> astros;

	public void OnEdit() {
		astros = GetComponentsInChildren<AstroFilter>().ToList();
	}

	public void Update() {
		if(Application.isEditor && !Application.isPlaying)
			OnEdit();
	}

	static float RangeDistance(Vector2 range, float x) {
		return Mathf.Abs(Mathf.Clamp(x, range.x, range.y) - x);
	}

	public void UpdateAstros() {
		MainUI main = FindObjectOfType<MainUI>();
		if(main == null)
			return;
		Telescope telescope = main.telescopeSettings.currentTelescope;
		foreach(AstroFilter filter in astros) {
			float alpha = 1;
			alpha *= filter.astro.visibleTelescopes.Contains(telescope) ? 1 : 0;
			Vector2 targetRange = filter.astro.visibleRange;
			targetRange.x = Mathf.Log(targetRange.x);
			targetRange.y = Mathf.Log(targetRange.y);
			float targetDistance = Mathf.Log(main.distanceSettings.Value);
			alpha *= Mathf.Exp(-RangeDistance(targetRange, targetDistance));
			filter.targetAlpha = alpha;
		}
	}

	public void Start() {
	}
}
