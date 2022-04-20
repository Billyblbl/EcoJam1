using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

#nullable enable

public class MenuManager : MonoBehaviour {

	public GameObject?	pauseMenu;
	public PlayerInput? input;

	private void Update() {
		if (input!.actions["Pause"].triggered) TogglePause();
	}

	public void TogglePause() {
		if (pauseMenu!.activeSelf) Resume();
		else Pause();
	}

	public void Pause() {
		Time.timeScale = 0f;
		pauseMenu?.SetActive(true);
	}

	public void Resume() {
		Time.timeScale = 1f;
		pauseMenu?.SetActive(false);
	}

	public void Restart() {
		Time.timeScale = 1f;
		SceneManager.LoadScene("PlanetScene");
	}

	public void GoToMainMenu() {
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
	}

	public void Quit() {
		Application.Quit();
	}
}
