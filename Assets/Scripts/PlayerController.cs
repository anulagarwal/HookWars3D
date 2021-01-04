using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public FixedJoystick joystick;
    public Transform hook;
    public bool IsHooking;
    public CharacterController cc;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate character only when not hooking
         if (hook.GetComponent<Hook>().currentState ==Hook.State.Stop)
         {
            // GetComponent<Rigidbody>().velocity = Vector3.zero;
             if (Input.GetMouseButton(0))
             {
             }
             //Send hook
             if (Input.GetMouseButtonUp(0))
             {
                 hook.GetComponent<Hook>().HookNow();
             }
            //Movement();

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "Enemy")
        {          
            Destroy(collision.gameObject);
        }
    }

    private void Movement()
    {
        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;
        cc.Move(direction * Time.deltaTime * moveSpeed);

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
           // animator.SetBool("Run", true);

           // EnableWoodenCartMovement(true);
        }
        else
        {
          //  animator.SetBool("Run", false);

           // EnableWoodenCartMovement(false);
        }
    }

}

