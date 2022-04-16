using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

#nullable enable

public class Unit : MonoBehaviour {
	public Renderer? selectedIndicator;
	public Renderer? hoveredIndicator;
	public NavMeshAgent? agent;

	public static List<Unit> Population = new();

	public Queue<Order>	orders = new();

	public void CancelOrders() {
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

}
