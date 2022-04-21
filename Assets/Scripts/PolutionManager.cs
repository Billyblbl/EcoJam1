
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif
#nullable enable

public class PolutionManager : MonoBehaviour {

	public GameData?	gameData;

	public PolutionChunk[]?	chunks;

#if UNITY_EDITOR

	public PolutionChunk?	polutionChunk;
	public CubeSphereGrid	mapping = new();
	[Range(.01f, 10)] public float gizmosSize = .1f;

	public bool generate = true;

	IEnumerator GenerateChunks() {
		yield return null;

		if (chunks != null) for (int i = 0; i < chunks.Length; i++) if (chunks[i] != null)
			DestroyImmediate(chunks[i].gameObject);

		chunks = new PolutionChunk[mapping.vertices.Length];
		foreach (var (vertex, i) in mapping.vertices.Select((v, i) => (v, i))) {
			GameObject go = (PrefabUtility.InstantiatePrefab(polutionChunk!.gameObject, transform) as GameObject)!;
			go!.transform.localPosition = vertex;
			go!.transform.up = go!.transform.localPosition.normalized;
			chunks[i] = go!.GetComponent<PolutionChunk>();
		}

		// This stuff is gonna be slow af
		foreach (var (chunk, i) in chunks.Select((c, i) => (c, i))) chunk.neighbours = chunks
			.Where((_, j) => j != i)
			.OrderBy(neighbour => Vector3.Distance(chunk.transform.position, neighbour.transform.position))
			.Take(4)
			.ToArray();
	}

	private void OnValidate() {
		mapping.GenerateVertices();
		if (!generate || polutionChunk == null) return;
		StartCoroutine(GenerateChunks());
	}

	private void OnDrawGizmos () {
		mapping.DrawColoredVertices(gizmosSize, transform);
	}

#endif

	float start;
	private void Start() {
		start = Time.time;
		gameData!.instance = new();
	}

	public AnimationCurve spreadModel = AnimationCurve.Linear(0, 0, 1, 1);
	public float spreadSpeed = 1f;
	public float maxAveragePolution = 50f;

	public UnityEvent	OnPolutionTooHigh = new();

	private void FixedUpdate() {
		foreach (var chunk in chunks!) chunk.Spread((spread) => spreadModel.Evaluate(spread), Time.deltaTime * spreadSpeed);
		var average = chunks!.Average(chunk => chunk.polutionLevel);
		Debug.LogFormat("Health = {0}", maxAveragePolution - average);
		if (average > maxAveragePolution) {
			OnPolutionTooHigh?.Invoke();
			if (gameData != null) gameData!.instance.playTime = Time.time - start;
			SceneManager.LoadScene("GameOver");
		}
	}

}
