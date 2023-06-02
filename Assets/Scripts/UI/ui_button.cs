using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ui_button : MonoBehaviour
{
    public GameObject player;
    public Button button;
    AudioSource sound;

    public UnityEvent onPress;
    public UnityEvent onRelease;

    bool isPressed;
    public bool disable;
    public int disableDelay;
    public bool isDoubleTap;
    public int delay;
    public List<string> contactList;

    public enum Hand // your custom enumeration
    {
        left,
        right
    };
    // Start is called before the first frame update
    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
        sound = GetComponent<AudioSource>();
        isPressed = false;
        isDoubleTap = false;
        disable = false;
        contactList.Add("Point (8)");
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0 && isPressed)
        {
            delay--;
        } 

        if (delay <= 0)
        {
            isPressed = false;
        }

        if (disableDelay > 0)
        {
            disableDelay--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string log = other.name;
        string gesture = "";
        string type = "";

        if (other.gameObject.GetComponent<hand_sensor>().gesture != null)
        {
            gesture = other.gameObject.GetComponent<hand_sensor>().gesture;
        }

        if (other.gameObject.GetComponent<hand_sensor>().hand.ToString() != null) {
            type = other.gameObject.GetComponent<hand_sensor>().hand.ToString();
        }
        //Debug.Log(other.name + ", Gesture: " + gesture + ", Type: " + type + " Hand");

        if (disable)
        {
            print("disabled");
        } else
        {
            if (disableDelay <= 0)
            {
                if (type == "right" && gesture == "onepointforward" || gesture == "twopointforward")
                {
                    if (!isDoubleTap)
                    {
                        if (!isPressed)
                        {
                            if (contactList.Contains(other.name.ToString()))
                            {
                                if (other.gameObject.layer == LayerMask.NameToLayer("Hands"))
                                {
                                    delay = 50;
                                    isPressed = true;
                                    sound.Play();
                                    button.OnSelect(null);
                                    button.Select();
                                    button.onClick.Invoke();
                                    onPress.Invoke();
                                }
                            }
                        }
                    }
                    else
                    {
                        doubleTap(other);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hands"))
        {
            //isPressed = false;
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            onRelease.Invoke();
        }
    }

    public void doubleTap(Collider other)
    {

    }
    
    public void toggleDisable(GameObject UI)
    {
        if (disable)
        {
            disableDelay = 50;
            disable = false;
        } else if (!disable)
        {
            disable = true;
        }
    }

    public void spawnGameObject(GameObject gameobject)
    {
        player = GameObject.Find("Player");
        Vector3 summonPosition = new Vector3(player.transform.position.x + 2f, player.transform.position.y, player.transform.position.z + 2f);
        Instantiate(gameobject, summonPosition, Quaternion.identity);
    }

    public void SpawnSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        float x = Random.Range(-10f, 10.0f);
        float z = Random.Range(2f, 10.0f);

        sphere.transform.localPosition = new Vector3(x, 1, z);
        sphere.AddComponent<Rigidbody>();
    }

    public void SpawnCube()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Cube);

        sphere.transform.localScale = new Vector3(1f, 1f, 1f);
        player = GameObject.Find("Player");

        sphere.transform.localPosition = new Vector3(player.transform.position.x + 2f, player.transform.position.y, player.transform.position.z + 2f);
        sphere.AddComponent<Rigidbody>();
    }

    public void SpawnBigSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        sphere.transform.localScale = new Vector3(1f, 1f, 1f);
        player = GameObject.Find("Player");

        sphere.transform.localPosition = new Vector3(player.transform.position.x - 2f, player.transform.position.y, player.transform.position.z + 2f);
        sphere.AddComponent<Rigidbody>();
    }
}
