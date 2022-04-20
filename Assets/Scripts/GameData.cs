using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] public class Data {
	public float playTime;
	public int factoriesSpawned;
	public int unitSpawned;
	public float polutionProduced;
	public float polutionScrubbed;
}

[CreateAssetMenu(menuName = "EcoJam1/GameData")]
public class GameData : Singleton<Data> {}
