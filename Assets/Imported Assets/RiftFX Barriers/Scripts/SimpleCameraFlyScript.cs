using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.RiftFX_Barriers.Scripts
{
    /*
     * Description: A basic script to allow the user to freely move around the scene.
     */
    public class SimpleCameraFlyScript : MonoBehaviour
    {
        // mouse look speed
        public float LookSpeedX = 80f;
        public float LookSpeedY = 80f;

        // camera movement speed
        public float MoveSpeed = 10f;

        // Use this for initialization
        void Start ()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
	
        // Update is called once per frame
        void Update ()
        {
            // lock / unlock the mouse with R key
            if (Input.GetKeyUp(KeyCode.R))
            {
                // if it's locked, unlock it and make cursor visible
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                // if it's not locked, lock it and hide the cursor
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }

            // only update the camera if the mouse is locked
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // get x and y values for movement
                float x = CrossPlatformInputManager.GetAxis("Vertical") * MoveSpeed * Time.deltaTime;
                float y = CrossPlatformInputManager.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime;

                // get the mouse delta
                float h = Input.GetAxis("Mouse X") * LookSpeedX * Time.deltaTime;
                float v = Input.GetAxis("Mouse Y") * LookSpeedY * Time.deltaTime;

                // calculate the new movement direction 
                var direction = ((transform.forward * x) + (transform.right * y));

                // move the camera in the calculated direction
                transform.Translate(transform.InverseTransformDirection(direction));

                // rotate the camera based on mouse x and y delta values 
                var rot = Quaternion.AngleAxis(h, Vector3.up) * Quaternion.AngleAxis(-v, transform.right) * transform.rotation;
                transform.rotation = rot;
            }            
        }
    }
}
