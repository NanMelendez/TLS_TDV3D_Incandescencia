using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalDetector : MonoBehaviour
{
    [SerializeField] private string nextLevelName;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            SceneManager.LoadScene(nextLevelName);
    }
}
