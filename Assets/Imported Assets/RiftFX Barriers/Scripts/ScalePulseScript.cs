using UnityEngine;

namespace Assets.RiftFX_Barriers.Scripts
{
    /*
     * Description: Slightly alters the scale of an object in a pulsating fashion 
     */
    public class ScalePulseScript : MonoBehaviour
    {
        public bool ScaleX = true;
        public bool ScaleY = false;
        public bool ScaleZ = true;
        public float Dampening = 0.5f;
        public float Speed = 1f;

        private Vector3 m_initialScale;

        // Use this for initialization
        void Start ()
        {
            // store the initial scale of the object
            m_initialScale = transform.localScale;
        }
	
        // Update is called once per frame
        void Update ()
        {
            float pulse = Mathf.Sin(Time.time * Speed) * Dampening;

            // add the pulse value to the objects initial scale and set the new scale equal to this value 
            transform.localScale = m_initialScale + new Vector3(ScaleX ? pulse : 0, ScaleY ? pulse : 0, ScaleZ ? pulse : 0);
        }
    }
}
