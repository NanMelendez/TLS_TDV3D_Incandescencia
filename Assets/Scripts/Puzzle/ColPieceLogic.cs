using UnityEngine;

public class ColPieceLogic : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
	}
}
