using UnityEngine;

#nullable enable

public class MoveOrder : Order
{
	public float destinationThreshold = float.Epsilon;

	private void Start() {
		OnAllComplete.AddListener(order => Destroy(gameObject));
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
		return Vector3.Distance(transform.position, executor.transform.position) < destinationThreshold;
	}
}
