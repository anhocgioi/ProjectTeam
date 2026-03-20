using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("1 = P1 (WASD, J) | 2 = P2 (Arrows, NumPad 1)")]
    public int playerID = 1;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float attackSpeedMultiplier = 0.1f;

    [Header("Combo Settings")]
    [SerializeField] private float comboResetTime = 0.6f;

    [Header("Combat & Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask opponentLayer;
    public bool isDead = false;

    [Header("Slash FX Settings")]
    [SerializeField] private GameObject slashObject;
    [SerializeField] private SpriteRenderer slashSpriteRenderer;
    [SerializeField] private Sprite[] slashSprites;

    private Rigidbody2D body;
    private Animator anim;
    private int jumpCount = 0;
    private bool grounded;

    private bool isAttacking = false;
    private bool canAttack = true;
    private int comboStep = 0;
    private float lastAttackTime;
    private bool isHurt = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (slashObject != null) slashObject.SetActive(false);
    }

    void Update()
    {
        if (isDead || isHurt) return;

        HandleMovement();
        HandleJump();
        HandleAttackInput();

        if (Time.time - lastAttackTime > comboResetTime && !isAttacking && comboStep > 0)
        {
            ResetCombo();
        }

        if (anim != null)
        {
            anim.SetBool("grounded", grounded);
            anim.SetBool("Run", Mathf.Abs(body.velocity.x) > 0.1f);
        }
    }

    private void HandleMovement()
    {
        float moveInput = 0;

        // Tách biệt phím điều khiển
        if (playerID == 1)
        {
            if (Input.GetKey(KeyCode.A)) moveInput = -1;
            else if (Input.GetKey(KeyCode.D)) moveInput = 1;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1;
            else if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1;
        }

        // Xoay hướng nhân vật (Sửa lỗi ngược mặt P2)
        if (moveInput != 0)
        {
            // Kiểm tra xem nhân vật có đang bị xoay 180 độ bởi LevelManager không
            bool isRotated = Mathf.Approximately(transform.eulerAngles.y, 180f);

            if (isRotated)
            {
                // Nếu đang xoay 180 độ (P2), hướng scale phải đảo ngược lại
                transform.localScale = new Vector3(-moveInput * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // P1 bình thường
                transform.localScale = new Vector3(moveInput * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        float currentSpeed = isAttacking ? speed * attackSpeedMultiplier : speed;
        body.velocity = new Vector2(moveInput * currentSpeed, body.velocity.y);
    }

    private void HandleJump()
    {
        bool jumpKey = (playerID == 1) ? Input.GetKeyDown(KeyCode.W) : Input.GetKeyDown(KeyCode.UpArrow);

        if (jumpKey && jumpCount < 1 && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            grounded = false;
            jumpCount++;
        }
    }

    private void HandleAttackInput()
    {
        bool attackKey = (playerID == 1) ? Input.GetKeyDown(KeyCode.J) : Input.GetKeyDown(KeyCode.Keypad1);

        if (attackKey && canAttack)
        {
            lastAttackTime = Time.time;
            isAttacking = true;

            comboStep++;
            if (comboStep > 3 || (slashSprites != null && comboStep > slashSprites.Length))
                comboStep = 1;

            if (anim != null) anim.SetTrigger("Atk" + comboStep);

            if (comboStep >= 3) canAttack = false;

            CancelInvoke("EndAttack");
            Invoke("EndAttack", 0.4f);
        }
    }

    public void Hit()
    {
        if (slashObject != null && slashSprites != null && slashSprites.Length > 0)
        {
            int spriteIndex = Mathf.Clamp(comboStep - 1, 0, slashSprites.Length - 1);
            slashObject.SetActive(true);
            slashSpriteRenderer.sprite = slashSprites[spriteIndex];
            CancelInvoke("HideSlash");
            Invoke("HideSlash", 0.1f);
        }

        if (attackPoint == null) return;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, opponentLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            PlayerMove opponent = enemy.GetComponent<PlayerMove>();
            if (opponent != null && opponent != this)
            {
                opponent.TakeDamage(15);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        isHurt = true;

        if (currentHealth <= 0) Die();
        else
        {
            if (anim != null) anim.SetTrigger("Hurt");
            CancelInvoke("EndHurt");
            Invoke("EndHurt", 0.3f);
        }
    }

    private void EndHurt() => isHurt = false;
    private void HideSlash() => slashObject.SetActive(false);

    private void Die()
    {
        isDead = true;
        if (anim != null) anim.SetBool("isDead", true);
        body.velocity = Vector2.zero;
        body.simulated = false;
    }

    private void ResetCombo() { comboStep = 0; canAttack = true; }
    public void EndAttack() { isAttacking = false; if (comboStep >= 3) ResetCombo(); else canAttack = true; }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) { jumpCount = 0; grounded = true; }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}