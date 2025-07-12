using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public JoyStickMovement joystickMovement;
    public float playerSpeed;
    private Rigidbody2D rb;
    private AutoShoot autoShoot;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        autoShoot = GetComponent<AutoShoot>();
    }

    void FixedUpdate()
    {
        if(joystickMovement.joystickVec.y != 0)
        {
            rb.linearVelocity= new Vector2(joystickMovement.joystickVec.x * playerSpeed, joystickMovement.joystickVec.y * playerSpeed);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        Flip(joystickMovement.joystickVec.x);
    }

    private void Flip(float xInput)
    {
        Transform enemy = autoShoot.GetNearestEnemy();

        if (enemy != null)
        {
            float dirToEnemy = enemy.position.x - transform.position.x;

            if (Mathf.Abs(dirToEnemy) > 0.1f)
            {
                transform.localScale = new Vector3(Mathf.Sign(dirToEnemy), 1f, 1f);
                return;
            }
        }

        if (Mathf.Abs(xInput) > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Sign(xInput), 1f, 1f);
        }
    }

}
