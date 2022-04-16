using UnityEngine;

#nullable enable

public class FollowOrder : Order {

		private void Start() {
		OnAllComplete.AddListener(order => Destroy(this));
	}

	public override void StartExecution(Unit executor)
	{
		executor.agent?.SetDestination(transform.position);
	}

	public override void StopExecution(Unit executor)
	{
		executor.agent?.SetDestination(executor.transform.position);
	}

	public override bool UpdateExecution(Unit executor)
	{
		executor.agent?.SetDestination(transform.position);
		Debug.LogFormat("{0} Following {1}", executor.gameObject.name, gameObject.name);
		return false;
	}
}
