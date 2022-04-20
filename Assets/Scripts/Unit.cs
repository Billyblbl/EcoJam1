using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

#nullable enable

public class Unit : MonoBehaviour {

	public Renderer? selectedIndicator;
	public Renderer? hoveredIndicator;
	public SphereMovement? movement = null;

	public UnityEvent	OnSelect = new();
	public UnityEvent	OnOrder = new();

	public static List<Unit> Population = new();

	public Queue<Order>	orders = new();
	// public Order? currentOrder;

	public void CancelOrders() {
		if (orders.TryPeek(out var first)) first.StopExecution(this);
		foreach (var order in orders) {
			order.units--;
		}
		orders.Clear();
	}

	private void OnEnable() {
		Population.Add(this);
	}

	private void OnDisable() {
		Population.Remove(this);
	}

	public static bool Hover(Camera cam, Vector2 mousePos, out Unit? unit) {
		unit = null;
		return (
			Physics.Raycast(cam.ScreenPointToRay(mousePos), out var hit) &&
			hit.collider.TryGetComponent<Unit>(out unit)
		);
	}

	private void Update() {

		if (orders.TryPeek(out var currentOrder) && currentOrder.UpdateExecution(this)) {
			currentOrder.StopExecution(this);
			orders.Dequeue().units--;
			if (orders.TryPeek(out currentOrder)) currentOrder.StartExecution(this);
		}
	}

}
