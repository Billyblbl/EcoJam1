using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

#nullable enable

public class CameraController : MonoBehaviour {

	public PlayerInput?	input;
	public CinemachineVirtualCamera?	cam;
	public SphereCollider? col;
	[SerializeField] private Vector3	_sensibilities = Vector3.one;
	public Vector3	sensibilities { get => _sensibilities; set {
		_sensibilities = value;
		SetSpeeds(Vector2.Scale(initialMaxSpeeds, sensibilities));
	}}

	CinemachineFramingTransposer?	transposer;
	CinemachinePOV?	pov;
	CinemachineInputProvider?	camInputProvider;
	Vector2 initialMaxSpeeds;

	private void Start() {
		camInputProvider = cam!.GetComponent<CinemachineInputProvider>();
		transposer = cam!.GetCinemachineComponent<CinemachineFramingTransposer>();
		pov = cam!.GetCinemachineComponent<CinemachinePOV>();

		initialMaxSpeeds = new Vector2(
			pov.m_HorizontalAxis.m_MaxSpeed,
			pov.m_VerticalAxis.m_MaxSpeed
		);

		SetSpeeds(Vector2.Scale(initialMaxSpeeds, sensibilities));
	}

	void SetSpeeds(Vector2 speeds) {
		pov!.m_HorizontalAxis.m_MaxSpeed = speeds.x;
		pov!.m_VerticalAxis.m_MaxSpeed = speeds.y;
	}

	private void Update() {
		var panGrab = input!.actions["Pan Grab"].ReadValue<float>() > float.Epsilon;
		camInputProvider!.enabled = panGrab;

		var zoom = input!.actions["Zoom"].ReadValue<float>();
		transposer!.m_CameraDistance = Mathf.Max(transposer!.m_CameraDistance + zoom * sensibilities.z, col!.radius);
	}


	// public Vector3 sensibility = Vector3.one;

	// Vector3 CameraInput { get {
	// 	var panGrab = input!.actions["Pan Grab"].ReadValue<float>() > float.Epsilon;
	// 	var panning = panGrab ? input!.actions["Panning"].ReadValue<Vector2>() : Vector2.zero;
	// 	var zoom = input!.actions["Zoom"].ReadValue<float>();
	// 	return Vector3.Scale(new Vector3(panning.x, panning.y, zoom), sensibility);
	// }}

	// public void MoveCamera(Vector3 movement) {
	// 	transform.Rotate(Vector3.up, movement.x, Space.World);
	// 	transform.Rotate(Vector3.right, movement.y, Space.Self);
	// 	cam!.localPosition += Vector3.forward * movement.z;
	// }

	// private void Update() => MoveCamera(CameraInput);
}
