using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb; //Deklarerar en Rigidbody-variabel
    private SpriteRenderer sprite; //Deklarerar en Rigidbody-variabel
    private Animator anim; //Deklarerar en Animator-variabel
    [SerializeField] private AudioSource jumpSound; //Ljudeffekt för hopp
    [SerializeField] private AudioSource doubleJumpSound; //Ljudeffekt för dubbelhopp
    [SerializeField] private float speed = 5f; //Deklarerar en float som ska styra spelarens hastighet
    // SerializeField gör att variabeln går att ändra från editorn
    [SerializeField] private float jumpHeight = 10f; //Deklarerar en float som ska styra spelarens hastighet
    [SerializeField] private int maxJumps = 2;
    private int jumps;
    // För att känna av LANDNING
    private bool wasGrounded;
    // För att undvika att IsGrounded är true precis efter hopp
    [SerializeField] private float groundGraceAfterJump = 0.06f;
    private float ignoreGroundedUntilTime;
    private float horizontalInput; //En variabel som lagrar åt vilket håll användaren trycker (-1 är vänster, 1 är höger)
    private enum MovementState { idle, running, jumping, falling, doubleJumping };
    MovementState state = MovementState.idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Hämtar Rigidbody-komponenten på spelobjektet och lagrar i rb
        sprite = GetComponent<SpriteRenderer>(); //Hämtar Sprite-komponenten på spelobjektet och lagrar i sprite
        anim = GetComponent<Animator>(); //Hämtar Animator-komponenten på spelobjektet och lagrar i anim
        jumps = maxJumps;
    }

    bool IsGrounded()
    {
        if (Time.time < ignoreGroundedUntilTime) return false;
        return Physics2D.BoxCast(sprite.bounds.center, sprite.bounds.size, 0, Vector2.down, 0.05f, LayerMask.GetMask("Ground"));
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y); //Sätter en hastighet på rigidbodyn på spelaren
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        jumpSound.Play();
        // Undvik att "grounded" läses samma frame
        ignoreGroundedUntilTime = Time.time + groundGraceAfterJump;
    }

    void DoubleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        doubleJumpSound.Play();
        // Också bra att ignorera mark en stund här
        ignoreGroundedUntilTime = Time.time + groundGraceAfterJump;
    }

    void UpdateAnimationState()
    {
        state = MovementState.idle;

        if (rb.linearVelocity.x < -0.1f) { sprite.flipX = true; state = MovementState.running; }
        if (rb.linearVelocity.x >  0.1f) { sprite.flipX = false; state = MovementState.running; }

        if (rb.linearVelocity.y > 0.1f && jumps == 0) state = MovementState.doubleJumping;
        else if (rb.linearVelocity.y > 0.1f)          state = MovementState.jumping;

        if (rb.linearVelocity.y < -0.1f) state = MovementState.falling;

        anim.SetInteger("state", (int)state);
        
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); //Lyssnar efter om användaren trycker till höger eller till vänster och sparar i horizontalInput
        Move();

        bool grounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                // Första hoppet: sätt kvarvarande hopp till maxJumps-1
                Jump();
                jumps--;
            }
            else if (jumps > 0)
            {
                // Dubbelhopp i luften
                DoubleJump();
                jumps--;
            }
        }

        // Återställ först när vi LANDAR (edge: !wasGrounded -> grounded)
        if (grounded && !wasGrounded)
        {
            jumps = maxJumps;
        }

        wasGrounded = grounded;

        //Uppdatera animationens state
        UpdateAnimationState();

    }
}
