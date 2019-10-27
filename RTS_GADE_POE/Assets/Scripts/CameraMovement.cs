using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        CameraMove();
        CameraZoom();
        CameraTurn();
    }

    void CameraZoom()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime * 10, 0);
        }
        else if (Input.GetKey(KeyCode.E))
        {

            transform.position += new Vector3(0, speed * Time.deltaTime * 10, 0);
        }
    }

    void CameraMove()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        transform.position += new Vector3(speed * Time.deltaTime * horizontal, 0, speed * Time.deltaTime * vertical);
    }

    void CameraTurn()
    {
        if (Input.GetKey(KeyCode.R))
        {
            transform.Rotate(Vector3.forward, 50f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.F))
        {
            transform.Rotate(Vector3.back, 50f * Time.deltaTime);
        }
    }
}
