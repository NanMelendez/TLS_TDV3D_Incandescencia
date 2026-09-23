using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] [Min(0)] private int health = 1;

	public int Health
	{
		get => health;
		set
		{
			health = Mathf.Max(health + value, 0);
		}
	}

	public bool IsAlive
	{
		get => health > 0;
	}

	public void Heal(int extraHP)
	{
		health += extraHP;
	}

	public void TakeDamage(int dmg)
	{
        health -= dmg;
	}
}
