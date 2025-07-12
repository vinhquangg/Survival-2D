using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickMovement : MonoBehaviour
{
    public GameObject joystickBG;
    public GameObject joystick;
    public Vector2 joystickVec;
    private Vector2 joystickTouchPos;
    private Vector2 joystickOriginalPos;
    private float joystickRadius;
    void Start()
    {
        joystickOriginalPos = joystickBG.transform.position;
        joystickRadius = joystickBG.GetComponent<RectTransform>().sizeDelta.x / 4f;
    }

    public void PointerDown()
    {
        joystick.transform.position = Input.mousePosition;
        joystickBG.transform.position = Input.mousePosition;
        joystickTouchPos = Input.mousePosition;
    }

    public void Drag(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
        Vector2 dragPos = pointerData.position;
        joystickVec = (dragPos - joystickTouchPos).normalized;

        float joystickDistance = Vector2.Distance(dragPos, joystickTouchPos);

        if(joystickDistance < joystickRadius)
        {
            joystick.transform.position = joystickTouchPos + joystickVec * joystickDistance;
        }
        else
        {
            joystick.transform.position = joystickTouchPos + joystickVec * joystickRadius;
        }
    }

    public void PointerUp()
    {
        joystick.transform.position = joystickOriginalPos;
        joystickBG.transform.position = joystickOriginalPos;
        joystickVec = Vector2.zero;
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
