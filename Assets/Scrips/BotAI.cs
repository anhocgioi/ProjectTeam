using UnityEngine;

public class BotAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float stopDistance = 1.5f;
    public float attackDistance = 1.2f;

    [Header("Attack")]
    public float attackCooldown = 1f;

    [Header("Target")]
    public Transform target;

    private Rigidbody2D rb;
    private Animator anim;

    private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (target == null)
        {
            GameObject player = GameObject.Find("Player_1");
            if (player != null)
                target = player.transform;
        }

        rb.freezeRotation = true;
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        // ====== QUAY MẶT ======
        float dir = target.position.x > transform.position.x ? 1 : -1;

        transform.localScale = new Vector3(
            dir * Mathf.Abs(transform.localScale.x),
            transform.localScale.y,
            transform.localScale.z
        );

        // ====== DI CHUYỂN ======
        if (distance > stopDistance)
        {
            Move(dir);
        }
        else
        {
            Stop();
        }

        // ====== TẤN CÔNG ======
        if (distance <= attackDistance)
        {
            TryAttack();
        }
    }

    void Move(float dir)
    {
        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

        if (anim != null)
            anim.SetBool("Run", true);
    }

    void Stop()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);

        if (anim != null)
            anim.SetBool("Run", false);
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        if (anim != null)
        {
            anim.SetTrigger("Atk1");
        }
    }
}