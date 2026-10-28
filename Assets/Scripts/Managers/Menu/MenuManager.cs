using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public void ChangeLevel(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void Exit()
	{

	}
}
