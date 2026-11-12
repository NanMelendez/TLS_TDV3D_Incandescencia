using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceReceptacle : PuzzleComponent, IPuzzleProgressProvider, IInteractable
{
	[SerializeField] private Transform piecesBase;
	[SerializeField] private GameObject pieceInstance;
	[SerializeField] private GameObject fakePieceInstance = null;
	[SerializeField][Range(0, 100)] private int fakePieceSpawnChance = 40;
	[SerializeField] private CollectibleType expectedType;
	[SerializeField] private string navmeshAreaName;
	private GameplayUIManager guim;

	readonly private List<GameObject> piecesGOs = new();
	private int collectedCount = 0;

	public event Action<int, int> OnProgressChanged;
	public int MaxProgress => piecesGOs.Count;
	public int CurrentProgress => collectedCount;

	private void Awake()
	{
		guim = FindFirstObjectByType<GameplayUIManager>();

		foreach (Transform child in piecesBase)
		{
			child.gameObject.SetActive(false);
			piecesGOs.Add(child.gameObject);
		}
	}

    private void Start()
    {
        PieceSpreadManager.Instance.SpawnPieces(navmeshAreaName, piecesGOs.Count, expectedType, pieceInstance, fakePieceInstance, fakePieceSpawnChance);
    }

    public void Interact()
	{
		PlayerPickupCollector playerCollector = FindFirstObjectByType<PlayerPickupCollector>();
		CollectibleData heldPiece = playerCollector.HeldPiece;

		if (collectedCount >= MaxProgress)
		{
			if (guim)
				guim.HandleMessageText("Ya coloqué todas las piezas aquí");

			if (heldPiece.isFake)
				playerCollector.Consume();
            return;
        }

		if (!playerCollector)
		{
			if (guim)
				guim.HandleMessageText("A mis creadores les faltó darme un \"PlayerPickupCollector\" >:(");
            AudioManager.Instance.SFXError();
            return;
        }

		if (!playerCollector.HoldingPiece)
		{
			if (guim)
				guim.HandleMessageText("Cierto, tengo que recoger una pieza...");
            return;
        }
		if (heldPiece.type != expectedType)
		{
			if (guim)
				guim.HandleMessageText("Ay. Esto no encaja aquí");
			AudioManager.Instance.SFXError();
			return;
        }

        if (heldPiece.isFake)
		{
			if (guim)
				guim.HandleMessageText("Pucha, esta pieza es falsa. Mejor la descarto");
            AudioManager.Instance.SFXError();
            playerCollector.Consume();
			return;
		}

		playerCollector.Consume();
		if (collectedCount < MaxProgress)
		{
			piecesGOs[collectedCount].SetActive(true);
			collectedCount++;
			OnProgressChanged?.Invoke(CurrentProgress, MaxProgress);

			if (collectedCount == MaxProgress)
			{
				AudioManager.Instance.SFXActivationDefault();
                Activate();
            }
		}
	}
}
