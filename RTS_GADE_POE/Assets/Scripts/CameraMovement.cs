using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float ZoomAmount = 0; //With Positive and negative values
    private float MaxToClamp = 10;
    private float ROTSpeed = 10;
    private Vector3 lastPosition;
    [SerializeField] float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        MouseZoom();
        KeyboardZoom();
        CameraTurn();
        MousePan();
    }

    void KeyboardZoom()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime * 5, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {

            transform.position += new Vector3(0, speed * Time.deltaTime * 5, 0);
        }
    }

    void MouseZoom()
    {
        ZoomAmount += Input.GetAxis("Mouse ScrollWheel");
        ZoomAmount = Mathf.Clamp(ZoomAmount, -MaxToClamp, MaxToClamp);
        var translate = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")), MaxToClamp - Mathf.Abs(ZoomAmount));
        gameObject.transform.Translate(0, 0, translate * ROTSpeed * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")));
    }
   

    void CameraTurn()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward, 50f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.back, 50f * Time.deltaTime);
        }
    }

    void MousePan()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            transform.Translate(delta.x * -0.01f, delta.y * -0.01f, 0);
            lastPosition = Input.mousePosition;
        }
    }
}
