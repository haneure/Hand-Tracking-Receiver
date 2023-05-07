using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_panel : MonoBehaviour
{
    Image img;

    public byte r;
    public byte g;
    public byte b;

    public byte r_normal;
    public byte g_normal;
    public byte b_normal;
    public bool transparent;

    public bool isSelected;
    
    // Start is called before the first frame update
    void Start()
    {
        img = this.gameObject.GetComponent<Image>();
        isSelected = false;
    }

    public void ChangeColorSelected()
    {
        if (!isSelected)
        {
            Color temp = new Color32(r, g, b, 255);
            img.color = temp;
            isSelected = true;
        } else if(isSelected) {
            Color temp;
            if (transparent)
            {
                temp = new Color32(r_normal, g_normal, b_normal, 107);
            } else
            {
                temp = new Color32(r_normal, g_normal, b_normal, 255);
            }
            img.color = temp;
            isSelected = false;
        }
        
    }
}
