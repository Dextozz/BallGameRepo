using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    private Image bgImg;
    private Image joystickImg;
    private Vector3 inputVector;

    GameObject leftCamInput;
    GameObject rightCamInput;

    private void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();

        rightCamInput = GameObject.Find("RightCameraController");
        leftCamInput = GameObject.Find("LeftCameraController");
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform,
                                                                    ped.position,
                                                                    ped.pressEventCamera,
                                                                    out pos))
        {
            //Get the position relative to bgImg size 
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            //So the position goes from -1 to 1 and not from 0 to 1
            inputVector = new Vector3(pos.x * 2 - 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1) ? inputVector.normalized : inputVector;

            joystickImg.rectTransform.anchoredPosition =
                new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3),
                            inputVector.z * (bgImg.rectTransform.sizeDelta.y / 3));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
        leftCamInput.SetActive(false);
        rightCamInput.SetActive(false);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;

        leftCamInput.SetActive(true);
        rightCamInput.SetActive(true);
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxis("Vertical");
    }
}
