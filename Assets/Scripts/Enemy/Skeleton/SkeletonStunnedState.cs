using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    EnemySkeleton enemy;

    public SkeletonStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.FX.InvokeRepeating("RedColorBlink", 0, 0.1f);

        stateTimer = enemy.StunnedDuration;

        rb.velocity = new Vector2(-enemy.FacingDir * enemy.StunDirection.x, enemy.StunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.FX.Invoke("CancelRedBlink", 0);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
}
