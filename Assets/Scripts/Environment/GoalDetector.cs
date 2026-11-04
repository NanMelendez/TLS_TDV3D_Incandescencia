using UnityEngine;
using UnityEngine.Events;

public class GoalDetector : MonoBehaviour
{
    public UnityEvent actionOnCollide;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
			actionOnCollide?.Invoke();
	}
}
