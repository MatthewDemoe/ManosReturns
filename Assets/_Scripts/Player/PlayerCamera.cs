using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    GameObject targetPlayer;

    [SerializeField]
    GameObject pivot;

    [SerializeField]
    InputManager inputManager;

    [SerializeField]
    float followDistance = 5.0f;
    float _avoidDistance;

    [SerializeField]
    float height = 1.0f;

    [SerializeField]
    float LerpSpeed = 1.0f;

    [SerializeField]
    float ySensitivity = 1.5f;

    [SerializeField]
    float maxHeight = 5.0f;

    [SerializeField]
    float maxUpTilt = 400.0f;

    [SerializeField]
    float maxDownTilt = 400.0f;

    float minHeight = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        transform.forward = targetPlayer.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        float _posX = Mathf.Lerp(transform.position.x, targetPlayer.transform.position.x - (followDistance * targetPlayer.transform.forward.x), Time.deltaTime * LerpSpeed);
        float _posY = Mathf.Lerp(transform.position.y, targetPlayer.transform.position.y + height, Time.deltaTime * LerpSpeed);
        float _posZ = Mathf.Lerp(transform.position.z, targetPlayer.transform.position.z - (followDistance * targetPlayer.transform.forward.z), Time.deltaTime * LerpSpeed);

        pivot.transform.position = new Vector3(_posX, _posY, _posZ);

    }

    public void Rotate(float horizontal, float sensitivity)
    {
        //Up/Down Rotation
        float _rot = inputManager.GetRStick().y;

        pivot.transform.Rotate(0.0f, horizontal * sensitivity, 0.0f);

        if (_rot < 0.0f)
        {
            if (height <= maxHeight)
            {
                height -= _rot / LerpSpeed;
                //transform.rotation = Quaternion.Euler(Mathf.Lerp(transform.rotation.x, maxUpTilt, Time.deltaTime), 0.0f, 0.0f);
                transform.Rotate(-_rot * ySensitivity * Time.deltaTime, 0.0f, 0.0f);
            }
        }

        else if (_rot > 0.0f)
        {
            if (height > minHeight)
            {
                height -= _rot / LerpSpeed;
                //transform.rotation = Quaternion.Euler(Mathf.Lerp(transform.rotation.x, maxDownTilt, Time.deltaTime), 0.0f, 0.0f);
                transform.Rotate(-_rot * ySensitivity * Time.deltaTime, 0.0f, 0.0f);
            }
        }


    }
}
