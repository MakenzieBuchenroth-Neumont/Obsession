/*
Copyright 2023 miraclewhips

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;
using UnityEditor;

public class SnapToFloor : MonoBehaviour
{
    [MenuItem("GameObject/Snap To Floor _g", false, 2000)]
		private static void SnapBounds() {
			Snap(true);
		}

    [MenuItem("GameObject/Snap Pivot To Floor #g", false, 2001)]
		private static void SnapPivot() {
			Snap(false);
		}

		private static void Snap(bool useBounds) {
			foreach(var transform in Selection.transforms) {
				RaycastHit hit;
				
				if(Physics.Raycast(transform.position, Vector3.down, out hit)) {
					float offset = 0f;
					string undoName = "Snap Pivot To Floor";

					if(useBounds) {
						offset = FindOffset(transform);
						undoName = "Snap To Floor";
					}

					Undo.RecordObject(transform, undoName);
					transform.position = hit.point + (Vector3.up * offset);
				}
			}
		}

		private static float FindOffset(Transform parent) {
			Bounds bounds = new Bounds(parent.transform.position, Vector3.zero);

			foreach(Renderer renderer in parent.GetComponentsInChildren<Renderer>()) {
				bounds.Encapsulate(renderer.bounds);
			}

			return parent.transform.position.y - (bounds.center.y - bounds.extents.y);
		}
}
