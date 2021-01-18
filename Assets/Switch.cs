using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public Transform connectedObject;
   public bool On = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (On)
        {
           
    connectedObject.rotation = Quaternion.Lerp(connectedObject.rotation, Quaternion.Euler(0, 90, 0), 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hookable")
        {
            On = true;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
