using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script handles facial expressions

public class AtomicEagleExpressionStates : MonoBehaviour
{

   private enum SHAPE_KEYS
    {
        BLINK = 0,
        CLOSE_LEFT_EYE,
        SNEER,
        FURROWED_BROW,
        SERIOUS_MOUTH,
        SMILE,
        GRIMACE,
        NUM_SHAPE_KEYS
    }

    [Header("Expression weights")]
    [SerializeField]
    private float[] normalState = new float[(int)SHAPE_KEYS.NUM_SHAPE_KEYS];

    [SerializeField]
    private float[] painState = new float[(int)SHAPE_KEYS.NUM_SHAPE_KEYS];
    

    public enum EXPRESSION_STATE
    {
        NEUTRAL = 0,
        //SERIOUS,
        PAIN,
        NUM_EXPRESSIONS
    }

    private EXPRESSION_STATE state;
    private float[] targetStateWeights = new float[(int)SHAPE_KEYS.NUM_SHAPE_KEYS];

    float expressionTimer;

    //[Header("Expression weights")]
    //[SerializeField]
    //private float[,] expressions = new float[(int)EXPRESSION_STATE.NUM_EXPRESSIONS, (int)SHAPE_KEYS.NUM_SHAPE_KEYS];

    [Header("Blinking")]
    [SerializeField]
    public Vector2 blinkIntervalRange = new Vector2(0.5f, 6.0f); // seconds between blinks

    [SerializeField]
    public float blinkDuration = 0.5f; // how long a blink lasts

    private float blinkState = 0.0f; // 0 to 100%
    private float blinkTimer = 0;
    private float blinkInterval;

    private SkinnedMeshRenderer faceMesh;

	// Use this for initialization
	void Start ()
    {
        state = EXPRESSION_STATE.NEUTRAL;
        faceMesh = GetComponent<SkinnedMeshRenderer>();
        //expressions = new float[(int)EXPRESSION_STATE.NUM_EXPRESSIONS, (int)SHAPE_KEYS.NUM_SHAPE_KEYS];
        //expressions[EXPRESSION_STATE.NEUTRAL] = normalState;
        //expressions[EXPRESSION_STATE.PAIN] = painState;

        //expressions[]// = new { normalState, painState };

        SetState(EXPRESSION_STATE.NEUTRAL);
    }

    public void SetState(EXPRESSION_STATE newState)
    {
        state = newState;
        //Debug.Log("expression set to " + state);
         
        //for (int i = (int)SHAPE_KEYS.BLINK + 1; i < (int)SHAPE_KEYS.NUM_SHAPE_KEYS; i++)
        //{
        //    float weightForShapeKey = expressions[(int)state, i];
        //    Debug.Log((SHAPE_KEYS)i + "weight: " + weightForShapeKey);
        //    faceMesh.SetBlendShapeWeight(i, weightForShapeKey);
        //}

       switch(state)
       {
           case EXPRESSION_STATE.NEUTRAL:
               {
                   for (int i = (int)SHAPE_KEYS.BLINK + 1; i < normalState.Length; i++)
                   {
                       float weightForShapeKey = normalState[i];
                       faceMesh.SetBlendShapeWeight(i, weightForShapeKey);
                   }
                   break;
               }
       
           case EXPRESSION_STATE.PAIN:
               {
                   for (int i = (int)SHAPE_KEYS.BLINK + 1; i < painState.Length; i++)
                   {
                       float weightForShapeKey = painState[i];
                       faceMesh.SetBlendShapeWeight(i, weightForShapeKey);
                   }
                   break;
               }
       }
    }

    public IEnumerable TakeDamageResponseCoroutine()
    {
        SetState(EXPRESSION_STATE.PAIN);
        yield return new WaitForSeconds(5.0f);
        SetState(EXPRESSION_STATE.NEUTRAL);
    }

    public void TakeDamageResponse()
    {
        //StartCoroutine("TakeDamageResponseCoroutine");
        expressionTimer = 4.0f;
        SetState(EXPRESSION_STATE.PAIN);
    }

    public IEnumerable TakeDamageResponse(float time)
    {
        SetState(EXPRESSION_STATE.PAIN);
        yield return new WaitForSeconds(time);
        SetState(EXPRESSION_STATE.NEUTRAL);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            SetState((EXPRESSION_STATE)((((int)state) + 1) % (int)(EXPRESSION_STATE.NUM_EXPRESSIONS)));
        }

        // tick blink timer
        blinkTimer = Mathf.Max(0.0f, blinkTimer - Time.deltaTime);

        if (blinkTimer <= 0)
        {
            // blink
            blinkInterval = Random.Range(blinkIntervalRange.x, blinkIntervalRange.y);
            blinkState = 100.0f;
            blinkTimer = blinkDuration + blinkInterval;
        }

        blinkState = UtilMath.Lmap(blinkTimer, blinkDuration + blinkInterval, blinkInterval, 100.0f, 0.0f);
        faceMesh.SetBlendShapeWeight((int)SHAPE_KEYS.BLINK, blinkState);

        // tick expression timer
        expressionTimer = Mathf.Max(0.0f, expressionTimer - Time.deltaTime);

        if (expressionTimer == 0)
        {
            SetState(EXPRESSION_STATE.NEUTRAL);
        }
    }
}
