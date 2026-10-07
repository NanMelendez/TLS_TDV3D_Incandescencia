using UnityEngine;
using UnityEngine.AI;

public class ColRecipientManager : MonoBehaviour
{
	[SerializeField] private GameObject pieceInstance;
	[SerializeField] [Min(0.1f)] private float spawnRadius = 50.0f;
	[SerializeField] private Transform instanceParent;

	private Vector3 navMeshMinBounds;
	private Vector3 navMeshMaxBounds;

	public static ColRecipientManager Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;

		NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
		foreach (Vector3 vertex in navMeshData.vertices)
		{
			navMeshMinBounds = Vector3.Min(navMeshMinBounds, vertex);
			navMeshMaxBounds = Vector3.Max(navMeshMaxBounds, vertex);
		}
	}

	public void SpawnPieces(int n)
	{
		NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

		if (triangulation.vertices == null || triangulation.vertices.Length == 0)
			return;

		int spawnedCount = 0;

		while (spawnedCount < n)
		{
			float randomX = Random.Range(navMeshMinBounds.x, navMeshMaxBounds.x);
			float randomZ = Random.Range(navMeshMinBounds.z, navMeshMaxBounds.z);
			
			float spawnY = navMeshMaxBounds.y + 2.0f;
			Vector3 randomPoint = new Vector3(randomX, spawnY, randomZ);

			float searchRadius = (navMeshMaxBounds.y - navMeshMinBounds.y) + 5.0f;

			if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, searchRadius, NavMesh.AllAreas))
			{
				Vector3 final = hit.position + Vector3.up * 0.5f;
				Instantiate(pieceInstance, final, Quaternion.identity, instanceParent);
				spawnedCount++;
			}
		}
	}
}
