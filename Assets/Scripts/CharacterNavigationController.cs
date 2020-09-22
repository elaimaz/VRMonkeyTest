using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterNavigationController : MonoBehaviour {

    AIAgent aiAgent;
    NavMeshAgent navMeshAgent;
    Transform currentTarget;
    Vector3 flatTargetPosition;
    public float stoppingDistance = 0.5f;
    bool moving = false;

    public bool enableRotation = true;
    public bool rotating = false;
    public bool rotateOnlyY = false;
    bool rotationCloseEnough = false;
    public bool instantTurn = false;
    public float maxRotationDelta = 15;
    float rotationDelta = 180;
    public float angularSpeed = 10;

    float stuckTime = 2.0f;
    float currentStuckTimer = 0;

    public RobotThreadController threadController;
    public float maxThreadSpeed = 0.1f;
    public float maxMoveSpeed = 5.0f;


    public void SetTarget(Transform newTarget,float targetSpeed,float distanceToStop,bool onlyRotate=false)
    {
        currentStuckTimer = 0;
        if (targetSpeed != 0)
        {
            navMeshAgent.speed = targetSpeed;
        }
        currentTarget = newTarget;
        
        navMeshAgent.SetDestination(currentTarget.position);

        navMeshAgent.stoppingDistance = distanceToStop;

        if (!onlyRotate)
        {
            navMeshAgent.isStopped = false;
            moving = true;
        }
        else
        {
            navMeshAgent.isStopped = true;
            //moving =false;
        }
        enableRotation = true;
    }

	// Use this for initialization
	void Awake () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        aiAgent = GetComponent<AIAgent>();
	}
	


	// Update is called once per frame
	void Update () {
        if (moving)
        {
            threadController.moving = true;
            threadController.speed = (navMeshAgent.velocity.magnitude / maxMoveSpeed) * (maxThreadSpeed);
        }
        else
        {
            threadController.moving = false;
        }
        


        if (currentTarget == null || !moving)
        {
            return;
        }


        Rotate();

        if (navMeshAgent.velocity.magnitude < 0.5f && navMeshAgent.speed>0.1f)
        {
            currentStuckTimer += Time.deltaTime;
            if (currentStuckTimer > stuckTime)
            {
                Debug.Log("Stuck!");
                ArriveAtTarget();
            }
        }
        Vector3 flatTargetPosition = new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z);
        float distanceToTarget = Vector3.Distance(transform.position, flatTargetPosition);

        if (distanceToTarget > 0.2f)
        {
            Rotate();
        }

        
        if (navMeshAgent.isActiveAndEnabled && distanceToTarget< stoppingDistance)
        {
            ArriveAtTarget();
        }
	}

    public void Stop()
    {
        moving = false;
        navMeshAgent.isStopped = true;
    }

    void ArriveAtTarget()
    {
        moving = false;
        if (aiAgent != null)
        {
            aiAgent.OnArriveAtTarget();
        }
    }

    

    void Rotate()
    {
        rotationCloseEnough = true;
        //if (pathfindingMode == PathFindingMode.pathNodes) { 
        if (true)
        {
            if (instantTurn)
            {
                transform.LookAt(currentTarget);
            }
            else
            {
                Quaternion currentRotation = transform.rotation;
                transform.LookAt(currentTarget);
                if (rotateOnlyY)
                {

                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                }

                Quaternion targetRotation = transform.rotation;
                transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, angularSpeed * Time.deltaTime);
                float rotationDelta = Quaternion.Angle(transform.rotation, targetRotation);

                

                if (rotationDelta >= maxRotationDelta)
                {
                    rotationCloseEnough = false;
                }
            }
        }
        else
        {
            if (rotationDelta >= maxRotationDelta)
            {
                rotationCloseEnough = false;
            }
        }
    }
}
