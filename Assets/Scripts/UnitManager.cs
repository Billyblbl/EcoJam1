using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

#nullable enable

public class UnitManager : MonoBehaviour {
	public SelectionManager?	selectionManager;
	public GameData?	gameData;
	public PlayerInput?	input;
	public Camera? cam;
	public List<Collider> terrain = new();

	public Unit[]	playerUnitTypes = new Unit[1];

	private void Update() {
		var contextOrder = input!.actions["Context Order"].triggered;
		var additiveOperation = input!.actions["Additive Operation"].ReadValue<float>() > float.Epsilon;
		var mousePos = Mouse.current.position.ReadValue();

		if (contextOrder) {
			var hasHit = Physics.Raycast(cam!.ScreenPointToRay(mousePos), out var hit);
			if (!additiveOperation) foreach (var unit in selectionManager!.selection) unit.CancelOrders();
			if (hasHit && selectionManager!.selection.Count > 0 && ResolveContextOrder(hit.point, hit.collider, out var order))
				selectionManager.GiveOrder(order!);
		}
	}

	public bool ResolveContextOrder(Vector3 context, Collider collider, out Order? order) {
		// if terrain, move
		if (terrain.Contains(collider)) {
			var destination = new GameObject("Destination");
			order = destination.AddComponent<MoveOrder>();
			destination.transform.position = context;
			return true;
		}

		// if unit, follow
		else if (collider.TryGetComponent<Unit>(out var unit)) {
			order = unit.gameObject.AddComponent<FollowOrder>();
			return true;
		}

		order = null;
		return false;
	}

	public void SpawnUnit(int index) {
		Physics.Raycast(cam!.ScreenPointToRay(cam!.pixelRect.center), out var hit);
		Instantiate(playerUnitTypes[index], hit.point, Quaternion.identity).transform.parent = transform;
		if (gameData != null) gameData!.instance.unitSpawned++;
	}
}
