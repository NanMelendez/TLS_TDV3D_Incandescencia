using UnityEngine;
using UnityEngine.AI;

public class PieceSpreadManager : MonoBehaviour
{
	[SerializeField] private Transform whereToAnchor;

	private Vector3 navMeshMinBounds;
	private Vector3 navMeshMaxBounds;

	public static PieceSpreadManager Instance
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

	public void SpawnPieces(int n, CollectibleType collectibleType, GameObject instance, GameObject fakeInstance = null, int fakeSpawnChance = 10)
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
			Vector3 randomPoint = new(randomX, spawnY, randomZ);

			float searchRadius = (navMeshMaxBounds.y - navMeshMinBounds.y) + 5.0f;

			if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, searchRadius, NavMesh.AllAreas))
			{
				Vector3 final = hit.position + Vector3.up * 0.5f;
				if (collectibleType != CollectibleType.Key)
				{
                    Instantiate(instance, final, Quaternion.identity, whereToAnchor);
                    spawnedCount++;
                }
				else
				{
					int rng = Random.Range(0, 100);
					GameObject toSpawnInstance;
                    if (rng < fakeSpawnChance)
                        toSpawnInstance = fakeInstance;
                    else
					{
						toSpawnInstance = instance;
						spawnedCount++;
					}

                    Instantiate(toSpawnInstance, final, Quaternion.identity, whereToAnchor);
                }
			}
		}
	}
}
