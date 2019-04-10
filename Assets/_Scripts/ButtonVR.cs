using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Very hardcoded, physics-based button press
/// </summary>
public class ButtonVR : MonoBehaviour {

    [SerializeField]
    Rigidbody buttonBody;

    [SerializeField]
    float unpressSpeed;

    bool pressed;

    public UnityEvent onButtonPress;

	// Use this for initialization
	void Start () {
		
	}

    private void FixedUpdate()
    {
        buttonBody.transform.localPosition = new Vector3(0, buttonBody.transform.localPosition.y, 0);
    }

    // Update is called once per frame
    void Update() {
        if (buttonBody.transform.localPosition.y < 0)
        {
            buttonBody.velocity = unpressSpeed * buttonBody.transform.up;
            //buttonBody.MovePosition(buttonBody.transform.localPosition + buttonBody.transform.right * unpressSpeed * Time.fixedDeltaTime);
        }
        else if (buttonBody.transform.localPosition.y > 0)
        {
            buttonBody.transform.localPosition = Vector3.zero;
        }

        if (buttonBody.transform.localPosition.y < -4)
        {
            if (!pressed)
            {
                onButtonPress.Invoke();
                pressed = true;
            }
        }
        else
        {
            pressed = false;
        }

        if (buttonBody.transform.localPosition.y <= -4.5f)
        {
            buttonBody.transform.localPosition = new Vector3(0, -4.5f, 0);
        }
	}
}
