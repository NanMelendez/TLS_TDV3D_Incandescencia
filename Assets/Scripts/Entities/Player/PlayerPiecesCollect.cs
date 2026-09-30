using UnityEngine;

public class PlayerPiecesCollect : MonoBehaviour
{
	private int collectedPiecesCount = 0;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("CollectibleComponent"))
			collectedPiecesCount++;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("CollectiblesRecipient") && collectedPiecesCount > 0)
		{
			PuzzleComponentRecipient pcr = collision.gameObject.GetComponent<PuzzleComponentRecipient>();
			while (collectedPiecesCount > 0)
			{
				pcr.ReceivePiece();
				collectedPiecesCount--;
			}
		}
	}
}
