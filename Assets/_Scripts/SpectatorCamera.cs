using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A lot of code by this cool dude:
/// https://www.reddit.com/r/Unity3D/comments/8k7w7v/unity_simple_mouselook/
/// </summary>
public class SpectatorCamera : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;

    Vector2 rotation = new Vector2(0, 0);

    [SerializeField]
    public float speed = 3;

    Camera cam;

    [SerializeField]
    bool camActive;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float theSpeed = moveSpeed;

        if (camActive)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                theSpeed *= 3;
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * theSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position -= transform.forward * theSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= transform.right * theSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * theSpeed * Time.deltaTime;
            }
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                rotation.y += Input.GetAxis("Mouse X");
                rotation.x += -Input.GetAxis("Mouse Y");
                transform.eulerAngles = rotation * speed;
            }
        }

        if (Input.anyKeyDown)
        {
            if (cam.depth != 1)
            {
                camActive = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
                camActive = true;
        }

        if (Input.GetMouseButtonDown(0) && cam.depth == 1)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            camActive = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
