using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    int comboCounter;
    float lastTimeAttacked;
    float comboWindow = 2;

    public PlayerPrimaryAttackState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        horizontal = 0; //fix a bug on attack direction

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.Anim.SetInteger("AttackCounter", comboCounter);

        //CHOOSE ATTACK DIRECTION
        float attackDirection = player.FacingDir;

        if(horizontal != 0)
            attackDirection = horizontal;

        player.SetVelocity(player.AttackMovement[comboCounter].x * attackDirection, player.AttackMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        lastTimeAttacked = Time.time;

        player.StartCoroutine("BusyFor", .15f);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }
}
