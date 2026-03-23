using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Settings")]
    public int playerID = 1; // 1 = P1, 2 = P2 (hoặc AI)

    [Header("Movement Settings")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float attackSpeedMultiplier = 0.1f;

    [Header("Combo Settings")]
    [SerializeField] private float comboResetTime = 0.6f;

    [Header("Combat & Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask opponentLayer;

    public bool isDead = false;

    [Header("Slash FX")]
    [SerializeField] private GameObject slashObject;
    [SerializeField] private SpriteRenderer slashSpriteRenderer;
    [SerializeField] private Sprite[] slashSprites;

    // Components
    private Rigidbody2D body;
    private Animator anim;

    // State
    public bool isAttacking;
    private bool grounded;
    private int jumpCount;
    private bool canAttack = true;
    private int comboStep = 0;
    private float lastAttackTime;
    public bool isHurt;

    // AI only
    private Transform opponent;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (slashObject != null) slashObject.SetActive(false);

        // Logic tìm đối thủ cho AI (giữ nguyên của bạn)
        if (GameDataManager.instance != null && GameDataManager.instance.isSinglePlayer && playerID == 2)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) opponent = playerObj.transform;
        }
    }

    void Update()
    {
        if (isDead || isHurt) return;

        // KIỂM TRA CHẾ ĐỘ CHƠI
        bool isAI = (playerID == 2 && GameDataManager.instance?.isSinglePlayer == true);

        if (isAI)
        {
            AIControl();
        }
        else
        {
            HandleMovement();
            HandleJump();
            HandleAttack();
        }

        if (Time.time - lastAttackTime > comboResetTime && !isAttacking)
        {
            ResetCombo();
        }

        if (anim != null)
        {
            anim.SetBool("grounded", grounded);
            anim.SetBool("Run", Mathf.Abs(body.velocity.x) > 0.1f);
        }
    }

    // ================= CHỈNH SỬA DI CHUYỂN =================
    private void HandleMovement()
    {
        float moveInput = 0;

        if (playerID == 1)
        {
            // Player 1: A (Trái), D (Phải)
            if (Input.GetKey(KeyCode.A)) moveInput = -1;
            else if (Input.GetKey(KeyCode.D)) moveInput = 1;
        }
        else if (playerID == 2)
        {
            // Player 2: LeftArrow, RightArrow
            if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1;
            else if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1;
        }

        // Xoay mặt nhân vật
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(moveInput) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
        }

        float currentSpeed = isAttacking ? speed * attackSpeedMultiplier : speed;
        body.velocity = new Vector2(moveInput * currentSpeed, body.velocity.y);
    }

    // ================= CHỈNH SỬA NHẢY =================
    private void HandleJump()
    {
        bool jumpKey = false;

        if (playerID == 1) jumpKey = Input.GetKeyDown(KeyCode.W);
        else if (playerID == 2) jumpKey = Input.GetKeyDown(KeyCode.UpArrow);

        if (jumpKey && grounded)
        {
            Jump();
        }
    }

    // ================= CHỈNH SỬA TẤN CÔNG =================
    private void HandleAttack()
    {
        bool attackKey = false;

        if (playerID == 1)
            attackKey = Input.GetKeyDown(KeyCode.J);
        else if (playerID == 2)
            attackKey = Input.GetKeyDown(KeyCode.Keypad1); // Phím 1 bên NumLock

        if (attackKey && canAttack)
        {
            Attack();
        }
    }

    // Các hàm Action bên dưới (Attack, Jump, Hit, TakeDamage...) giữ nguyên như code của bạn
    private void Attack()
    {
        lastAttackTime = Time.time;
        isAttacking = true;
        comboStep = Mathf.Min(comboStep + 1, 3);
        if (anim != null) anim.SetTrigger("Atk" + comboStep);
        if (comboStep >= 3) canAttack = false;
        CancelInvoke(nameof(EndAttack));
        Invoke(nameof(EndAttack), 0.45f);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        grounded = false;
        jumpCount++;
    }

    public void Hit()
    {
        if (attackPoint == null) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, opponentLayer);
        foreach (var hit in hits)
        {
            PlayerMove target = hit.GetComponent<PlayerMove>();
            if (target != null && target != this) target.TakeDamage(15);
        }
        if (slashObject != null && slashSprites.Length > 0)
        {
            int index = Mathf.Clamp(comboStep - 1, 0, slashSprites.Length - 1);
            slashSpriteRenderer.sprite = slashSprites[index];
            slashObject.SetActive(true);
            CancelInvoke(nameof(HideSlash));
            Invoke(nameof(HideSlash), 0.1f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        isHurt = true;
        if (anim != null) anim.SetTrigger("Hurt");
        if (currentHealth <= 0) Die();
        else Invoke(nameof(EndHurt), 0.3f);
    }

    private void EndHurt() => isHurt = false;
    private void Die() { isDead = true; if (anim != null) anim.SetBool("isDead", true); body.velocity = Vector2.zero; body.simulated = false; }
    private void ResetCombo() { comboStep = 0; canAttack = true; }
    private void EndAttack() { isAttacking = false; if (comboStep >= 3) ResetCombo(); else canAttack = true; }
    private void HideSlash() { if (slashObject != null) slashObject.SetActive(false); }
    private void OnCollisionEnter2D(Collision2D other) { if (other.gameObject.CompareTag("Ground")) { grounded = true; jumpCount = 0; } }

    // Logic AIControl giữ nguyên để tránh lỗi khi chơi SinglePlayer
    private void AIControl()
    {
        if (opponent == null) return;
        float direction = Mathf.Sign(opponent.position.x - transform.position.x);
        if (direction != 0) transform.localScale = new Vector3(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        float currentSpeed = isAttacking ? speed * attackSpeedMultiplier : speed;
        body.velocity = new Vector2(direction * currentSpeed, body.velocity.y);
        if (Vector2.Distance(transform.position, opponent.position) < attackRange + 0.6f && canAttack) Attack();
        if (grounded && Random.value < 0.012f) Jump();
    }
}