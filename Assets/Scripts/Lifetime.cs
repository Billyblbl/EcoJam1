using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public class Lifetime : MonoBehaviour {
	public float seconds;

	private void OnEnable() {
		// Debug.LogFormat("Destroying {0}", gameObject.name);
		Destroy(gameObject, seconds);
	}
}
