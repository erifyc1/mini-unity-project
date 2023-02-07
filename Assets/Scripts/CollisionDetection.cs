using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        transform.parent.parent.GetComponent<AnchorPoint>().PartCollision(other.transform.parent.gameObject);
    }
}
