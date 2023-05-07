using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_main : MonoBehaviour
{
    public GameObject MAIN_UI;
    public bool isShown;

    // Start is called before the first frame update
    void Start()
    {
        isShown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (MAIN_UI != null)
        {
            if(MAIN_UI.activeSelf)
            {
                isShown = true;
            }
        }
    }

    public void showUI(GameObject MAIN_UI)
    {
        if (isShown == false)
        {
            MAIN_UI.SetActive(true);
            isShown = true;
        } else {
            MAIN_UI.SetActive(false);
            isShown = false;
        }
    }

    IEnumerator triggerUI()
    {
        isShown = !isShown; 
        yield return new WaitForSeconds(2);
    }
}
