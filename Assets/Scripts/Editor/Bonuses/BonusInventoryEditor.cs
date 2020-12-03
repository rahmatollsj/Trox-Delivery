using UnityEditor;
using UnityEngine;
namespace Game{
	// Author: David Pagotto
	[CustomEditor(typeof(BonusInventory))]
	public class BonusInventoryEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var inv = (BonusInventory)target;
			if (EditorApplication.isPlaying)
			{
				EditorGUILayout.HelpBox($"Slot 0: {inv.Slots[0]?.BonusType.ToString()}", MessageType.Info);
				EditorGUILayout.HelpBox($"Slot 1: {inv.Slots[1]?.BonusType.ToString()}", MessageType.Info);
				EditorGUILayout.HelpBox($"Slot 2: {inv.Slots[2]?.BonusType.ToString()}", MessageType.Info);
			}
		}
	}
}