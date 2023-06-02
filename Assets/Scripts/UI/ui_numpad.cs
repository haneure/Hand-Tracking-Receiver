using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ui_numpad : MonoBehaviour
{
    public List<char> screen = new List<char>();
    public int maxChar;
    public string password;

    public UnityEvent onCorrect;
    public UnityEvent onFalse;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < maxChar; i++)
        {
            GameObject child = this.transform.GetChild(i).GetChild(0).gameObject;
            child.GetComponent<Text>().text = "";
            //leftDetectedGestureUI.GetComponent<Text>().text
            //this.GetComponentsInChildren<>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < screen.Count; i++)
        {
            GameObject child = this.transform.GetChild(i).GetChild(0).gameObject;
            child.GetComponent<Text>().text = screen[i].ToString();
        }
    }

    public void addNumber0()
    {
        if (screen.Count < maxChar)
        {
            screen.Add('0');
        }
    }

    public void addNumber1()
    {
        if (screen.Count < maxChar)
        {
            screen.Add('1');
        }
    }
    public void addNumber2()
    {
        if (screen.Count < maxChar)
        {
            screen.Add('2');
        }
    }
    public void addNumber3()
    {
        if (screen.Count < maxChar)
        {
            screen.Add('3');
        }
    }
    public void addNumber4()
    {
        if (screen.Count < maxChar)
        {
            screen.Add('4');
        }
    }
    public void addNumber5()
    {
        if (screen.Count < maxChar)
        {
            screen.Add('5');
        }
    }
    public void addNumber6()
    {
        if (screen.Count < maxChar)
        {
            screen.Add('6');
        }
    }
    public void addNumber7()
    {
        if (screen.Count < maxChar)
        {
            screen.Add('7');
        }
    }
    public void addNumber8()
    {
        if (screen.Count < maxChar)
        {
            screen.Add('8');
        }
    }
    public void addNumber9()
    {
        if (screen.Count < maxChar)
        {
            screen.Add('9');
        }
    }

    public void Confirm()
    {
        string tempScreen = "";
        for (int i = 0; i < screen.Count; i++)
        {
            tempScreen += screen[i].ToString();
        }

        Debug.Log(tempScreen + " == " + password);

        if (tempScreen == password)
        {
            onCorrect.Invoke();
            StartCoroutine(hideCorrectPassword());
        } else
        {
            onFalse.Invoke();
            StartCoroutine(hideWrongPassword());
        }
    }
    public void Delete()
    {
        if (screen.Count > 0)
        {
            GameObject child = this.transform.GetChild(screen.Count - 1).GetChild(0).gameObject;
            child.GetComponent<Text>().text = "";
            screen.RemoveAt(screen.Count - 1);
        }
    }

    IEnumerator hideWrongPassword()
    {
        yield return new  WaitForSeconds(3);
        GameObject.Find("WrongPasswordAlert").gameObject.SetActive(false);
    }

    IEnumerator hideCorrectPassword()
    {
        yield return new WaitForSeconds(3);
        GameObject.Find("CorrectPasswordAlert").gameObject.SetActive(false);
    }
}
