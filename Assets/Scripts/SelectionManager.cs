using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;

#nullable enable

public class SelectionManager : MonoBehaviour {

	public PlayerInput? input;
	public Camera? cam;
	public RectTransform? selectionBox;
	[ColorUsage(true, true)] public Color boxColorBorder;
	[ColorUsage(true, true)] public Color boxColorFill;
	public float boxBorderThickness = 1f;
	public float boxBorderRadius = 0f;
	public float dragSelectThreshold;

	public List<Unit>	selection = new();
	public List<Unit>	hovered = new();

	Vector2 mousePosOnSelectDown = Vector3.zero;
	bool selecting = false;
	bool dragging(Vector2 mousePos) => selecting && (mousePos - mousePosOnSelectDown).sqrMagnitude > dragSelectThreshold;
	Rect boxSelectionRect;

	public void GiveOrder(Order order) {
		order.units += selection.Count;
		foreach (var unit in selection) {
			unit.orders.Enqueue(order);
			if (unit.orders.Count == 1) {
				order.StartExecution(unit);
				unit.OnOrder?.Invoke();
			}
		}
	}

	private void Update() {
		var click = input!.actions["Click"].ReadValue<float>() > float.Epsilon;
		var mousePos = Mouse.current.position.ReadValue();
		// Debug.LogFormat("Click = {0}", click);

		boxSelectionRect = RectContaining(mousePosOnSelectDown, mousePos);

		UpdateSelection(
			clickDown: click && !selecting,
			clickUp: !click && selecting,
			mousePos: mousePos,
			additiveOperation: input!.actions["Additive Operation"].ReadValue<float>() > float.Epsilon
		);

		selecting = click;
	}

	void UpdateSelection(bool clickDown, bool clickUp, Vector2 mousePos, bool additiveOperation) {

		DisableIndicators();

		//Effective selection
		if (clickDown) {
			mousePosOnSelectDown = mousePos;
		} else if (clickUp) {
			selection = ResolveSelection(mousePos, additiveOperation ? selection : null);
			foreach (var unit in selection) unit.OnSelect?.Invoke();
		}

		//Feedback selection
		hovered = ResolveSelection(mousePos);

		EnableIndicators();

	}

	void DisableIndicators() {
		foreach (var unit in selection) unit.selectedIndicator!.enabled = false;
		foreach (var unit in hovered) unit.hoveredIndicator!.enabled = false;
	}

	void EnableIndicators() {
		foreach (var unit in selection) unit.selectedIndicator!.enabled = true;
		foreach (var unit in hovered) unit.hoveredIndicator!.enabled = true;
	}

	bool OverUnit(Vector2 mousePos, out Unit? unit) {
		unit = null;
		return (
			Physics.Raycast(cam!.ScreenPointToRay(mousePos), out var hit) &&
			hit.collider.TryGetComponent<Unit>(out unit)
		);
	}

	List<Unit>	ResolveSelection(Vector2 mousePos, List<Unit>? oldSelection = null) {
		var newSelection = oldSelection ?? new List<Unit>();
		if (dragging(mousePos)) {
			newSelection.AddRange(GetUnitsInBox(boxSelectionRect).Where(unit => !newSelection.Contains(unit)).ToList());
		} else if (Unit.Hover(cam!, mousePos, out var unit)) {
			newSelection.Add(unit!);
		}
		return newSelection;
	}

	Rect RectContaining(Vector2 A, Vector2 B) {
		var min = new Vector2(Mathf.Min(A.x, B.x), Mathf.Min(A.y, B.y));
		var max = new Vector2(Mathf.Max(A.x, B.x), Mathf.Max(A.y, B.y));
		return new Rect(min, max - min);
	}

	List<Unit>	GetUnitsInBox(Rect box) {
		var list = new List<Unit>();
		//TODO factions
		foreach(var unit in Unit.Population) {
			//TODO raycast and filter to keep those we can actually see (not behind something)
			if (box.Contains(cam!.WorldToScreenPoint(unit.transform.position)))
				list.Add(unit);
		}
		return list;
	}

	Vector2 ScreenToGUI(Vector2 vec) => new Vector2(vec.x, Screen.height - vec.y);

	Rect ScreenToGUI(Rect rect) {
		var position = rect.position;
		var size = rect.size;
		position.y += size.y;
		return new Rect(ScreenToGUI(position), size);
	}

	private void OnGUI() {

		if (selecting) {
			var color = GUI.color;
			GUI.color = boxColorFill;
			GUI.DrawTexture(ScreenToGUI(boxSelectionRect), Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0);
			GUI.color = boxColorBorder;
			GUI.DrawTexture(ScreenToGUI(boxSelectionRect), Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, boxColorBorder, boxBorderThickness, boxBorderRadius);
			GUI.color = color;
		}

	}

}
