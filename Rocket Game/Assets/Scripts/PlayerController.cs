using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float thrustForce = 10f;
    Rigidbody2D rb;
    public UIDocument uiDocument;
    private Label scoreText;
    public GameObject explosionEffect;
    private Button restartButton;
    private Button homeButton;
    private int highScore;
    int score;
    private Label highScoreLabel;
    private float minX = -12f;
    private float maxX = 12f;
    private float minY = -6f;
    private float maxY = 7f;
    public GameObject apple;
    public InputAction moveForward;
    public InputAction lookPosition;
    public Vector2 scoreDown = new Vector2(0, 85);

    [SerializeField] private Sprite pinkManVisual;
    [SerializeField] private Sprite ninjaFrogVisual;
    [SerializeField] private Sprite virtualGuyVisual;
    [SerializeField] private Sprite maskDudeVisual;
    private Sprite currentSprite;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        ApplySelectedCharacter();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        Debug.Log(PlayerPrefs.GetString("Character"));

        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;

        homeButton = uiDocument.rootVisualElement.Q<Button>("HomeButton");
        homeButton.style.display = DisplayStyle.None;
        homeButton.clicked += EndGame;

        highScoreLabel = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        highScoreLabel.style.display = DisplayStyle.None;

        if (!PlayerPrefs.HasKey("High Score"))
        {
            PlayerPrefs.SetInt("High Score", 0);
        }
        highScore = PlayerPrefs.GetInt("High Score");

        NewApple();
        moveForward.Enable();
        lookPosition.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (moveForward.IsPressed())
        {
            // Calculate mouse direction
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>());
            Vector2 direction = (mousePos - transform.position).normalized;

            // Move player towards mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);
        }
        }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            // Destroy player if collide with obstacle
            Destroy(gameObject);

            // Show restart and home buttons
            restartButton.style.display = DisplayStyle.Flex;
            homeButton.style.display = DisplayStyle.Flex;

            if (score > highScore)
            {
                PlayerPrefs.SetInt("High Score", score);
                highScore = PlayerPrefs.GetInt("High Score");
            }

            highScoreLabel.text = "High Score: " + highScore;
            highScoreLabel.style.display = DisplayStyle.Flex;
            Debug.Log("High Score: " + PlayerPrefs.GetInt("High Score"));

            scoreText.style.translate = scoreDown;
        }
    }

    void ApplySelectedCharacter()
{

    string selected = PlayerPrefs.GetString("Character", "PinkMan");

    currentSprite = selected switch
    {
        "PinkMan" => pinkManVisual,
        "NinjaFrog" => ninjaFrogVisual,
        "VirtualGuy" => virtualGuyVisual,
        "MaskDude" => maskDudeVisual,
        _ => pinkManVisual
    };

    spriteRenderer.sprite = currentSprite;
}


    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Apple"))
    {
        score += 1;
        scoreText.text = "Score: " + score;

        Destroy(other.gameObject);
        NewApple();
    }
}

    void NewApple()
    {
        float posX = Random.Range(minX, maxX);
        float posY = Random.Range(minY, maxY);

        Vector3 spawnPos = new Vector3(posX, posY, 0f);

        GameObject newApple = Instantiate(apple, spawnPos, Quaternion.identity);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void EndGame()
    {
        SceneManager.LoadScene("StartScene");
    }
}
