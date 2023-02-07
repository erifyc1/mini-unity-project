using UnityEngine;
using UnityEngine.InputSystem;

public class BuildController : MonoBehaviour
{
    private float LMB;
    private Vector2 mousePos;
    private bool holdingItem = false;
    private GameObject carriedObject = null;
    [SerializeField] Vector3 offset = new Vector3(70, 0, 70);
    void Start()
    {
        
    }

    void Update()
    {
        if (holdingItem && carriedObject != null)
        {
            var ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));
            Vector3 point = ray.GetPoint(400);
            carriedObject.transform.position = point + offset;
        }
    }

    public void OnLMB(InputAction.CallbackContext context)
    {
        LMB = context.ReadValue<float>();
        if (context.started && LMB == 1f)
        {
            TryPickupItem();
        }
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    private void TryPickupItem()
    {
        if (!holdingItem)
        {
            var ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2000f, LayerMask.GetMask("Object"), QueryTriggerInteraction.Collide))
            {
                Transform t = hit.transform;
                // check for buttons
                if (t.tag == "nextButton")
                {
                    t.parent.GetComponent<AnchorPoint>().AnimateNextStep();
                }
                else
                {
                    // attempts to navigate to parent with pickup tag
                    while (t.parent != null)
                    {
                        if (t.tag == "pickup") break;
                        t = t.parent;
                    }
                    if (t.tag == "pickup")
                    {
                        carriedObject = t.gameObject;
                        holdingItem = true;
                    }
                }
            }
        }
        else
        {
            carriedObject = null;
            holdingItem = false;
        }
    }

    public void PlacedItem()
    {
        carriedObject = null;
        holdingItem = false;
    }
}
