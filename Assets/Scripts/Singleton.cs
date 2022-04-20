using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

// [CreateAssetMenu(menuName = "EcoJam1/GameData")]
public class Singleton<T> : ScriptableObject where T : new() {
	T? _instance;
	public T instance { get {
		if (_instance == null) _instance = new();
		return _instance!;
	} set => _instance = value; }
}
