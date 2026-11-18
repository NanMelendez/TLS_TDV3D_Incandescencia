using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
	[Serializable]
	public struct WeightedRoom
	{
		public GameObject roomPrefab;
		[Min(0.0f)] public float weight;
		[Range(1, 4)] public int maxPuzzleDepth;
	}

	public enum PuzzleComponentType
	{
		Button,
		Password,
		Collectibles,
		LightMaze
	}

	[Serializable]
	public struct PuzzleComponentGen
	{
		public GameObject pcPrefab;
		public PuzzleComponentType type;
		[Min(0.0f)] public float heightOffset;
		[Min(0.0f)] public float weight;
	}

	[Range(1, 3)] public int maxPuzzleNodeBranching = 3;
	[Range(0.0f, 1.0f)] public float signalReceiverGenerationChance = 0.4f;
	[Min(1)] public int maxComponentGenerationAttemps = 20;
	public LayerMask wallLayerMask;
	public LayerMask componentLayerMask;
	public GameObject doorPrefab;
	public List<WeightedRoom> rooms = new();
	public GameObject signalReceiverPrefab;
	public List<PuzzleComponentGen> puzzleComponentPrefabs = new();
	public Transform roomGenAnchor;
	public string playerSpawnpointName = "SPAWN_JUGADOR";
	public string doorSpawnpointName = "ANCLAJE_PUERTA";
	public string puzzleComponentsAnchorName = "==== COMPONENTES ====";
	public string propsAnchorName = "==== PROPS ====";

	private GameObject player;
	private GameObject currentRoom = null;
	private List<GameObject> roomProps = new();
	private List<GameObject> puzzleComponents = new();
	private List<GameObject> collectiblePieces = new();
	private int maxPuzzleDepth;

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void GenerateRoom()
	{
		if (currentRoom != null)
		{
			Destroy(currentRoom);

			roomProps.Clear();
			puzzleComponents.Clear();
			collectiblePieces.Clear();
		}

		currentRoom = Instantiate(SelectRoomWeighted(), roomGenAnchor);

		Transform playerSpawn = currentRoom.transform.Find(playerSpawnpointName);
		Transform doorSpawn = currentRoom.transform.Find(doorSpawnpointName);
		Transform puzzleComponentsAnchor = currentRoom.transform.Find(puzzleComponentsAnchorName);
		Transform propsAnchor = currentRoom.transform.Find(propsAnchorName);

		player.transform.SetPositionAndRotation(playerSpawn.position, playerSpawn.rotation);

		puzzleComponents.Add(Instantiate(doorPrefab, doorSpawn.position, doorSpawn.rotation, doorSpawn));

		CreateNode(puzzleComponents[0].GetComponentInChildren<PuzzleSignalReceiver>(), 0);
	}

	private GameObject SelectRoomWeighted()
	{
		float totalWeight = 0.0f;
		foreach (WeightedRoom room in rooms)
			totalWeight += room.weight;

		maxPuzzleDepth = 0;

		if (totalWeight <= 0.0f)
			return null;

		float rndWeight = UnityEngine.Random.Range(0, totalWeight);

		foreach (WeightedRoom room in rooms)
		{
			if (room.weight <= 0.0f)
				continue;

			rndWeight -= room.weight;

			if (rndWeight < 0.0f)
			{
				maxPuzzleDepth = room.maxPuzzleDepth;
				return room.roomPrefab;
			}
		}

		return null;
	}

	private void CreateNode(PuzzleSignalReceiver parent, int level)
	{
		int branchingCount = UnityEngine.Random.Range(1, maxPuzzleNodeBranching);

		for (int i = 0; i < branchingCount; i++)
		{
			float n = UnityEngine.Random.Range(0.0f, 1.0f);

			if (n < signalReceiverGenerationChance)
			{

			}
			else
			{

			}
		}
	}

	private bool TryRandomPlacement(GameObject targetRoom, GameObject component, float verticalOffset, out Vector3 position, out Vector3 normal)
	{
		position = Vector3.zero;
		normal = Vector3.zero;

		Transform roomCenter = targetRoom.transform;

		for (int i = 0; i < maxComponentGenerationAttemps; i++)
		{
			Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

			Vector3 direction = new(randomDirection.x, 0.0f, randomDirection.y);

			if (!Physics.Raycast(roomCenter.position, direction, out RaycastHit hit, 200, wallLayerMask))
				continue;

			Vector3 candidateNormal = hit.normal;

			// Vector3 candidatePosition = hit.point + Vector3.up * verticalOffset;
			Vector3 candidatePosition = new(hit.point.x, verticalOffset, hit.point.z);

			if (!IsPositionAvailable(candidatePosition, component, candidateNormal))
				continue;

			position = candidatePosition;
			normal = candidateNormal;

			return true;
		}

		return false;
	}

	private bool IsPositionAvailable(
	Vector3 position,
	GameObject component,
	Vector3 normal)
	{
		BoxCollider box = component.GetComponentInChildren<BoxCollider>();

		if (!box)
			return false;

		Transform root = component.transform;
		Transform boxTransform = box.transform;

		Quaternion rootRotation = Quaternion.LookRotation(normal, Vector3.up);

		Vector3 localCenter = root.InverseTransformPoint(boxTransform.TransformPoint(box.center));

		Vector3 center = position + rootRotation * Vector3.Scale(localCenter, root.lossyScale);

		Quaternion relativeRotation = Quaternion.Inverse(root.rotation) * boxTransform.rotation;

		Quaternion boxRotation = rootRotation * relativeRotation;

		Vector3 scale = boxTransform.lossyScale;

		Vector3 halfExtents = Vector3.Scale(box.size * 0.5f, new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z)));

		Collider[] collisions = Physics.OverlapBox(center, halfExtents, boxRotation, componentLayerMask, QueryTriggerInteraction.Ignore);

		return collisions.Length == 0;
	}
}
