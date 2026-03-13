using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float attackSpeedMultiplier = 0.1f;

    [Header("Combo Settings")]
    [SerializeField] private float comboResetTime = 0.6f;

    private Rigidbody2D body;
    private Animator anim;
    private int jumpCount = 0;
    private bool grounded;

    private bool isAttacking = false;
    private bool canAttack = true;
    private int comboStep = 0;
    private float lastAttackTime;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump(); // Đã đổi sang phím W
        HandleAttackInput();

        if (Time.time - lastAttackTime > comboResetTime && !isAttacking)
        {
            ResetCombo();
        }

        anim.SetBool("grounded", grounded);
        anim.SetBool("Run", Mathf.Abs(body.velocity.x) > 0.1f);
    }

    private void HandleMovement()
    {
        float moveInput = 0;

        // Di chuyển bằng A và D
        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        float currentSpeed = isAttacking ? speed * attackSpeedMultiplier : speed;
        body.velocity = new Vector2(moveInput * currentSpeed, body.velocity.y);
    }

    private void HandleJump()
    {
        // SỬ DỤNG PHÍM W ĐỂ NHẢY
        if (Input.GetKeyDown(KeyCode.W) && jumpCount < 1 && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            grounded = false;
            jumpCount++;
            // Nếu bạn có trigger cho nhảy, hãy thêm vào đây: anim.SetTrigger("Jump");
        }
    }

    private void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.J) && canAttack)
        {
            lastAttackTime = Time.time;
            isAttacking = true;

            comboStep++;
            anim.SetTrigger("Atk" + comboStep);

            if (comboStep >= 3)
            {
                canAttack = false;
            }

            CancelInvoke("EndAttack");
            Invoke("EndAttack", 0.5f);
        }
    }

    private void ResetCombo()
    {
        comboStep = 0;
        canAttack = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
        if (comboStep >= 3)
        {
            ResetCombo();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            grounded = true;

        }
    }
}