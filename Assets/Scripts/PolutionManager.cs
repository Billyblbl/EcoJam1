
using UnityEngine;
using System.Linq;
using System.Collections;

#nullable enable

public class PolutionManager : MonoBehaviour {

	public PolutionChunk[]?	chunks;

#if UNITY_EDITOR

	public PolutionChunk?	polutionChunk;
	public CubeSphereGrid	mapping = new();
	[Range(.01f, 10)] public float gizmosSize = .1f;

	IEnumerator GenerateChunks() {
		yield return null;

		if (chunks != null) for (int i = 0; i < chunks.Length; i++) if (chunks[i] != null)
			DestroyImmediate(chunks[i].gameObject);

		chunks = new PolutionChunk[mapping.vertices.Length];
		foreach (var (vertex, i) in mapping.vertices.Select((v, i) => (v, i))) {
			var go = Instantiate(polutionChunk!, transform);
			go.transform.localPosition = vertex;
			chunks[i] = go.GetComponent<PolutionChunk>();
		}

		// This stuff is gonna be slow af
		foreach (var (chunk, i) in chunks.Select((c, i) => (c, i))) {
			chunk.neighbours = chunks
				.Where((_, j) => j != i)
				.OrderBy(neighbour => Vector3.Distance(chunk.transform.position, neighbour.transform.position))
				.Take(4)
				.ToArray();
		}
	}

	private void OnValidate() {
		mapping.GenerateVertices();
		if (polutionChunk == null) return;
		StartCoroutine(GenerateChunks());
	}

	private void OnDrawGizmos () {
		mapping.DrawColoredVertices(gizmosSize, transform);
	}

#endif

}
