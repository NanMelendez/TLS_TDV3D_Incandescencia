using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

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
	[Min(0.0f)] public float passwordSurfaceVerticalOffset = 1.75f;
	public LayerMask wallLayerMask;
	public LayerMask componentLayerMask;
	public LayerMask groundLayerMask;
	public GameObject doorPrefab;
	public List<WeightedRoom> rooms = new();
	public GameObject signalReceiverPrefab;
	public List<PuzzleComponentGen> puzzleComponentPrefabs = new();
	public GameObject passwordDisplayPrefab;
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

		CreateNode(puzzleComponents[0].GetComponentInChildren<PuzzleSignalReceiver>(), puzzleComponentsAnchor, 0);
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

	private PuzzleComponentGen SelectComponenteWeighted()
	{
		float totalWeight = 0.0f;
		foreach (PuzzleComponentGen compGen in puzzleComponentPrefabs)
			totalWeight += compGen.weight;

		if (totalWeight <= 0.0f)
			return default;

		float rndWeight = UnityEngine.Random.Range(0, totalWeight);

		foreach (PuzzleComponentGen compGen in puzzleComponentPrefabs)
		{
			if (compGen.weight <= 0.0f)
				continue;

			rndWeight -= compGen.weight;

			if (rndWeight < 0.0f)
				return compGen;
		}

		return default;
	}

    private void CreateNode(PuzzleSignalReceiver parent, Transform componentsAnchor, int level)
	{
		if (level >= maxPuzzleDepth)
			return;

		int branchingCount = UnityEngine.Random.Range(1, maxPuzzleNodeBranching + 1);

		for (int i = 0; i < branchingCount; i++)
		{
			float n = UnityEngine.Random.Range(0.0f, 1.0f);

			if (n < signalReceiverGenerationChance)
			{
				if (TryRandomPlacement(signalReceiverPrefab, 1.5f, out Vector3 position, out Vector3 normal))
				{
                    GameObject sri = Instantiate(signalReceiverPrefab, position, Quaternion.LookRotation(normal, Vector3.up), componentsAnchor);
                    PuzzleSignalReceiver psr = sri.GetComponentInChildren<PuzzleSignalReceiver>();

                    puzzleComponents.Add(sri);
                    parent.puzzleActivators.Add(psr);
                    CreateNode(psr, componentsAnchor, level + 1);
                }
			}
			else
			{
				PuzzleComponentGen compGen = SelectComponenteWeighted();
				
				if (TryRandomPlacement(compGen.pcPrefab, compGen.heightOffset, out Vector3 position, out Vector3 normal))
				{
                    GameObject iComponent = Instantiate(compGen.pcPrefab, position, Quaternion.LookRotation(normal, Vector3.up), componentsAnchor);
                    puzzleComponents.Add(iComponent);

                    switch (compGen.type)
					{
						case PuzzleComponentType.Button:
							PuzzleButton pb = iComponent.GetComponentInChildren<PuzzleButton>();
							parent.puzzleActivators.Add(pb);
							break;
						case PuzzleComponentType.Collectibles:
							PuzzlePieceReceptacle ppr = iComponent.GetComponent<PuzzlePieceReceptacle>();

							// Init ocurrirá SOLO tras generar todos los componentes de puzzle y props

							parent.puzzleActivators.Add(ppr);
							break;
						case PuzzleComponentType.LightMaze:
							PuzzleLaserReceiver plr = iComponent.GetComponent<PuzzleLaserReceiver>();
							parent.puzzleActivators.Add(plr);
							
							// Generar el maze de luces aquí

							break;
						case PuzzleComponentType.Password:
							PuzzlePasswordReceiver ppwdr = iComponent.GetComponent<PuzzlePasswordReceiver>();
							parent.puzzleActivators.Add(ppwdr);

							ppwdr.Init(GeneratePassword(ppwdr.MaxCharacters), 5);

							GenerateRandomPasswordSurface(ppwdr, componentsAnchor);

							break;
					}
				}
			}
		}
	}

	private void GenerateMirrorMaze()
	{

	}

	private void GenerateRandomPasswordSurface(PuzzlePasswordReceiver ppwdr, Transform componentsAnchor)
	{
		Transform roomCenter = currentRoom.transform;

		Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

		Vector3 direction = new(randomDirection.x, 0.0f, randomDirection.y);

		if (!Physics.Raycast(roomCenter.position, direction, out RaycastHit hit, 200, wallLayerMask))
			return;

		Vector3 normal = hit.normal;
		Vector3 position = new(hit.point.x, passwordSurfaceVerticalOffset, hit.point.z);

		GameObject temp = Instantiate(passwordDisplayPrefab, position, Quaternion.LookRotation(normal), componentsAnchor);
		PasswordDisplay pDisp = temp.GetComponentInChildren<PasswordDisplay>();

		pDisp.PasswordText = ppwdr.ExpectedPassword;
	}

	private string GeneratePassword(int length)
	{
		if (length <= 0) return "";

		char[] digits = new char[length];
		for (int i = 0; i < length; i++)
			digits[i] = (char)('0' + UnityEngine.Random.Range(0, 10));

		return new string(digits);
	}

	private bool TryRandomPlacement(GameObject component, float verticalOffset, out Vector3 position, out Vector3 normal)
	{
		position = Vector3.zero;
		normal = Vector3.zero;

		Transform roomCenter = currentRoom.transform;

		for (int i = 0; i < maxComponentGenerationAttemps; i++)
		{
			Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

			Vector3 direction = new(randomDirection.x, 0.0f, randomDirection.y);

			if (!Physics.Raycast(roomCenter.position, direction, out RaycastHit hit, 200, wallLayerMask))
				continue;

			Vector3 candidateNormal = hit.normal;

			Vector3 candidatePosition = new(hit.point.x, verticalOffset, hit.point.z);

			if (!IsPositionAvailable(candidatePosition, component, candidateNormal))
				continue;

			position = candidatePosition;
			normal = candidateNormal;

			return true;
		}

		return false;
	}

	private bool IsPositionAvailable(Vector3 position, GameObject component, Vector3 normal)
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
