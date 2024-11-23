using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Check Collisions")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask groundLayer;

    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }

    public int FacingDir { get; private set; } = 1;
    protected bool isFacingRight = true;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }

    #region Velocity
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        RB.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void SetZeroVelocity() => RB.velocity = Vector2.zero;
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
    }
    #endregion
}
