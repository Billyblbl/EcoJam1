using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

#nullable enable

public class CameraController : MonoBehaviour {

	public PlayerInput?	input;
	public CinemachineVirtualCamera?	cam;
	public Collider? col;
	[SerializeField] private Vector3	_sensibilities = Vector3.one;
	public Vector3	sensibilities { get => _sensibilities; set {
		_sensibilities = value;
		SetSpeeds(Vector2.Scale(initialMaxSpeeds, sensibilities));
	}}

	CinemachineFramingTransposer?	transposer;
	CinemachinePOV?	pov;
	CinemachineInputProvider?	camInputProvider;
	Vector2 initialMaxSpeeds;

	float planetRadius;

	private void Start() {
		camInputProvider = cam!.GetComponent<CinemachineInputProvider>();
		transposer = cam!.GetCinemachineComponent<CinemachineFramingTransposer>();
		pov = cam!.GetCinemachineComponent<CinemachinePOV>();

		initialMaxSpeeds = new Vector2(
			pov.m_HorizontalAxis.m_MaxSpeed,
			pov.m_VerticalAxis.m_MaxSpeed
		);

		SetSpeeds(Vector2.Scale(initialMaxSpeeds, sensibilities));


		Physics.Raycast(cam!.transform.position, -cam!.transform.localPosition.normalized, out var hit);
		planetRadius = hit.point.magnitude;
	}

	void SetSpeeds(Vector2 speeds) {
		pov!.m_HorizontalAxis.m_MaxSpeed = speeds.x;
		pov!.m_VerticalAxis.m_MaxSpeed = speeds.y;
	}

	private void Update() {
		var panGrab = input!.actions["Pan Grab"].ReadValue<float>() > float.Epsilon;
		camInputProvider!.enabled = panGrab;

		var zoom = input!.actions["Zoom"].ReadValue<float>();
		transposer!.m_CameraDistance = Mathf.Max(transposer!.m_CameraDistance + zoom * sensibilities.z, planetRadius);
	}
}
