using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{

    [SerializeField]
    Vector2 screenPos;

    [SerializeField]
    Text text;

    SpeechBubble(float duration)
    {
        Duration = duration;
    }

    [SerializeField]
    Camera cam;

    public float Duration { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Duration > 0.0f)
        // {
        //     if (cam)
        //     {
        //         transform.LookAt(cam.transform.position, Vector3.up);
        //
        //
        //     } else
        //     {
        //         transform.LookAt(Camera.main.transform.position, Vector3.up);
        //     }
        //
        //     Duration -= Time.deltaTime;
        // }

        screenPos = ToScreenSpace(cam);

        // Bound screenspace position
       screenPos.y = Mathf.Min(1.0f, screenPos.y);
       screenPos.x = Mathf.Min(1.0f, screenPos.x);

       screenPos.y = Mathf.Max(-1.0f,    screenPos.y);
       screenPos.x = Mathf.Max(-1.0f,    screenPos.x);

        text.rectTransform.position = screenPos;
        //text.rectTransform.position = screenPos;
    }

    Vector2 ToScreenSpace(Camera whichCam)
    {
        // glm::mat4 viewProj = ViewManager::getInstance()->getViewProjection();
        // glm::vec4 pos = glm::vec4(worldspacePosition, 1.0);
        // pos = viewProj * pos;
        // pos /= pos.w;
        // glm::vec2 screenspacePos = glm::vec2(pos.x, pos.y);
        // screenspacePos += glm::vec2(1.0);
        // screenspacePos /= 2.0f;
        // auto screenDimensions = Window::getScreen();
        //
        // screenspacePos.x *= screenDimensions.right;
        // screenspacePos.y *= screenDimensions.bottom;
        // return screenspacePos;

        Vector4 worldPos = transform.localToWorldMatrix.GetColumn(3);
        //cam.projectionMatrix
        worldPos = whichCam.previousViewProjectionMatrix * worldPos;
        worldPos /= worldPos.w;

        Vector2 screenSpacePos = new Vector2(worldPos.x, -worldPos.y);

        return screenSpacePos;
    }
}
