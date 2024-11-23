using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Move Info")]
    [SerializeField] float moveSpeed;
    [SerializeField] float idleTime;
    [SerializeField] float battleTime;

    [Header("Attack Info")]
    [SerializeField] float attackDistance;
    [SerializeField] float attackCooldown;


    public float MoveSpeed => moveSpeed;
    public float IdleTime => idleTime;
    public float AttackDistance => attackDistance;
    public float AttackCooldown => attackCooldown;
    public float BattleTime => battleTime;
    public float LastTimeAttacked { get; set; }
    public EnemyStateMachine StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.Update();
    }

    public virtual void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

    public virtual RaycastHit2D IsPlayerDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 50, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * FacingDir, transform.position.y));
    }
}
