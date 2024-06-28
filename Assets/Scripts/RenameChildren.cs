using UnityEngine;

public class RenameChildren : MonoBehaviour {
	void Start() {
		// Loop through all child objects
		foreach (Transform child in transform) {
			// Get the position of the child object
			Vector3 pos = child.position;

			// Append XYZ values to the child object's name
			child.name += "_" + pos.x.ToString("F2") + "_" + pos.y.ToString("F2") + "_" + pos.z.ToString("F2");
		}
	}
}
