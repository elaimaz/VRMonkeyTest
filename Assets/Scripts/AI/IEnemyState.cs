using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    AIAgent.StateType getStateType();

    void Start(AIAgent thisCharacter);

    void UpdateState();

    bool OnTriggerEnter(Collider other);

    bool OnSeeEnemyStart(Transform enemy);

    bool OnSeeEnemy(Transform enemy);

    bool OnLoseSight();

    bool OnDamage(Transform attacker);

    void End();

    bool OnKnockbackStart();

    bool OnKnockbackEnd();

    bool OnArriveAtTarget();

    bool OnCompleteAction();

    bool OnDeath();

    bool OnReceiveAlertStart(Transform alertTarget);

    bool OnReceiveAlert(Transform alertTarget);

    bool OnDamagePlayer();
}
