using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Settings")]
    public int playerID = 1;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float attackSpeedMultiplier = 0.5f; // Tăng lên một chút để di chuyển mượt hơn khi đánh

    [Header("Combat & Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f; // Nên để tầm 0.5 - 1.0 cho game 2D
    [SerializeField] private LayerMask opponentLayer;
    public HealthBar healthBar;

    public bool isDead = false;
    public bool isAttacking;
    public bool isHurt;

    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    private bool canAttack = true;
    private float originalScaleX;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        originalScaleX = Mathf.Abs(transform.localScale.x);

        // Tự động tìm HealthBar theo tên nếu chưa kéo thả
        if (healthBar == null)
        {
            string hbName = (playerID == 1) ? "HealthBar_Player1" : "HealthBar_Player2";
            GameObject hbObj = GameObject.Find(hbName);
            if (hbObj != null) healthBar = hbObj.GetComponent<HealthBar>();
        }

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(maxHealth);
        }
    }

    void Update()
    {
        if (isDead) return;

        if (isHurt)
        {
            body.velocity = new Vector2(0, body.velocity.y);
            return;
        }

        HandleMovement();
        HandleJump();
        HandleAttack();

        // Cập nhật Animator
        if (anim != null)
        {
            anim.SetBool("grounded", grounded);
            anim.SetBool("Run", Mathf.Abs(body.velocity.x) > 0.1f);
        }
    }

    private void HandleMovement()
    {
        float moveInput = 0;
        // Tách biệt phím di chuyển
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

        // Quay mặt nhân vật (Chỉ quay khi không đang đánh để tránh lỗi animation)
        if (moveInput != 0 && !isAttacking)
        {
            transform.localScale = new Vector3(moveInput * originalScaleX, transform.localScale.y, transform.localScale.z);
        }

        // Tốc độ di chuyển khi đánh sẽ chậm lại
        float currentSpeed = isAttacking ? speed * attackSpeedMultiplier : speed;
        body.velocity = new Vector2(moveInput * currentSpeed, body.velocity.y);
    }

    private void HandleJump()
    {
        bool jumpKey = (playerID == 1) ? Input.GetKeyDown(KeyCode.W) : Input.GetKeyDown(KeyCode.UpArrow);
        if (jumpKey && grounded && !isAttacking)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            grounded = false;
        }
    }

    private void HandleAttack()
    {
        // Tách biệt phím đánh
        bool attackKey = (playerID == 1) ? Input.GetKeyDown(KeyCode.J) : Input.GetKeyDown(KeyCode.Keypad1);

        if (attackKey && canAttack && !isHurt)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;
        isAttacking = true;

        if (anim != null) anim.SetTrigger("Atk1");

        // Đợi đến thời điểm "vung kiếm" trong animation thì mới check damage
        yield return new WaitForSeconds(0.1f);
        CheckDamage();

        // Đợi animation kết thúc
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
        canAttack = true;
    }

    private void CheckDamage()
    {
        // Tạo một vòng tròn va chạm tại điểm attackPoint
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, opponentLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            PlayerMove target = enemy.GetComponent<PlayerMove>();

            // FIX LỖI TỰ MẤT MÁU: Kiểm tra target khác chính mình (this)
            if (target != null && target != this)
            {
                target.TakeDamage(25);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (healthBar != null) healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            isHurt = true;
            if (anim != null) anim.SetTrigger("Hurt");
            Invoke(nameof(EndHurt), 0.2f);
        }
    }

    private void EndHurt() => isHurt = false;

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (anim != null) anim.SetBool("IsDead", true);

        // Vô hiệu hóa vật lý để xác rơi xuống hoặc đứng yên
        body.velocity = Vector2.zero;
        body.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        // Gọi BattleManager thông báo người thua
        if (BattleManager.instance != null)
        {
            BattleManager.instance.FinishGame(playerID);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) grounded = true;
    }

    // Vẽ vòng tròn phạm vi đánh để dễ căn chỉnh trong Editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}