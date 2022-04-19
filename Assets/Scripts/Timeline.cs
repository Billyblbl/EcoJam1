using UnityEngine;
using UnityEngine.Events;

#nullable enable

public class Timeline : MonoBehaviour {
	[System.Serializable] public struct Sequence {
		public string name;
		public float duration;
		public UnityEvent onStart;
		public UnityEvent onUpdate;
		public UnityEvent onEnd;
	}

	public Sequence[]	sequences = new Sequence[1];
	public int currentSequence = 0;
	float startTime = 0;

	private void Start() {
		startTime = Time.time;
		sequences[currentSequence].onStart?.Invoke();
	}

	private void Update() {
		if (currentSequence >= sequences.Length) return;

		sequences[currentSequence].onUpdate?.Invoke();
		if (Time.time > startTime + sequences[currentSequence].duration) {
			sequences[currentSequence++].onEnd?.Invoke();
			startTime = Time.time;
			if (currentSequence < sequences.Length) {
				sequences[currentSequence].onStart?.Invoke();
			}
		}
	}



}
