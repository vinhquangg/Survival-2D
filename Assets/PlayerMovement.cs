using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public JoyStickMovement joystickMovement;
    public float playerSpeed;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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
    }
}
