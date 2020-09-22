using UnityEngine;
using System.Collections;
[System.Serializable]
public class SeekState : IEnemyState
{
    private AIAgent agent;

    
    public enum Substate { starting,chasing,lookingForPlayer,lookingAround,postLookLag}
    public Substate substate = Substate.starting;
    public float currentTimer = 0;

    public float seeEnemyDelay = 0.5f;
    public float chaseSpeed = 6.0f;
    public float lookForPlayerTime = 1.5f;

    Quaternion initialLookAroundRotation;
    public float lookAroundTime = 1;
    public float lookAroundAngle = 90;
    public bool lookAroundDirection = true;
    public float postLookLag = 0.5f;

    public AIAgent.StateType getStateType()
    {
        return AIAgent.StateType.seeking;
    }

    public void Start(AIAgent newAgent)
    {
        currentTimer = 0;
        agent = newAgent;
        agent.navAgent.Stop();
        SetSubstate(Substate.starting);
    }

    public void SetSubstate(Substate newSubstate)
    {
        substate = newSubstate;
        currentTimer = 0;

        switch (substate)
        {
            case Substate.chasing:
                agent.navAgent.SetTarget(agent.lastTargetPosition, chaseSpeed,0.25f);
                break;
            case Substate.lookingForPlayer:
                agent.eyeController.SetQuestion();
                break;
            case Substate.lookingAround:
                
                initialLookAroundRotation = agent.transform.rotation;
                lookAroundDirection = true;
                //Debug.Log("looking around");
                break;
        }
    }

    public void UpdateState()
    {
        currentTimer += Time.deltaTime;

        switch (substate)
        {
            case Substate.starting:
                if (currentTimer >= seeEnemyDelay)
                {
                    SetSubstate(Substate.chasing);
                }
                break;
            case Substate.chasing:

                break;
            case Substate.lookingForPlayer:
                if (currentTimer >= lookForPlayerTime)
                {
                    SetSubstate(Substate.lookingAround);
                }
                break;
            case Substate.lookingAround:
                Quaternion targetRotation = initialLookAroundRotation;
                if (lookAroundDirection)
                {
                    targetRotation = targetRotation * Quaternion.AngleAxis(lookAroundAngle,Vector3.up);
                }
                else
                {
                    targetRotation = targetRotation * Quaternion.AngleAxis(-2*lookAroundAngle, Vector3.up);
                }
                agent.navAgent.threadController.moving = true;
                agent.navAgent.threadController.speed = 0.5f;

                agent.transform.rotation = Quaternion.Lerp(initialLookAroundRotation, targetRotation, currentTimer / lookAroundTime);

                if (Quaternion.Angle(agent.transform.rotation, targetRotation) < 5)
                {
                    if (lookAroundDirection)
                    {
                        //Debug.Log("looking the other way");
                        lookAroundDirection = false;
                        initialLookAroundRotation = agent.transform.rotation;
                        currentTimer = 0;
                    }
                    else
                    {
                        agent.navAgent.threadController.moving = false;
                        SetSubstate(Substate.postLookLag);
                    }
                }
                break;

            case Substate.postLookLag:
                if (currentTimer > postLookLag)
                {
                    agent.setState(agent.idleState);
                }
                break;
        }
    }

    public void End()
    {

    }
    public bool OnTriggerEnter(Collider other)
    {
        return false;
    }


    public bool OnLoseSight()
    {
        //agent.setState(agent.seekingState);
        return false;
    }

    public bool OnSeeEnemyStart(Transform enemy)
    {
        return false;
    }

    public bool OnSeeEnemy(Transform enemy)
    {
       // agent.audioSource.PlayOneShot(AudioManager.getInstance().enemyAlert);
        agent.target = enemy.transform;
        agent.GetLastTargetPosition().position = enemy.transform.position;
        agent.setState(agent.chasingState);
        return true;
    }

    public bool OnDamage(Transform attacker)
    {
        if (attacker != null && agent.target!=null)
        {
            agent.target = attacker;
            agent.GetLastTargetPosition().position = attacker.position;
        }
        //agent.eyeController.SetDamaged();
        return true;
    }


    public bool OnKnockbackStart()
    {
        agent.character.SetState(Character.States.hitStun);
       // agent.navAgent.StopAllMovement();
        agent.setState(agent.damagedState);
        return false;
    }

    public bool OnKnockbackEnd()
    {
       // agent.eyeController.SetNormal();
        if (agent.aiSight.seenCharacter == null)
        {
            agent.setState(agent.seekingState);
        }
        else
        {
         //   agent.setState(agent.combatState);
        }
        
        return false;
    }

    public bool OnArriveAtTarget()
    {
       if(substate==Substate.chasing)
        {
            SetSubstate(Substate.lookingForPlayer);
        }
        return true;
    }

    public bool OnCompleteAction()
    {
        return false;
    }

    public bool OnDeath()
    {
        return false;
    }

    public bool OnReceiveAlert(Transform alertTarget)
    {

        return false;
    }

    public bool OnReceiveAlertStart(Transform alertTarget)
    {
        return false;
    }

    public bool OnDamagePlayer()
    {
        return false;
    }

}