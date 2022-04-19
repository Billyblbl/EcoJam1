using UnityEngine;
using System.Linq;

#nullable enable

public class PolutionTensor : MonoBehaviour {
	public float strength = 0f;
	public float range = 2f;

	public PolutionChunk[]? chunks;

	PolutionChunk[]	ProbeChunks() => Physics.OverlapSphere(transform.position, range)
		.Select(col => col.GetComponent<PolutionChunk>())
		.Where(ch => ch != null)
		.OrderBy(ch => Vector3.Distance(ch.transform.position, transform.position))
		.Take(4)
		.ToArray();

	private void FixedUpdate() {
		chunks = ProbeChunks();
		if (chunks != null) foreach (var chunk in chunks) {
			var polutionIncrease = strength * (range / Vector3.Distance(chunk.transform.position, transform.position)) * Time.deltaTime;
			// if (polutionIncrease > float.Epsilon) Debug.LogFormat("polution increase : {0}", polutionIncrease);
			chunk.polutionLevel += polutionIncrease;
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
