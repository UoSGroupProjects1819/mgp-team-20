using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour
{
    float throwForce = 600.0f;
    Vector3 objectPosition;
    Vector3 objectScale;
    float distance;

    public GameObject item;
    public GameObject temporaryParent;
    public bool isHolding = false;

    void Start()
    {
        objectScale = transform.lossyScale;
    }

    void Update()
    {
        //item.transform.localScale = objectScale;
        distance = Vector3.Distance(item.transform.position, temporaryParent.transform.position);
        if (distance >= 10.0f)
            isHolding = false;
        if (isHolding == true)
        {
                        
            if (Input.GetMouseButtonDown(0))
            {
                item.GetComponent<Rigidbody>().isKinematic = false;
                item.GetComponent<Rigidbody>().AddForce(temporaryParent.transform.forward * throwForce);
                isHolding = false;
                if (item.name.ToLower().Contains("bottle")  //If the item's name contains
                    || item.name.ToLower().Contains("shot") //bottle OR (|| is or) shot...
                    || item.name.ToLower().Contains("glass")
                    || item.name.ToLower().Contains("whiskey"))
                {
                    //#PLAY CLINK SOUND
                }
                else
                {
                    //#PLAY THUD SOUNDS
                }
            }
        }
        else
        {
            objectPosition = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = objectPosition;
        }
    }

    void OnMouseOver()
    {
       if (Input.GetKeyDown(KeyCode.E))
        {
            isHolding = true;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().detectCollisions = true;
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.transform.SetParent(temporaryParent.transform);
            temporaryParent.GetComponent<Rigidbody>().detectCollisions = true;
        }
    }
}
