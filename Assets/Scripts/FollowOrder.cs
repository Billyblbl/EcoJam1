using UnityEngine;

public class FollowOrder : Order {

		private void Start() {
		OnAllComplete.AddListener(order => Destroy(this));
	}

	public override void StartExecution(Unit executor)
	{
		executor.agent.SetDestination(transform.position);
	}

	public override void StopExecution(Unit executor)
	{
		executor.agent.SetDestination(executor.transform.position);
	}

	public override bool UpdateExecution(Unit executor)
	{
		executor.agent.SetDestination(transform.position);
		return false;
	}
}
