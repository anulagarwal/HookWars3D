using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AwesomeText : MonoBehaviour
{
    public float speed;
    public string text;
    public float fadeSpeed;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
        GetComponentInChildren<TextMeshPro>().text = text;
    }
        
    public void SetText( string s) {
        GetComponentInChildren<TextMeshPro>().text = s;

    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, speed, 0) * Time.deltaTime);
        GetComponentInChildren<TextMeshPro>().alpha -= fadeSpeed;
    }
}
