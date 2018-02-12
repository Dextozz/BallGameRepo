using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class CameraInput : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    EventSystem eventSystem;
    GameObject rightBut;
    GameObject leftBut;
    CameraMovement camera;

    float horizontalInput;

    bool moveRight;
    bool moveLeft;

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        rightBut = GameObject.Find("RightCameraController");
        leftBut = GameObject.Find("LeftCameraController");
        camera = Camera.main.gameObject.GetComponent<CameraMovement>();
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        //Disable the non selected button so he can't rotate left and right at the same time
        if(eventSystem.currentSelectedGameObject == rightBut && CameraMovement.rotatVal % 90 == 0)
        {
            leftBut.SetActive(false);
            moveRight = true;

            if(!CameraMovement.cameraLocked && !CameraMovement.gamePaused)
                camera.MoveRight();
        }
        else if(eventSystem.currentSelectedGameObject == leftBut && CameraMovement.rotatVal % 90 == 0)
        {
            rightBut.SetActive(false);
            moveLeft = true;

            if(!CameraMovement.cameraLocked && !CameraMovement.gamePaused)
                camera.MoveLeft();
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
