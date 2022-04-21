using UnityEngine;
using System.Linq;

#nullable enable

public class PolutionTensor : MonoBehaviour {
	public float strength = 0f;
	public float range = 2f;

	public GameData? gameData;

	public ParticleSystem? suckEffect;
	public float effectStrength = 100f;

	public PolutionChunk[]? chunks;

	PolutionChunk[]	ProbeChunks() => Physics.OverlapSphere(transform.position, range)
		.Select(col => col.GetComponent<PolutionChunk>())
		.Where(ch => ch != null)
		.OrderBy(ch => Vector3.Distance(ch.transform.position, transform.position))
		.Take(4)
		.ToArray();

	public float lastChange = 0f;

	private void FixedUpdate() {
		chunks = ProbeChunks();
		if (chunks != null) foreach (var chunk in chunks) {
			var polutionChange = strength * (range / Vector3.Distance(chunk.transform.position, transform.position)) * Time.fixedDeltaTime;
			// if (polutionIncrease > float.Epsilon) Debug.LogFormat("polution increase : {0}", polutionIncrease);
			chunk.polutionLevel = Mathf.Clamp(chunk.polutionLevel + polutionChange, 0f, float.MaxValue);
			if (gameData != null && polutionChange > 0) gameData.instance.polutionProduced += polutionChange;
			if (gameData != null && polutionChange < 0) gameData.instance.polutionScrubbed -= polutionChange;
			lastChange = polutionChange;
		}
	}

	private void Update() {
		if (suckEffect != null) {
			suckEffect.Emit((int)(Mathf.Abs(lastChange) * effectStrength * Time.deltaTime));
		}
	}

	private void OnValidate() {
		chunks = ProbeChunks();
	}

	private void OnDrawGizmos() {

		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, range);
		if (chunks != null) foreach (var chunk in chunks) {
			Gizmos.DrawLine(transform.position, chunk.transform.position);
		}
	}
}
