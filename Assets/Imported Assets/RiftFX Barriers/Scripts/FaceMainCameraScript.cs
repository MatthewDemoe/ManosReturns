using UnityEngine;

namespace Assets.RiftFX_Barriers.Scripts
{
    /*
     * Description: Simple script to make this object always point toward the main camera
     */
    [ExecuteInEditMode]
    public class FaceMainCameraScript : MonoBehaviour
    {
        void Start()
        {
            Camera.main.depthTextureMode = DepthTextureMode.Depth;
        }

        // Update is called once per frame
        void Update ()
        {
            // get the main camera 
            var mainCamera = Camera.main;

            // make sure it's not null before we use it
            if (mainCamera != null)
                // make our transform point toward the main camera's position
                transform.LookAt(transform.position + Vector3.Normalize(transform.position - mainCamera.transform.position), Vector3.up);
        }
    }
}
