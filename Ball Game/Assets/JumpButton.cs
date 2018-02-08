using UnityEngine.EventSystems;
using UnityEngine;

public class JumpButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    GameObject leftCamControl;
    GameObject rightCamControl;

    void Start()
    {
        leftCamControl = GameObject.Find("LeftCameraController");
        rightCamControl = GameObject.Find("RightCameraController");
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        leftCamControl.SetActive(false);
        rightCamControl.SetActive(false);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        leftCamControl.SetActive(true);
        rightCamControl.SetActive(true);
    }
}
