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

	public void ShowAstros() {
		foreach(AstroFilter astro in astros)
			astro.gameObject.SetActive(true);
	}

	public void Start() {
	}
}
