using UnityEngine;
#nullable enable

[System.Serializable] public class CubeSphereGrid {

	[Range(1, 30)] public int resolution = 1;
	public Vector3[] vertices;

	public Vector3 VertexAt(Vector3Int coordinates) {
		// classic sphericalisation would just be a normalisation, to have better uniform cells on the 
		// Sphere grid we use the modification from https://catlikecoding.com/unity/tutorials/cube-sphere/
		var v = new Vector3(coordinates.x, coordinates.y, coordinates.z) * 2f / resolution - Vector3.one;
		var squared = Vector3.Scale(v, v);

		return Vector3.Scale(v, new Vector3(
			Mathf.Sqrt(1f - squared.y / 2f - squared.z / 2f + squared.y * squared.z / 3f),
			Mathf.Sqrt(1f - squared.x / 2f - squared.z / 2f + squared.x * squared.z / 3f),
			Mathf.Sqrt(1f - squared.x / 2f - squared.y / 2f + squared.x * squared.y / 3f)
		));
	}
	public Vector3[] GenerateVertices() {
		int cornerVertices = 8;
		int edgeVertices = (resolution * 3 - 3) * 4;
		int faceVertices = (resolution - 1) * (resolution - 1) * 6;

		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

		//TODO mapping from xyz to v
		int v = 0;
		for (int y = 0; y <= resolution; y++) {
			for (int x = 0; x <= resolution; x++) {
				vertices[v++] = VertexAt(new Vector3Int(x, y, 0));
			}
			for (int z = 1; z <= resolution; z++) {
				vertices[v++] = VertexAt(new Vector3Int(resolution, y, z));
			}
			for (int x = resolution - 1; x >= 0; x--) {
				vertices[v++] = VertexAt(new Vector3Int(x, y, resolution));
			}
			for (int z = resolution - 1; z > 0; z--) {
				vertices[v++] = VertexAt(new Vector3Int(0, y, z));
			}
		}

		for (int z = 1; z < resolution; z++) {
			for (int x = 1; x < resolution; x++) {
				vertices[v++] = VertexAt(new Vector3Int(x, resolution, z));
			}
		}
		for (int z = 1; z < resolution; z++) {
			for (int x = 1; x < resolution; x++) {
				vertices[v++] = VertexAt(new Vector3Int(x, 0, z));
			}
		}
		return vertices;
	}

	public CubeSphereGrid(int resolution = 1) {
		this.resolution = resolution;
		this.vertices = GenerateVertices();
	}

	public void DrawColoredVertices(float spheresRadius, Transform? transform = null) {
		var oldColor = Gizmos.color;
		foreach (var vertex in vertices) {
			var color = new Color(vertex.x + .5f, vertex.y + .5f, vertex.z + .5f);
			Gizmos.color = color;
			Gizmos.DrawSphere(transform?.TransformPoint(vertex) ?? vertex, spheresRadius);
		}
		Gizmos.color = oldColor;
	}
}
