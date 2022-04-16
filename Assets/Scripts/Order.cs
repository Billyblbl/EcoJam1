using UnityEngine;
using UnityEngine.Events;

#nullable enable

public abstract class Order : MonoBehaviour {

	[SerializeField] int _units = 0;
	public int units { get => _units; set {
		_units = value;
		if (units == 0) OnAllComplete?.Invoke(this);
	}}

	public UnityEvent<Order>	OnAllComplete = new();

	public abstract void StartExecution(Unit executor);
	public abstract bool UpdateExecution(Unit executor);
	public abstract void StopExecution(Unit executor);

}
