using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool IsTutorialOn;

    public List<GameObject> hintPanels;
    public int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentIndex < hintPanels.Count-1)
            {
                hintPanels[currentIndex].SetActive(false);
                currentIndex++;
                hintPanels[currentIndex].SetActive(true);
            }
        }
    }
}
