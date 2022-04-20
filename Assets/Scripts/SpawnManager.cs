using UnityEngine;
using System.Linq;

#nullable enable

public class SpawnManager : MonoBehaviour {

	public GameData?	gameData;
	[System.Serializable] public struct SpawnTableEntry {
		public Transform? entity;
		public AnimationCurve spawnWeight;
	}

	public SpawnTableEntry[] spawnTable = new SpawnTableEntry[2];
	public float altitude = 51;
	public float timeBetweenSpawns = 10;

	public Transform SpawnRandom() {
		lastSpawn = Time.fixedTime;
		var direction = Random.onUnitSphere;
		var position = direction * altitude;

		var totalWeights = spawnTable.Select(entry => entry.spawnWeight.Evaluate(Time.time)).Aggregate((a, b) => a + b);
		var choice = Random.Range(0, totalWeights);
		var previousEntriesTotal = 0f;
		var entry = spawnTable.First((entry) => (previousEntriesTotal += entry.spawnWeight.Evaluate(Time.time)) >= choice);

		var entity = Instantiate(entry.entity, transform);
		entity!.localPosition = position;
		entity!.up = direction;

		if (gameData != null) gameData!.instance.factoriesSpawned++;

		return entity!;
	}

	float lastSpawn = 0f;
	private void FixedUpdate() {
		if (Time.fixedTime > lastSpawn + timeBetweenSpawns) SpawnRandom();
	}
}
