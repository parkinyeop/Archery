using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickMovement : MonoBehaviour
{
    static JoyStickMovement instance;
    public static JoyStickMovement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<JoyStickMovement>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("JoyStickMovemnet");
                    instance = instanceContainer.AddComponent<JoyStickMovement>();
                }
            }
            return instance;
        }
    }

    public GameObject smallStick;
    public GameObject bgStick;
    Vector3 stickFirstPosition;
    public Vector3 joyVec;
    Vector3 joyStickFirstPosition;
    float stickRadius;

    // Start is called before the first frame update
    void Start()
    {
        stickRadius = bgStick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        joyStickFirstPosition = bgStick.transform.position;
    }

    public void PointDown()
    {
        bgStick.transform.position = Input.mousePosition;
        smallStick.transform.position = Input.mousePosition;
        stickFirstPosition = Input.mousePosition;

        //SetTrigger 이동 애니메이션 연결
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 DragPosition = pointerEventData.position;
        joyVec = (DragPosition - stickFirstPosition).normalized;
        
        //Debug.Log(DragPosition);

        float stickDistance = Vector3.Distance(DragPosition, stickFirstPosition);

        if (stickDistance < stickRadius)
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;
        }
        else
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickRadius; ;
        }
    }

    public void Drop()
    {
        joyVec = Vector3.zero;
        bgStick.transform.position = joyStickFirstPosition;
        smallStick.transform.position = joyStickFirstPosition;
        //SetTrigger 아이들 애니메이션으로 연결
    }
}
