using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public enum State { Out, In, Stop, Pulled, Pulling};
    public float speed;
    public float duration;

    public float distance;
    public Vector3 origPos;
    public Transform player;
    public State currentState;
    public bool IsHooking;
    public float StartTime;
    // Start is called before the first frame update
    void Start()
    {
        origPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.Out)
        {
            if(Vector3.Distance(transform.position, player.position)> distance)
            {
                currentState = State.In;
                GetComponent<Rigidbody>().AddForce(player.forward * -speed *2, ForceMode.Impulse);
            }
        }

        if(currentState== State.In)
        {
            if (Vector3.Distance(transform.position, player.position) <=0.6f)
            {
                transform.localPosition = origPos;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                currentState = State.Stop;         
            }
        }

        if(currentState == State.Pulled)
        {
            if (Vector3.Distance(transform.position, player.position) <= 0.6f)
            {
                transform.SetParent(player);
                transform.localPosition = origPos;
                currentState = State.Stop;

                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        if(currentState == State.Pulling)
        {
            if (Vector3.Distance(transform.position, player.position) <= 0.6f)
            {
                transform.localPosition = origPos;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.GetChild(0).parent = null;
                currentState = State.Stop;
            }
        }
    }

    public void HookNow()
    {
        currentState = State.Out;
        StartTime = Time.time;
        GetComponent<Rigidbody>().AddForce(player.forward * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState == State.Out)
        {
            if (collision.gameObject.tag == "Wall")
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                player.GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
                transform.parent = null;

                currentState = State.Pulled;
            }
            if(collision.gameObject.tag == "Enemy")
            {
                print("enemy");
                currentState = State.Pulling;
                GetComponent<Rigidbody>().AddForce(player.forward * -speed * 2, ForceMode.Impulse);
                collision.gameObject.transform.SetParent(transform);
            }

        }
    }
}
