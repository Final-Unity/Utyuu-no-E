using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

[ExecuteAlways]
public class AstroManager : MonoBehaviour {
	[ReadOnly] public List<AstroFilter> astros;
	public List<AstroFilter> targets = new List<AstroFilter>();

	public void OnEdit() {
		astros = GetComponentsInChildren<AstroFilter>().ToList();
		targets = astros.Where(f => f.astro.isTarget).ToList();
	}

	public void Update() {
		if(Application.isEditor && !Application.isPlaying)
			OnEdit();
	}

	static float RangeDistance(Vector2 range, float x) {
		return Mathf.Abs(Mathf.Clamp(x, range.x, range.y) - x);
	}

	void UpdateMenuText(MainUI main) {
		if(main == null)
			return;
		var guidance = main.guidanceSettings;
		if(targets.Count != 0)
			guidance.text.text = targets[0].astro.guidance;
		else
			main.EndGame();
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
		UpdateMenuText(FindObjectOfType<MainUI>());
	}

	public void FinishAstro(AstroFilter filter) {
		if(targets.Contains(filter))
			targets.Remove(filter);
		UpdateMenuText(FindObjectOfType<MainUI>());
	}

	public void Start() {
		OnEdit();
		foreach(var filter in astros)
			filter.manager = this;
	}
}
