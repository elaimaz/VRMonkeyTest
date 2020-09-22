using UnityEngine;
using System.Collections;
[System.Serializable]
public class StunnedState : IEnemyState
{
    private AIAgent agent;

    public AIAgent.StateType getStateType()
    {
        return AIAgent.StateType.idle;
    }

    public void Start(AIAgent newAgent)
    {
        agent = newAgent;
    }

    public void UpdateState()
    {

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
        agent.setState(agent.seekingState);
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
        agent.character.SetState(Character.States.idle);
        return false;
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