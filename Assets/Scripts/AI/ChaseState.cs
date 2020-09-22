using UnityEngine;
using System.Collections;
[System.Serializable]
public class ChaseState : IEnemyState
{
    private AIAgent agent;

    
    public enum Substate { starting,chasing,preparingFire,fireDelay}
    public Substate substate = Substate.starting;
    public float currentTimer = 0;

    public float seeEnemyDelay = 0.5f;
    public float chaseSpeed = 6.0f;
    public float shootingRange = 5.0f;
    public float shootingTimer = 2.0f;
    public float preparingShootTime = 0.25f;
    public float postShootTime = 0.5f;

    public AIAgent.StateType getStateType()
    {
        return AIAgent.StateType.chasing ;
    }

    public void Start(AIAgent newAgent)
    {
        agent = newAgent;
        agent.navAgent.Stop();
        currentTimer = 0;

        if (!agent.chasing)
        {
            agent.chasing = true;
            GameLogic.instance.AddChaser();
        }
        //SetSubstate(Substate.chasing);
    }

    public void SetSubstate(Substate newSubstate)
    {
        substate = newSubstate;
        currentTimer = 0;

        switch (substate)
        {
            case Substate.chasing:
                agent.navAgent.SetTarget(agent.target, chaseSpeed,shootingRange);
                break;
            case Substate.preparingFire:
                break;
            case Substate.fireDelay:
                break;
        }
    }

    public void UpdateState()
    {
        currentTimer += Time.deltaTime;
        agent.lastTargetPosition.position = agent.target.position;

        switch (substate)
        {
            case Substate.starting:
                if (currentTimer >= seeEnemyDelay)
                {
                    SetSubstate(Substate.chasing);
                }
                break;
            case Substate.chasing:
                agent.navAgent.SetTarget(agent.target, chaseSpeed,shootingRange);
                if (currentTimer >= shootingTimer)
                {
                    SetSubstate(Substate.preparingFire);
                }
                break;
            case Substate.preparingFire:
                if (currentTimer >= preparingShootTime)
                {
                    Fire();
                    
                }
                break;
            case Substate.fireDelay:
                if (currentTimer >= postShootTime)
                {
                    SetSubstate(Substate.chasing);
                }
                break;
        }
    }

    void Fire()
    {
        agent.character.Fire();
        SetSubstate(Substate.fireDelay);
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
       // agent.setState(agent.combatState);
        return false;
    }

    public bool OnDamage(Transform attacker)
    {
        if (attacker != null && agent.target!=null)
        {
            agent.target = attacker;
            agent.GetLastTargetPosition().position = attacker.position;
        }
        //agent.eyeController.SetDamaged();
        return false;
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
       if(substate==Substate.chasing && currentTimer > shootingTimer)
        {
            Fire();
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