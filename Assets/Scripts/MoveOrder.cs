using UnityEngine;

#nullable enable

public class MoveOrder : Order {

	private void Start() {
		OnAllComplete.AddListener(order => Destroy(gameObject));
	}

	public override void StartExecution(Unit executor) {
		if (executor.movement) executor.movement!.destination = transform.position;
	}

	public override void StopExecution(Unit executor) {
		// Debug.LogFormat("{0} : Stop execution of {1}", gameObject.name, name);
		if (executor.movement) executor.movement!.destination = executor.transform.position;
	}

	public override bool UpdateExecution(Unit executor) {
		return !executor.movement!.moving;
	}
}
