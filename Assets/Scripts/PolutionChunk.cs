using UnityEngine;

#nullable enable

public class PolutionChunk : MonoBehaviour {
	public PolutionChunk[] neighbours = new PolutionChunk[4];
	public ParticleSystem? gfx;
	public float polutionLevel = 0;

}
