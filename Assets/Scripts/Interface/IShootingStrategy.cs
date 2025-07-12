using UnityEngine;

public interface IShootingStrategy
{
    void Shooting(Transform firePoint, GameObject bulletPrefab, Transform target);
}
