using UnityEngine;

public class SingleShoot : IShootingStrategy
{
    public void Shooting(Transform firePoint, GameObject bulletPrefab, Transform target)
    {
        if (target == null) return;

        Vector2 direction = (target.position - firePoint.position).normalized;
        GameObject bullet = Object.Instantiate(bulletPrefab,firePoint.position,Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(direction);
    }

}
