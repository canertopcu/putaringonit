using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector3, Vector3> OnPointerMove;
    public static event Action<Vector3> OnSwipe;
    public static event Action<Vector3> OnClickStart;
    public static event Action<Vector3> OnClickEnd;
    public static event Action<Vector2> OnJoystickMove;
    private Joystick joystick;
    public float swipeTreshold = 1f;

    public bool considerFingerOverUI = false;
    public static bool isClicked = false;
    public bool fingerOverUI = false;
    private Vector3 pointerPosition = Vector3.zero;
    private Vector3 pointerDownPosition = Vector3.zero;

    private void Start()
    {
        joystick = FindObjectOfType<Joystick>();
    }

    void Update()
    {

        if (considerFingerOverUI)
        {
            fingerOverUI = IsPointerOverUI();
        }

        if (!fingerOverUI)
        {
            DownCheck();
            MoveCheck();
            UpCheck();
        }
    }

    private void DownCheck()
    {
        bool isDown = false;
        if (Input.GetMouseButtonDown(0))
        {
            pointerPosition = Input.mousePosition;
            isDown = true;
        }

        else if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                pointerPosition = Input.touches[0].position;
                isDown = true;
            }
        }
        if (isDown)
        {
            isClicked = true;
            pointerDownPosition = pointerPosition;
            //InvokeOnFingerDown(pointerPosition);
            OnClickStart?.Invoke(pointerPosition);
        }
    }

    private void MoveCheck()
    {
        if (isClicked)
        {
            Vector3 deltaPos = Vector3.zero;
            if (Input.GetMouseButton(0))
            {
                deltaPos = Input.mousePosition - pointerPosition;
                pointerPosition = Input.mousePosition;
            }
            else if (Input.touchCount > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary)
                {
                    deltaPos = Input.touches[0].deltaPosition;
                    pointerPosition = Input.touches[0].position;
                }
            }
            //InvokeOnFingerMove(deltaPos, pointerPosition);
            OnPointerMove?.Invoke(pointerPosition, deltaPos);

            if (joystick != null)
            {
                //InvokeOnJoystickMove(joystick.Direction);
                OnJoystickMove?.Invoke(joystick.Direction);
            }
        }
    }
    private void UpCheck()
    {
        bool isUp = false;
        if (Input.GetMouseButtonUp(0))
        {
            pointerPosition = Input.mousePosition;
            isUp = true;
        }
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                pointerPosition = Input.touches[0].position;
                isUp = true;
            }
        }

        if (isUp)
        {
            isClicked = false;
            //InvokeOnFingerUp(pointerPosition);
            OnClickEnd?.Invoke(pointerPosition);
            SwipeCheck();
            pointerPosition = Vector3.zero;
        }


    }

    private void SwipeCheck()
    {
        if (Vector3.Distance(pointerPosition, pointerDownPosition) > swipeTreshold)
        {
            Vector3 deltaPos = Vector3.zero;
            deltaPos = pointerPosition - pointerDownPosition;
            //InvokeOnFingerSwipe(deltaPos);
            OnSwipe?.Invoke(deltaPos);
        }
    }
    private bool IsPointerOverUI()
    {
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
        return Input.GetTouch(0).phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId); // Doesn't work correctly on mobile without TouchPhase.Began
#endif
    }
     

}
