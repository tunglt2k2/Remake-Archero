using UnityEngine;
using UnityEngine.EventSystems;
public class JoyStickMovement : MonoBehaviour
{
   public static JoyStickMovement Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<JoyStickMovement>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("JoyStickMovement");
                    instance = instanceContainer.AddComponent<JoyStickMovement>();
                }
            }
            return instance;
        }
    }
    private static JoyStickMovement instance;

    public GameObject smallStick;
    public GameObject bGStick;
    public Vector3 joyVec;
    private Vector3 stickFirstPosition;
    private Vector3 joyStickFirstPosition;
    private float stickRadius;

    public bool isPlayerMoving { get; private set; } = false;
    private void Start()
    {
        stickRadius = bGStick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        joyStickFirstPosition = bGStick.transform.position;
    }
    public void PointDown()
    {
        bGStick.transform.position = Input.mousePosition;
        smallStick.transform.position = Input.mousePosition;
        stickFirstPosition = Input.mousePosition;
        //Set Trigger
        if (!PlayerMovement.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            // Debug.Log("WALK");
            PlayerMovement.Instance.anim.SetBool("Attack", false);
            PlayerMovement.Instance.anim.SetBool("Idle", false);
            PlayerMovement.Instance.anim.SetBool("Walk", true);
        }
        isPlayerMoving = true;
        PlayerTargeting.Instance.getATarget = false;
    }
    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 DragPosition = pointerEventData.position;
        joyVec = (DragPosition - stickFirstPosition).normalized;

        float stickDistance = Vector3.Distance(DragPosition, stickFirstPosition);

        if(stickDistance < stickRadius)
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;
        }
        else
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickRadius;
        }
    }
    public void Drop()
    {
        joyVec = Vector3.zero;
        //Set Trigger
        if (!PlayerMovement.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
            //Debug.Log("IDLE!");
            PlayerMovement.Instance.anim.SetBool("Attack", false);
            PlayerMovement.Instance.anim.SetBool("Walk", false);
            PlayerMovement.Instance.anim.SetBool("Idle", true);
        }
        //Reset
        isPlayerMoving = false;
        bGStick.transform.position = joyStickFirstPosition;
        smallStick.transform.position = joyStickFirstPosition;
    }

}


