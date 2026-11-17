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

	[Range(1, 2)] public int maxPuzzleNodeBranching = 3;
	[Range(0.0f, 1.0f)] public float signalReceiverGenerationChance = 0.4f;
    public GameObject doorPrefab;
    public List<WeightedRoom> rooms = new();
	public List<PuzzleComponentGen> puzzleComponentPrefabs;
	public string playerSpawnpointName = "SPAWN_JUGADOR";
	public string doorSpawnpointName = "ANCLAJE_PUERTA";
	public string puzzleComponentsAnchorName = "==== COMPONENTES ====";
	public string propsAnchorName = "==== PROPS ====";

	private GameObject player;
	private GameObject currentRoom = null;
	private List<GameObject> roomProps = new();
	private List<GameObject> puzzleComponents = new();
	private List<GameObject> collectiblePieces = new();

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void GenerateRoom()
	{
		if (currentRoom != null)
		{
			Destroy(currentRoom);
			
			foreach(GameObject prop in roomProps)
				Destroy(prop);
			roomProps.Clear();
			foreach (GameObject pComponent in puzzleComponents)
				Destroy(pComponent);
			puzzleComponents.Clear();
			foreach (GameObject colPiece in collectiblePieces)
				Destroy(colPiece);
			collectiblePieces.Clear();
		}

		currentRoom = SelectRoomWeighted();

		Transform playerSpawn = currentRoom.transform.Find(playerSpawnpointName);
		Transform doorSpawn = currentRoom.transform.Find(doorSpawnpointName);
		Transform puzzleComponentsAnchor = currentRoom.transform.Find(puzzleComponentsAnchorName);
		Transform propsAnchor = currentRoom.transform.Find(propsAnchorName);

		player.transform.SetPositionAndRotation(playerSpawn.position, playerSpawn.rotation);

        /*
		 * Proceso de generación de la habitación
		 * =======================================
		 * 1. Eliminar habitación existente e instanciar cualquiera de los 3 prefabs de habitaciones
		 * 2. Teleportar al jugador a playerSpawn
		 * 3. Generar puerta (nodo padre del puzzle / PuzzleSignalReceiver maestro)
		 * 4. Primer bake del NavMesh Area
		 * 5. Generar puzzle:
		 *      5.1. Del nodo padre se generará un número aleatorio (1 - N) de nodos hijos
		 *      5.2. Cada nodo hijo tiene un 50/50 de convertirse en un PuzzleSignalReceiver o cualquiera de los otros PuzzleComponents
		 *			(Si es de tipo "Puzzle Maze")
		 *				- Desde el receptor se lanzará un rayo de longitud aleatoria. Al final del tramo se generará un espejo y se apuntará a una dirección aleatoria
		 *				- Repetir el paso anterior por cada espejo generado hasta alcanzar una profundidad L
		 *				- En el último nivel de profundidad, lanzar un rayo hasta la pared y generar un emisor
		 *				- Se generará un botón countdown / toggle
		 *			(Si es de tipo PasswordInput)
		 *				- Crear superficie secreta con espacio para texto
		 *				- Obtener la contraseña del componente y asignarlo a este texto
		 *			(Si es de tipo Button Countdown / Toggle)
		 *				- Hay una probabilidad P que se genere un SignalReceiver conectado a un foco
		 *			- Cada componente debe tener un NavMesh Obstacle para reducir el espacio de generación y evitar solapamientos
		 *			- Emparentar el componente generado al ancla "puzzleComponentsAnchor"
		 *		5.3. Cualquier PuzzleComponent generado será anexado a la lista de componentes del SignalReceiver padre
		 *		5.4. Repetir todo el proceso por cada nodo SignalReceiver.
		 *	6. Generar props
		 *		6.1. Se pueden generar un máximo de M props
		 *		6.2. Cada prop debe incluir un NavMesh Obstacle para restar espacio de generación
		 *		6.3. Se generará un prop en una posición y rotación aleatorias dentro del NavMesh Area
		 *	7. Re-bakear el NavMesh Area
		 *	8. Para todos los componentes PieceReceptacle
		 *		8.1. Invocar la función de esparcimiento de piezas del PieceSpreadManager
		 *		8.2. Cada pieza debe contener su propio NavMesh Obstacle
		 */

        puzzleComponents.Add(Instantiate(doorPrefab, doorSpawn.position, doorSpawn.rotation, doorSpawn));
	}

	private GameObject SelectRoomWeighted()
	{
		float totalWeight = 0.0f;
		foreach (WeightedRoom room in rooms)
			totalWeight += 1.0f / room.weight;

		float rndWeight = UnityEngine.Random.Range(0, totalWeight);

		float cumulativeWeight = 0.0f;
		foreach (WeightedRoom room in rooms)
		{
			cumulativeWeight += room.weight;

			if (rndWeight < cumulativeWeight)
				return room.roomPrefab;
		}

		return rooms[^1].roomPrefab;
	}
}
