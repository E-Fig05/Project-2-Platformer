using UnityEngine;

public class PlatformerPlayer : MonoBehaviour
{
    [SerializeField] private GameObject GoldenPlatform;
    [SerializeField] private GameObject BigBoss;
    [SerializeField] private float speed = 4.5f;
    [SerializeField] private float jumpForce = 12.0f;
    [SerializeField] private bool hitEnemy = false;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D box;
    public float EnemiesKilled;
    public float money = 0;
    public bool canMove = true;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        money = 0;
        EnemiesKilled = 0;
    }

    void Update()
    {
        if (canMove == true)
        {
            float deltaX = Input.GetAxis("Horizontal") * speed;
            Vector2 movement = new Vector2(deltaX, body.linearVelocity.y);
            body.linearVelocity = movement;

            Vector3 max = box.bounds.max;
            Vector3 min = box.bounds.min;

            Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
            Vector2 corner2 = new Vector2(min.x, min.y - 0.2f);

            Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
            bool grounded = false;

            if (hit != null)
            {
                grounded = true;
            }

            body.gravityScale = (grounded && Mathf.Approximately(deltaX, 0)) ? 0 : 1;

            if (grounded && Input.GetKeyDown(KeyCode.Space))
            {
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            MovingPlatform platform = null;
            GoldenPlatform goldenPlatform = null;
            EnemyController enemy = null;
            BossBehavior Boss = null;
            if (hit != null)
            {
                platform = hit.GetComponent<MovingPlatform>();
                goldenPlatform = hit.GetComponent<GoldenPlatform>();
                enemy = hit.GetComponent<EnemyController>();
                Boss = hit.GetComponent<BossBehavior>();
            }
            if (platform != null)
            {
                transform.parent = platform.transform;
            }
            else
            {
                transform.parent = null;
            }
            if (goldenPlatform != null)
            {
                transform.parent = goldenPlatform.transform;
                goldenPlatform.enabled = true;
            }
            else
            {
                if (platform == null)
                {
                    transform.parent = null;
                }
            }

            if (enemy != null)
            {
                if (hitEnemy == false)
                {
                    body.AddForce(Vector2.up * jumpForce * 2f, ForceMode2D.Impulse);
                    enemy.health -= 1;
                    hitEnemy = true;
                }
            }
            else if (Boss != null)
            {
                if (hitEnemy == false)
                {
                    body.AddForce(Vector2.up * jumpForce * 2f, ForceMode2D.Impulse);
                    Boss.health -= 1;
                    hitEnemy = true;
                }
            }
            else
            {
                hitEnemy = false;
            }

            if (EnemiesKilled == 3)
            {
                BigBoss.SetActive(true);
            }

            Vector3 pScale = Vector3.one;

            if (platform != null)
            {
                pScale = platform.transform.localScale;
            }

            //Changes the player's direction based on their speed.
            anim.SetFloat("Speed", Mathf.Abs(deltaX));

            if (!Mathf.Approximately(deltaX, 0))
            {
                transform.localScale = new Vector3(Mathf.Sign(deltaX) / pScale.x, 1 / pScale.y, 1);
            }

            if (money >= 10)
            {
                GoldenPlatform.SetActive(true);
            }
        }
        else { body.linearVelocity = Vector3.zero; anim.enabled = false; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DeathBox KillBox = other.GetComponent<DeathBox>();
        if (KillBox != null)
        {
            KillPlayer();
        }
        CoinScript Coin = other.GetComponent<CoinScript>();
        if(Coin != null)
        {
            //Debug.Log("Coin Touched");
            money += Coin.value;
            Coin.gameObject.SetActive(false);
        }
    }

    private void KillPlayer()
    {
        gameObject.SetActive(false);
    }
}
