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

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        _player.Anim.SetInteger("AttackCounter", comboCounter);

        //CHOOSE ATTACK DIRECTION
        float attackDirection = _player.FacingDir;

        if(_horizontal != 0)
            attackDirection = _horizontal;

        _player.SetVelocity(_player.AttackMovement[comboCounter].x * attackDirection, _player.AttackMovement[comboCounter].y);

        _stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        lastTimeAttacked = Time.time;

        _player.StartCoroutine("BusyFor", .15f);
    }

    public override void Update()
    {
        base.Update();

        if (_stateTimer < 0)
            _player.SetZeroVelocity();

        if (_triggerCalled)
            _stateMachine.ChangeState(_player.IdleState);
    }
}
