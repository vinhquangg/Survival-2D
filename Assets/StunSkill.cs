using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunSkill : MonoBehaviour
{
    public float stunRadius;          // Bán kính stun tối đa
    public float stunDuration;        // Thời gian kẻ địch bị stun
    public float expandSpeed;         // Tốc độ lan stun (đơn vị/giây)
    public ParticleSystem stunEffect; // Hiệu ứng particle
    public LayerMask enemyLayer;      // Layer kẻ địch

    private float debugRadius = 0f;   // Dùng để debug Gizmo

    public void Activate()
    {
        // 🔥 Play hiệu ứng
        stunEffect.Play();

        // 🚀 Bắt đầu stun lan dần từ tâm ra
        StartCoroutine(ExpandStun());
    }

    private IEnumerator ExpandStun()
    {
        float currentRadius = 0f;
        debugRadius = 0f;

        HashSet<Collider2D> stunnedEnemies = new HashSet<Collider2D>();

        while (currentRadius < stunRadius)
        {
            currentRadius += expandSpeed * Time.deltaTime;
            debugRadius = currentRadius;

            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, currentRadius, enemyLayer);
            foreach (var enemy in enemies)
            {
                if (!stunnedEnemies.Contains(enemy))
                {
                    // 💡 Kiểm tra kỹ khoảng cách thật để không stun ngoài rìa
                    float distance = Vector2.Distance(transform.position, enemy.transform.position);
                    if (distance <= stunRadius)
                    {
                        IStunnable stunnable = enemy.GetComponent<IStunnable>();
                        if (stunnable != null)
                        {
                            stunnable.ApplyStun(stunDuration);
                            stunnedEnemies.Add(enemy);
                        }
                    }
                }
            }

            yield return null;
        }

        // 🧹 Huỷ skill sau khi xong
        Destroy(gameObject);
    }

    // 💠 Gizmo vẽ vùng stun
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stunRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.5f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, debugRadius);
    }
}
