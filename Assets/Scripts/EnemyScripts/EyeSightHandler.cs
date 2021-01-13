using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSightHandler : MonoBehaviour
{
    #region Properties
    #endregion

    #region MonoBehaviour Functions
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!PlayerCaught)
            {
                PlayerCaught = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerCaught = false;
        }
    }
    #endregion

    #region Getter And Setter
    public bool PlayerCaught { get; set; }
    #endregion
}
