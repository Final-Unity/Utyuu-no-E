using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Scriptable Objects/Astro", fileName = "New Astro.asset")]
public class Astro : ScriptableObject {
	public new string name;
	[ShowAssetPreview] public Sprite sprite;
	[Range(50, 500)] public float height;
	public Vector2Int visibleRange;
	public Telescope[] visibleTelescopes;
	[ResizableTextArea] public string description;
	public bool isTarget = false;
	[ShowIf("isTarget")][ResizableTextArea] public string guidance;
	[ShowIf("isTarget")][ResizableTextArea] public string banner;
}
