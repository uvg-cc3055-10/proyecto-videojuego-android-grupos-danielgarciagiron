using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image back;
    public Image front;
    private Vector3 inputVector;




    public virtual void OnDrag(PointerEventData data)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(back.rectTransform, data.position, data.pressEventCamera, out position))
        {
            position.x = (position.x / back.rectTransform.sizeDelta.x);
            position.y = (position.y / back.rectTransform.sizeDelta.y);

            inputVector = new Vector3(position.x * 2 + 1, 0, position.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            front.rectTransform.anchoredPosition = new Vector3(inputVector.x * (back.rectTransform.sizeDelta.x / 2), inputVector.z * (back.rectTransform.sizeDelta.y / 2));

        }
    }
    public virtual void OnPointerDown(PointerEventData data)
    {
        OnDrag(data);
    }

    public virtual void OnPointerUp(PointerEventData data)
    {
        inputVector = Vector3.zero;
        front.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float horizontal()
    {
        return inputVector.x;
    }

    public float vertical()
    {
        return inputVector.z;
    }
}

