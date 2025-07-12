using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IStunnable
{
    public int health = 100;

    // Stun Variables
    private bool isStunned = false;
    private float stunTimer = 0f;

    void Update()
    {
        // Cooldown stun per frame
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                Debug.Log("Enemy hồi phục khỏi stun");
            }
        }

        //if (isStunned)
        //{
        //    // return; hoặc skip logic AI
        //}
        //else
        //{
        //    // Thực hiện AI nếu không bị stun
        //}
    }

    public void ApplyStun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        Debug.Log($"Enemy bị stun trong {duration} giây");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(damage + " damage taken. Remaining health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy đã chết");
        Destroy(gameObject);
    }
}
