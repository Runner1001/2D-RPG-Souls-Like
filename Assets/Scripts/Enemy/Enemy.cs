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
    float defaultMoveSpeed;

    [Header("Attack Info")]
    [SerializeField] float attackDistance;
    [SerializeField] float attackCooldown;

    [Header("Stunned Info")]
    [SerializeField] float stunnedDuration;
    [SerializeField] Vector2 stunDirection;
    [SerializeField] protected GameObject countedImage;
    protected bool canBeStunned;


    public float MoveSpeed => moveSpeed;
    public float IdleTime => idleTime;
    public float AttackDistance => attackDistance;
    public float AttackCooldown => attackCooldown;
    public float BattleTime => battleTime;
    public float LastTimeAttacked { get; set; }
    public EnemyStateMachine StateMachine { get; private set; }
    public Vector2 StunDirection => stunDirection;
    public float StunnedDuration => stunnedDuration;

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.Update();
    }

    public virtual void FreezeTime(bool timeFrozen)
    {
        if (timeFrozen)
        {
            moveSpeed = 0f;
            Anim.speed = 0f;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            Anim.speed = 1f;
        }
    }

    protected virtual IEnumerator FreezeTimerFor(float seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(seconds);

        FreezeTime(false);
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        countedImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        countedImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
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
