using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 0.5f;
    public float detectionRadius = 6f;
    public LayerMask enemyLayer;

    private float shootTimer = 0f;
    private IShootingStrategy shootingStrategy;

    private void Start()
    {
        shootingStrategy = new SingleShoot(); 
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            Transform target = GetNearestEnemy();
            if (target != null)
            {
                shootingStrategy.Shooting(firePoint, bulletPrefab, target);
                shootTimer = shootInterval;
            }
        }
    }

    public Transform GetNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        Transform nearest = null;
        float shortestDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

}
