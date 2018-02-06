using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class CameraInput : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    EventSystem eventSystem;
    GameObject rightBut;
    GameObject leftBut;

    float horizontalInput;

    [SerializeField]
    float speed;

    bool moveRight;
    bool moveLeft;

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        rightBut = GameObject.Find("RightCameraController");
        leftBut = GameObject.Find("LeftCameraController");
    }

    void Update()
    {
        if (!CameraMovement.cameraLocked && !CameraMovement.gamePaused)
        {
            if (moveRight)
                CameraMovement.horizontalInput -= speed;
            else if (moveLeft)
                CameraMovement.horizontalInput += speed;
            else
                CameraMovement.horizontalInput += Input.GetAxis("Mouse X");
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        //Disable the non selected button so he can't rotate left and right at the same time
        if(eventSystem.currentSelectedGameObject == rightBut)
        {
            leftBut.SetActive(false);
            moveRight = true;
        }
        else if(eventSystem.currentSelectedGameObject == leftBut)
        {
            rightBut.SetActive(false);
            moveLeft = true;
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        //Set both active
        rightBut.SetActive(true);
        leftBut.SetActive(true);

        moveRight = false;
        moveLeft = false;
    }
}
