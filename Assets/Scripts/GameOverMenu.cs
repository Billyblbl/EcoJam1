using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable enable

public class GameOverMenu : MonoBehaviour {

	public GameData?	gameData;

	public TMPro.TextMeshProUGUI? playTimeLabel;
	public TMPro.TextMeshProUGUI? factoriesSpawnedLabel;
	public TMPro.TextMeshProUGUI? unitSpawnedLabel;
	public TMPro.TextMeshProUGUI? polutionScrubbedLabel;

	private void Start() {
		if (playTimeLabel != null) playTimeLabel!.text = string.Format(playTimeLabel!.text, gameData!.instance.playTime);
		if (factoriesSpawnedLabel != null) factoriesSpawnedLabel!.text = string.Format(factoriesSpawnedLabel!.text, gameData!.instance.factoriesSpawned);
		if (unitSpawnedLabel != null) unitSpawnedLabel!.text = string.Format(unitSpawnedLabel!.text, gameData!.instance.unitSpawned);
		if (polutionScrubbedLabel != null) polutionScrubbedLabel!.text = string.Format(polutionScrubbedLabel!.text, gameData!.instance.polutionScrubbed);
	}

	public void Retry() {
		SceneManager.LoadScene("PlanetScene");
	}

	public void GoToMainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	public void Quit() {
		Application.Quit();
	}

}
