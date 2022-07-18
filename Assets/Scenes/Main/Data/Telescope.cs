using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Telescope", fileName = "New Telescope.asset")]
public class Telescope : ScriptableObject {
	public new string name;
	public Sprite hudSprite;
}
