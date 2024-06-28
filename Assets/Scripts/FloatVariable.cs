using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Float")]
public class FloatVariable : ScriptableObject {
	public float value;

	public static implicit operator FloatVariable(float v) {
		throw new NotImplementedException();
	}
}