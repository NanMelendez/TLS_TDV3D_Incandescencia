using UnityEngine;

public class PlayerPiecesCollect : MonoBehaviour
{
	private int collectedPiecesCount = 0;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("CollectibleComponent"))
			collectedPiecesCount++;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("CollectiblesRecipient") && collectedPiecesCount > 0)
		{
			collision.gameObject.GetComponent<PuzzleComponentRecipient>().ReceivePiece();
			collectedPiecesCount--;
		}
	}
}
