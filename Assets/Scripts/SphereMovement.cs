using UnityEngine;

#nullable enable

public class SphereMovement : MonoBehaviour {
	public Vector3	destination;
	public float speed = 1f;
	public float rampUpSpeed = 0.1f;
	public float altitude = 51f;
	public float destinationThreshold = float.Epsilon;

	float speedProportion = 0f;
	float ground;

	private void Start() {
		destination = transform.position;
		transform.localPosition = transform.localPosition.normalized * altitude;
		transform.up = transform.localPosition.normalized;
	}

	private void OnValidate() {
		transform.localPosition = transform.localPosition.normalized * altitude;
	}

	public Vector3 elevatedDestination {get => destination.normalized * altitude; }
	public bool moving { get => Vector3.Distance(transform.position, elevatedDestination) > destinationThreshold; }

	private void Update() {

		if (moving) {
			speedProportion = Mathf.Clamp(speedProportion + (moving ? 1 : -1) * rampUpSpeed * Time.deltaTime, 0, 1);

			var axis = Vector3.Cross(transform.localPosition, elevatedDestination);
			var delta = Quaternion.AngleAxis(speed * speedProportion * Time.deltaTime, axis);
			var newLocalPosition = delta * transform.localPosition;
			var velocity = newLocalPosition - transform.localPosition;
			transform.localPosition = newLocalPosition;

			var upward = transform.localPosition.normalized;
			var forward = Vector3.ProjectOnPlane(velocity, upward).normalized;
			transform.rotation = Quaternion.LookRotation(forward, upward);
		}
	}


}
