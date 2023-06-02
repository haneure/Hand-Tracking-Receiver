using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    CharacterController controller;
    private Vector3 v_movement;
    private Vector3 v_velocity;
    private float rotateX;
    private float rotateY;
    private float inputX;
    private float inputZ;
    public float speed = 5f;
    public float gravity = 1f;
    

    public bool handMode = false;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // gravity
        if (controller.isGrounded)
        {
            v_velocity.y = 0f;
        } else
        {
            v_velocity.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (handMode == false)
            {
                gameObject.transform.GetChild(0).transform.Find("LeftHand").gameObject.SetActive(true);
                gameObject.transform.GetChild(0).transform.Find("RightHand").gameObject.SetActive(true);
            } else
            {
                gameObject.transform.GetChild(0).transform.Find("LeftHand").gameObject.SetActive(false);
                gameObject.transform.GetChild(0).transform.Find("RightHand").gameObject.SetActive(false);
            }
        }
       
        inputZ = Input.GetAxis("Vertical");

        // Rotate Left Right
        if (Input.GetKey(KeyCode.Q))
        {
            rotateX = -1f;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            rotateX = 0f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotateX = 1f;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            rotateX = 0f;
        }

        // Rotate Up Down
        if (Input.GetKey(KeyCode.Z))
        {
            rotateY = -1f;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            rotateY = 0f;
        }

        if (Input.GetKey(KeyCode.X))
        {
            rotateY = 1f;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            rotateY = 0f;
        }

        float inputX = Input.GetAxis("Horizontal");
        v_movement = (controller.transform.forward * inputZ + controller.transform.right * inputX);
        controller.transform.Rotate(Vector3.up * rotateX * (100f * Time.deltaTime));
        controller.transform.Rotate(Vector3.right * rotateY * (100f * Time.deltaTime));

        controller.Move(v_movement * speed * Time.deltaTime);
        controller.Move(v_velocity);

        //controller.Move(new Vector3(inputX, 0, 0) * speed * Time.deltaTime);


        //Vector3 v_Input = new Vector3(0, 0, Input.GetAxis("Vertical"));
        //Vector3 h_Input = new Vector3(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        //Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //rb.MovePosition(transform.position + m_Input * Time.deltaTime * speed);

        //Quaternion deltaRotation = Quaternion.Euler(h_Input * Time.fixedDeltaTime);
        //transform.rotation = transform.rotation * deltaRotation;
        //rb.MoveRotation(rb.rotation * deltaRotation);
        //rb.velocity = new Vector3(x, rb.velocity.y, z) * speed;
    }
}
