using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState { Idle, Running, Jumping, Falling,Dead }

public class Player : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float distance;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private int hearts = 3;
    [SerializeField] private Corazones[] hearts_UI;
    

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private Weapon weapon;

    private float xInput;
    private PlayerState currentState;

    private Player player;

    private Vector2 direction = Vector2.right;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
    }

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if(currentState == PlayerState.Dead) return;

        Flip();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            weapon.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Jump();
        }

        ChangeAnimState();
    }

    void FixedUpdate()
    {
        if (currentState == PlayerState.Dead) return;
        HandleMovement();
    }

    private void HandleMovement()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        Vector2 move = new(xInput * speed, rb.velocity.y);
        rb.velocity = move;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        if (xInput > 0)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            direction = Vector2.right;
        }
        else if (xInput < 0)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
            direction = Vector2.left;
        }
    }

    private void ChangeAnimState()
    {
        if (isGrounded())
        {
            if (xInput == 0 && currentState != PlayerState.Jumping)
            {
                ChangeState(PlayerState.Idle);
            }
            else if (Mathf.Abs(xInput) > 0 && currentState != PlayerState.Jumping)
            {
                ChangeState(PlayerState.Running);
            }
        }
        else
        {
            if (rb.velocity.y < -0.5f)
            {
                ChangeState(PlayerState.Falling);
            }
            else if (rb.velocity.y > 0.5f)
            {
                ChangeState(PlayerState.Jumping);
            }
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, distance, whatIsGround);
        return hit.collider != null;
    }

    public void ChangeState(PlayerState newState)
    {
        if (newState == currentState) return;

        currentState = newState;

        switch (newState)
        {
            case PlayerState.Idle:
                anim.SetTrigger("Idle");
                break;
            case PlayerState.Running:
                anim.SetTrigger("Correr");
                break;
            case PlayerState.Jumping:
                anim.SetTrigger("Saltar");
                break;
            case PlayerState.Falling:
                anim.SetTrigger("Caer");
                break;
            case PlayerState.Dead:
                anim.SetTrigger("Muere");
                break;
        }
    }
     
    public Vector2 GetDirection() { return direction; }

    public void TakeDamage()
    {
        hearts = Mathf.Clamp(hearts - 1, 0, hearts);
        hearts_UI[hearts].SetHeartEmpty();

        if (hearts == 0)
        {
            player.ChangeState(PlayerState.Dead);

            SceneManager.LoadScene("Perdiste");
        }
    }
     
}
