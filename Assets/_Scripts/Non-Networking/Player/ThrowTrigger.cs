using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ThrowTrigger : MonoBehaviour
{
    public enum ThrowStates { OnCD, Ready, Charging, Throwing }
    [SerializeField]
    int maxFootballs = 10;

    [SerializeField]
    int currentFootballs = 2;

    [SerializeField]
    float throwCD = 0;
    [SerializeField]
    float throwCDtime = 10;
    [SerializeField]
    GameObject throwable;

    [SerializeField]
    float chargeSpeed = 1;

    [SerializeField]
    float baseSpeed = 5;

    [SerializeField]
    float maxCharge = 2;

    [SerializeField]
    float charge = 0.3f;

    [SerializeField]
    float speed = 3;

    [SerializeField]
    InputManager inputManager;

    [SerializeField]
    Camera cam;

    [SerializeField]
    ThrowStates states;

    [SerializeField]
    float segmentScale = 1.0f;

    [SerializeField]
    int numOfVertices = 10;

    [SerializeField]
    LineRenderer line;

    [SerializeField]
    GameObject targetPrefab;

    GameObject targetBall;

    [SerializeField]
    bool hitSomething;

    private Collider _hitObject;

    public Collider hitObject { get { return _hitObject; } }
    [SerializeField]
    GameObject collidedObj;
    [SerializeField]
    Vector3 launchAngle;

    [SerializeField]
    float startVecpercent = 0.0f;

    [SerializeField]
    float endVecpercent = 1.0f;

    [SerializeField]
    float targetOffset = 0.04f;

    Vector3 _startVec;

    Vector3 _endVec;

    [SerializeField]
    float _localtargetrot = 0.0f;

    [SerializeField]
    float rotSpeed = 70f;

    [SerializeField]
    float minDist = 1.0f;

    [SerializeField]
    float maxDist = 60.0f;

    [SerializeField]
    string test;

    GameObject _footballDoll;

    public string currentstate { get { return states.ToString(); } set { SetState(value); } }
    bool targetRender { get { return targetBall.GetComponent<Renderer>().enabled; } set { targetBall.GetComponent<Renderer>().enabled = value; } }

    [SerializeField]
    LayerMask mask;

    GameObject chad;
    Animator _anim;
    AnimationManager _animM;
    AudioManager _am;
    PlayerManager _pm;

    [SerializeField]
    GameObject sonicBoom;

    bool _canThrow = true;
    bool _pickupEnabled = true;

    [SerializeField]
    FootballFactory factory;

    [SerializeField]
    Text footballText;

    [SerializeField]
    Image footballSprite;


    public bool canThrowBomb { get { return _canThrow; } set { _canThrow = value; } }


    // Start is called before the first frame update
    void Start()
    {
        UpdateFootballCount();

        //Instantiat our target sprite and hide it
        targetBall = Instantiate(targetPrefab);
        targetRender = false;

        chad = GameObject.Find("Chad");
        _anim = chad.GetComponent<Animator>();
        _pm = chad.GetComponent<PlayerManager>();

        _animM = GetComponentInParent<AnimationManager>();

        _am = AudioManager.GetInstance();

        _footballDoll = GameObject.Find("football_doll");
        _footballDoll.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        test = currentstate;
        if (IsOnCD())//if is on cooldown
        {
            DecreaseCD();//Decrease cooldown timmer
            if (OffCoolDown())//if no longer on cooldown
                SetState("Ready");//set the state to ready, which means the throw can be used again
        }
        if (IsCharging())//if in charging state
        {

            if (line.enabled == false)//enables the line render if it was disabled before
                line.enabled = true;
            ChargeUp(chargeSpeed * Time.fixedDeltaTime);//charge up our speed and distance
            SimulateTrajectory();//simulate the flight path of the ball
            if (!_canThrow)
                CancelThrow();
        }
    }
    public void CancelThrow()
    {
        if (IsCharging())
        {
            targetRender = false;
            line.enabled = false;

            throwCD = throwCDtime;
            SetState("Ready");
            charge = 0;

            _animM.EndThrow();
            _footballDoll.SetActive(false);
        }
    }

    void DisarmChad()
    {
        _canThrow = false;
    }

    void RearmChad()
    {
        _canThrow = true;

    }
    void SimulateTrajectory()//Flight path simulator
    {

        Vector3[] _positions = new Vector3[numOfVertices];//array of all the verticies along the flight path
        _positions[0] = transform.position;//set the first on to the launch position

        Vector3 _segVel = _lanchVector();// the initial velocity

        _hitObject = null;//set to null incase something is not hit, it will return nothing
        hitSomething = false;
        for (int i = 1; i < numOfVertices; i++)
        {
            float segmentTime = (_segVel.sqrMagnitude != 0) ? segmentScale / _segVel.magnitude : 0;//how much time the segment simulates, set to zero if the football velocity is zero

            _segVel = (_segVel + (Physics.gravity * segmentTime));//update the new segement velocity base on previous velocity plus gravity
            RaycastHit ray;
            if (Physics.SphereCast(_positions[i - 1], 0.42f, _segVel, out ray, segmentScale, mask))//sphere casting since the thrown object is a sphere
            {

                _hitObject = ray.collider;//the collider of the hit object
                collidedObj = _hitObject.gameObject;//the hit object itself
                                                    // _positions[i] = _positions[i - 1] + _segVel.normalized * ray.distance;//update the new position of the new verticie
                _positions[i] = ray.point;
                for (int n = i; n < numOfVertices; n++)
                {
                    _positions[n] = _positions[i];//all of the remaining uncalculated positions are sent to the collision spot
                }
                targetBall.transform.position = _positions[i] + (ray.normal * targetOffset);
                targetBall.transform.up = ray.normal;
                _localtargetrot += rotSpeed * Time.fixedDeltaTime;
                targetBall.transform.RotateAround(_positions[i] + (ray.normal * targetOffset), ray.normal, _localtargetrot);
                hitSomething = true;
                i = numOfVertices + 1;

            }
            else
            {
                _positions[i] = _positions[i - 1] + _segVel * segmentTime;
            }
        }
        if (hitSomething)
        {
            if (targetRender == false)
                targetRender = true;
        }
        else if (!hitSomething && targetRender == true)
        {
            targetRender = false;

        }

        line.positionCount = numOfVertices;
        for (int i = 0; i < numOfVertices; i++)
            line.SetPositions(_positions);


    }

    void ChargeUp(float c)
    {
        if (charge <= maxCharge)
            charge += c;
        Vector3 _highAngle;
        Vector3 _lowAngle;
        float _dist = Mathf.Lerp(minDist, maxDist, charge);
        float _properspeed = Mathf.Lerp(baseSpeed, speed, charge);
        Vector3 _intersectionPoint = transform.position + (cam.transform.forward * _dist);
        int solutions = AppeaseJoss(transform.position, _properspeed, _intersectionPoint, -Physics.gravity.y, out _lowAngle, out _highAngle);
        launchAngle = _lowAngle;

    }

    public void ThrowFootball()
    {
        _canThrow = _pm.IsPlayerControllable();

        if (IsCharging() && _canThrow)
        {
            _footballDoll.SetActive(false);
            targetRender = false;
            line.enabled = false;
            throwCD = throwCDtime;
            SetState("OnCD");
            Throw();
            charge = 0;
            _am.PlaySoundOnce(AudioManager.Sound.ChadThrow, transform);
            _animM.EndThrow();

            currentFootballs--;
            UpdateFootballCount();
        }
    }

    public void AttemptChargeFootball()
    {
        _canThrow = _pm.IsPlayerControllable();

        if (IsReady() && currentFootballs > 0 && _canThrow && !IsCharging())
        {
            _footballDoll.SetActive(true);
            SetState("Charging");
            _animM.BeginThrow();


            charge = 0;
        }
    }

    void SetState(string s)
    {

        states = (ThrowStates)System.Enum.Parse(typeof(ThrowStates), s);

    }

    public bool IsCharging()
    {
        if (states.Equals(ThrowStates.Charging))
            return true;
        else
            return false;
    }

    bool IsReady()
    {

        if (states.Equals(ThrowStates.Ready))
            return true;
        else
            return false;
    }

    bool IsOnCD()
    {

        if (states.Equals(ThrowStates.OnCD))
            return true;
        else
            return false;
    }

    void DecreaseCD()
    {

        throwCD -= Time.deltaTime;
    }

    void Throw()
    {
        SpawnThrowable();

    }

    int AppeaseJoss(Vector3 initialPos, float speed, Vector3 target, float gravity, out Vector3 lowAngleVec, out Vector3 highAngleVec)
    {
        lowAngleVec = Vector3.zero;
        highAngleVec = Vector3.zero;

        Vector3 _difference = target - initialPos;
        Vector3 _twoDimensionalDifference = new Vector3(_difference.x, 0.0f, _difference.z);
        float _ground = _twoDimensionalDifference.magnitude;

        float _speedSquared = speed * speed;
        float _speedQuarted = speed * speed * speed * speed;
        float y = _difference.y;
        float x = _ground;
        float _gravityX = gravity * x;

        float _root = _speedQuarted - gravity * (gravity * x * x + 2 * y * _speedSquared);


        //No solution found
        if (_root < 0)
            return 0;

        _root = Mathf.Sqrt(_root);

        float _low = Mathf.Atan2(_speedSquared - _root, _gravityX);
        float _high = Mathf.Atan2(_speedSquared + _root, _gravityX);

        int _numOfSolutions = _low != _high ? 2 : 1;

        Vector3 _groundDirection = _twoDimensionalDifference.normalized;

        lowAngleVec = _groundDirection * Mathf.Cos(_low) * speed + Vector3.up * Mathf.Sin(_low) * speed;
        if (_numOfSolutions > 1)
            highAngleVec = _groundDirection * Mathf.Cos(_high) * speed + Vector3.up * Mathf.Sin(_high) * speed;

        return _numOfSolutions;
    }

    void SpawnThrowable()
    {
        GameObject _tempThrowable;
        _tempThrowable = Instantiate(throwable, transform.position, cam.transform.rotation);
        FootBomb _bomb = _tempThrowable.GetComponent<FootBomb>();
        _bomb.Boost(_lanchVector());
        Instantiate(sonicBoom, _bomb.transform.position, _bomb.transform.rotation);
        // sonicBoom.SetActive(true);
        // sonicBoom.transform.position = _bomb.transform.position;
        // sonicBoom.transform.rotation = _bomb.transform.rotation;
        //sonicBoom.GetComponentInChildren<ParticleSystem>().Play();
    }

    Vector3 _lanchVector()
    {
        return (launchAngle);
    }

    bool OffCoolDown()
    {
        return throwCD <= 0;
    }

    public float GetCharge()
    {
        return charge;
    }

    public void EnableNet()
    {
        _pickupEnabled = true;
    }

    public void DisableNet()
    {
        _pickupEnabled = false;
    }

    public bool PickUpFootball(int balls=1)
    {
        if (currentFootballs == maxFootballs)
        {
            return false;
        }
        if (currentFootballs+balls < maxFootballs)
        {
            currentFootballs+=balls;
            footballSprite.GetComponent<Animator>().SetTrigger("PingTrigger");
            factory.PickedUp();
            UpdateFootballCount();
            return true;
        }
        else if (currentFootballs + balls >= maxFootballs)
        {
            currentFootballs=maxFootballs;
            footballSprite.GetComponent<Animator>().SetTrigger("PingTrigger");
            factory.PickedUp();
            UpdateFootballCount();
            return true;
        }
      
        return false;
    }

    void UpdateFootballCount()
    {
        if (currentFootballs == 0)
            footballText.text = "<color=red>" + currentFootballs + "</color>";
        else
        footballText.text = "" + currentFootballs;
    }
}

