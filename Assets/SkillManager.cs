using System.Collections;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject stunSkillPrefab;
    private bool canUseStun = true;

    public void UseStunSkill()
    {
        if (!canUseStun) return;

        // Gọi kỹ năng
        GameObject stunFx = Instantiate(stunSkillPrefab, transform.position, Quaternion.identity);
        stunFx.GetComponent<StunSkill>().Activate();

        // Ngăn dùng nhiều lần nếu cần cooldown
        canUseStun = false;
        StartCoroutine(ResetStunCooldown(3f)); // Cooldown 3 giây (tuỳ bạn)
    }

    private IEnumerator ResetStunCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canUseStun = true;
    }
}
