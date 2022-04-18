using UnityEngine;
using System.Linq;

#nullable enable

public class PolutionChunk : MonoBehaviour {
	public PolutionChunk[] neighbours = new PolutionChunk[4];
	public ParticleSystem? gfx;
	public float polutionLevel = 0f;

	private void Update() {
		gfx!.Emit((int)(polutionLevel * Time.deltaTime * 100f));
	}

	public void Spread(System.Func<float, float> spreadModel, float amount) {
		var spreads = neighbours
			.Select(neighbour => Mathf.Clamp(spreadModel(polutionLevel - neighbour.polutionLevel), 0, float.MaxValue))
			.ToArray();
		var totalSpreads = spreads.Aggregate((a, b) => a+b);
		if (totalSpreads < float.Epsilon) return;
		var proportions = spreads.Select(spread => spread / totalSpreads).ToArray();
		var maxSpread = Mathf.Max(spreads) * amount;

		polutionLevel -= maxSpread;
		foreach (var (neighbour, i) in neighbours.Select((c, i) => (c, i))) {
			neighbour.polutionLevel += maxSpread * proportions[i];
		}
	}

}
