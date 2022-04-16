using UnityEngine;

#nullable enable

public class FollowOrder : Order {

	private void Start() {
		OnAllComplete.AddListener(order => Destroy(this));
	}

	public override void StartExecution(Unit executor)
	{
		if (executor.movement) executor.movement!.destination = transform.position;
	}

	public override void StopExecution(Unit executor)
	{
		if (executor.movement) executor.movement!.destination = executor.transform.position;
	}

	public override bool UpdateExecution(Unit executor)
	{
		if (executor.movement) executor.movement!.destination = transform.position;
		// Debug.LogFormat("{0} Following {1}", executor.gameObject.name, gameObject.name);
		return false;
	}
}
