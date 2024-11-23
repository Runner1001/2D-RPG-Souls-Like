using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Check Collisions")]
    [SerializeField] Transform attackCheck;
    [SerializeField] float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask groundLayer;

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] float knockbackDuration = 0.2f;
    protected bool isKnocked;

    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public EntityFX FX { get; private set; }

    public int FacingDir { get; private set; } = 1;
    protected bool isFacingRight = true;

    public Transform AttackCheck => attackCheck;
    public float AttackRadius => attackCheckRadius;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        FX = GetComponentInChildren<EntityFX>();
    }

    protected virtual void Update()
    {

    }

    public virtual void Damage()
    {
        FX.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");
        Debug.Log(gameObject.name + " was damaged!");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        RB.velocity = new Vector2(knockbackDirection.x * -FacingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    #region Velocity
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if(isKnocked)
            return;

        RB.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;

        RB.velocity = Vector2.zero;
    }
    #endregion

    #region Flip
    public void Flip()
    {
        FacingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController(float x)
    {
        if (x > 0 && !isFacingRight)
            Flip();
        else if (x < 0 && isFacingRight)
            Flip();

    }
    #endregion

    #region Collision
    public virtual bool IsGrounded() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, groundLayer);


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion
}
