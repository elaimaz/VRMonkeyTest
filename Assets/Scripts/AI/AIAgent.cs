using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour {

    public Character character;
    public Transform target;
    public Transform lastTargetPosition;

    public enum StateType { idle,chasing,seeking,damaged}
    public StateType currentStateType = StateType.idle;

    IEnemyState currentState;
    public IEnemyState previousState;
    public GlobalState globalState = new GlobalState();

    public IdleState idleState= new IdleState();
    public ChaseState chasingState = new ChaseState();
    public SeekState seekingState= new SeekState();
    public IEnemyState damagedState;



    public CharacterNavigationController navAgent;
    public AISight aiSight;
    public bool alertAllies = true;
    public bool sentAlert = false;
    public float betweenAlertsDelay = 10;
    public bool canAlert = true;
    public bool canReceiveAlert = true;
    public float seePlayerDelay = 0.5f;
    public float alertRange = 20.0f;
    public float alertDelay = 0.0f;
    public AudioSource audioSource;
    public EnemyEyeController eyeController;

    public bool aiEnabled = true;

    public PatrolNode lastPatrolNode;
    public Transform patrolNodeParent;
    public Transform patrolPost;

    public ParticleSystem stunParticles;
    public Light searchLight;
    public StealthPlayerController player;

    float maxLightRange;
    float maxSightRange;

    public Renderer bodyRenderer;

    public Vector3 initialPosition;
    public Quaternion initialRotation;

    public bool chasing = false;

    bool ignoreLoseSight = false;
    public Camera cam;
    void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (lastTargetPosition == null)
        {
            lastTargetPosition = new GameObject().transform;
            lastTargetPosition.hideFlags = HideFlags.HideInHierarchy & HideFlags.HideInInspector;

        }
        eyeController = GetComponent<EnemyEyeController>();
        character = GetComponent<Character>();
        navAgent = GetComponent<CharacterNavigationController>();
        audioSource = GetComponent<AudioSource>();

        if (patrolNodeParent != null)
        {
            patrolNodeParent.transform.SetParent(transform.parent);
        }

        patrolPost = new GameObject().transform;
        patrolPost.position = transform.position;

        //aiSight = GetComponentInChildren<AISight>();
    }

    public void Restart()
    {
        Debug.Log("Restarting" + gameObject.name);
        aiSight.OnPlayerDeath();
        navAgent.Stop();
        aiSight.setSightState(AISight.SightStates.disabled);
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        Start();
        lastTargetPosition.position = transform.position;
    }

    IEnumerator restartRoutine()
    {
        aiEnabled = false;
        ignoreLoseSight = true;
        yield return new WaitForSeconds(1.0f);
        ignoreLoseSight = false;
        aiEnabled = true;
    }

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        player = StealthPlayerController.getInstance();
        currentState = idleState;
        currentState.Start(this);
        globalState.Start(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (bodyRenderer.isVisible)
        {
            searchLight.enabled = true;
        }
        else
        {
            searchLight.enabled = false;
        }

        if (GameLogic.instance.gameState != GameLogic.GameStates.gameplay || !aiEnabled)
        {
            return;
        }

        if (!character.hitStun && !character.dead)
        {
            if (currentState != null)
            {
                currentState.UpdateState();
            }
            else
            {
                globalState.UpdateState();
            }
        }
    }

    public void setState(IEnemyState newState)
    {

        navAgent.threadController.moving = false;
        currentStateType = newState.getStateType();

        if(currentStateType== StateType.idle)
        {
            Debug.Log("hm");
        }
        currentState.End();
        previousState = currentState;
        currentState = newState;
        currentState.Start(this);
    }

    public bool OnDrainStart()
    {
        if (aiSight.sightState == AISight.SightStates.seeingEnemy)
        {
            return false;
        }

        StopAllCoroutines();
        searchLight.enabled = false;
        stunParticles.Play();
        aiEnabled = false;
        StartCoroutine(DrainRoutine());
        return true;
    }

    public IEnumerator DrainRoutine()
    {
        while (character.energyLeft>0)
        {
            character.energyLeft -=Time.deltaTime * player.drainSpeed;
            player.AddEnergy(Time.deltaTime * player.drainSpeed);
            SetEnergyFraction(character.energyLeft / character.maxDrainEnergy);
            yield return null;
        }
        Debug.Log("Fully drained");
        stunParticles.Stop();
        player.DrainOver();

    }

    void SetEnergyFraction(float fraction)
    {
        bodyRenderer.material.SetColor("_EmissionColor", Color.white*fraction);
        searchLight.range = maxLightRange * fraction;
        aiSight.viewDistance = maxSightRange * fraction;

    }
    public void OnDrainEnd()
    {
        StopCoroutine("DrainRoutine");
    }
    public void OnShock(float stunTime)
    {
        StopAllCoroutines();
        searchLight.enabled = false;
        stunParticles.Play();
        StartCoroutine(ShockRoutine(stunTime));
    }

    public IEnumerator ShockRoutine(float time)
    {
        aiEnabled = false;
        yield return new WaitForSeconds(time);
        aiEnabled = true;

       // target = StealthPlayerController.getInstance().transform;
        //lastTargetPosition.position = target.position;
        searchLight.enabled = true;
        stunParticles.Stop();
        //setState(chasingState);
    }

    public void SeeEnemy(Transform enemy)
    {
        audioSource.PlayOneShot(AudioManager.getInstance().enemyAlert);

        lastTargetPosition.position = enemy.position;
        if (character.dead)
        {
            return;
        }
        if (currentState == null || !currentState.OnSeeEnemyStart(enemy))
        {
            globalState.OnSeeEnemyStart(enemy);
        }

        StartCoroutine(SeeEnemyDelayRoutine(enemy));
    }

    IEnumerator SeeEnemyDelayRoutine(Transform enemy)
    {
        yield return new WaitForSeconds(seePlayerDelay);
        if (alertAllies && canAlert)
        {
            canAlert = false;
            AlertAllies(enemy);
            StartCoroutine(BetweenDelaysRoutine());
        }

        if (currentState == null || !currentState.OnSeeEnemy(enemy))
        {
            globalState.OnSeeEnemy(enemy);
        }
    }

    IEnumerator BetweenDelaysRoutine()
    {
        yield return new WaitForSeconds(betweenAlertsDelay);
        canAlert = true;
    }

    IEnumerator BetweenReceiveAlertsDelayRoutine()
    {
        yield return new WaitForSeconds(betweenAlertsDelay);
        canReceiveAlert = true;
    }
    public void AlertAllies(Transform alertTarget)
    {
        Debug.Log(gameObject.name + " alerting allies");
        foreach (Collider col in Physics.OverlapSphere(transform.position, alertRange))
        {
            AIAgent thisAgent = col.GetComponent<AIAgent>();
            if (thisAgent != null && col.transform != transform)
            {
                thisAgent.OnReceiveAlert(alertTarget);
            }
        }
    }

    public void OnReceiveAlert(Transform newTarget)
    {
        return;
    }

    IEnumerator AlertDelayRoutine(Transform enemy)
    {
        yield return new WaitForSeconds(seePlayerDelay);
        if (alertAllies)
        {
            AlertAllies(enemy);
        }

        if (currentState == null || !currentState.OnReceiveAlert(enemy))
        {
            globalState.OnReceiveAlert(enemy);
        }
    }

    public void OnLoseSight()
    {
        if (ignoreLoseSight)
        {
            return;
        }

        if (character.dead)
        {
            return;
        }
        if (currentState == null || !currentState.OnLoseSight())
        {
            globalState.OnLoseSight();
        }
    }

    public void OnArriveAtTarget()
    {
        if (currentState == null || !currentState.OnArriveAtTarget())
        {
            globalState.OnArriveAtTarget();
        }
    }

    public Transform GetLastTargetPosition()
    {
        return lastTargetPosition;
    }
}
