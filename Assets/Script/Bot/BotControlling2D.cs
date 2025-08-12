using UnityEngine;

public class BotControlling2D : MonoBehaviour, IDamageable
{
    [Header("Bot Settings")]
    public int maxHealth = 1;
    public int currentHealth;

    [Header("Targeting Settings")]
    public MoveToPlayerBehavior moveToPlayerBehavior;
    public IdleBehavior idleBehavior;

    public void Start()
    {
        moveToPlayerBehavior = GetComponent<MoveToPlayerBehavior>();
        currentHealth = maxHealth;
    }

    public void Update()
    {
        Debug.Log("Bắt đầu di chuyển");
        if (moveToPlayerBehavior != null && moveToPlayerBehavior.targetingBehavior != null)
        {
            Debug.Log("Bot đang di chuyển đến người chơi");
            moveToPlayerBehavior.MoveToTarget();
        }
        else
        {
            Debug.LogWarning("MoveToPlayerBehavior hoặc TargetingBehavior chưa được gán!");
        }

    }
    public void ResetBot() 
    {
        currentHealth = maxHealth;
        if(moveToPlayerBehavior != null)
        {
            moveToPlayerBehavior.MoveToTarget();
        }
        Debug.Log("Bot đã được reset về trạng thái ban đầu.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
            // Handle collision with player
            Debug.Log("Bot collided with player!");
        }
    }
    private void Die() 
    {
        Debug.Log("Bot died!");
        PoolingManager.Instance.ReturnToPool("Bot", transform.gameObject);
    }

    public void TakeDamage(int damage)
    {
       //currentHealth -= damage;
        Debug.Log($"Bot took {damage} damage. Current health: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
