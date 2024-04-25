using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MouseController : MonoBehaviour
{
    public float jetpackForce = 75.0f;
    public float forwaedMovementSpeed = 3.0f;
    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    public ParticleSystem jetpack;
    public TMP_Text textCoins;
    public Button buttonRestart;
    public Button buttonMenu;
    public AudioClip coinbCollectSound;
    public AudioSource jetpackAudio;
    public AudioSource footstepsAudio;
    public ParallaxScroll parallaxScroll;

    private Rigidbody2D rb;
    private bool grounded;
    private Animator animator;
    private bool dead = false;
    private uint coins = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        textCoins.text = "0";

        StartCoroutine(LevelCount()); 
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Coins")
        {
            CollectCoin(collider);
        }
        else
        {
            HitByLaser(collider);
        }
    }

    private void CollectCoin(Collider2D coinCollider)
    {
        ++coins;
        textCoins.text = coins.ToString();

        Destroy(coinCollider.gameObject);
        AudioSource.PlayClipAtPoint(coinbCollectSound, transform.position);
    }

    private void HitByLaser(Collider2D laserCollider)
    {
        if (!dead)
        {
            AudioSource laser = laserCollider.GetComponent<AudioSource>();
            laser.Play();
        }

        dead = true;
        animator.SetBool("dead", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");

        if (!dead)
        {
            jetpackActive = jetpackActive && !dead;
            if (jetpackActive)
            {
                rb.AddForce(jetpackForce * Vector2.up);
            }

            Vector2 newVelocity = rb.velocity;
            newVelocity.x = forwaedMovementSpeed;
            rb.velocity = newVelocity;
        }

        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);
        DisplayRestartButton();
        DisplayMenuButton();
        AdjustFootstepsAndJetpackSound(jetpackActive);
        parallaxScroll.offset = transform.position.x;
    }

    void UpdateGroundedStatus()
    {
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        animator.SetBool("grounded", grounded);
    }

    void AdjustJetpack(bool jetpackActive)
    {
        var emission = jetpack.emission;
        emission.enabled = !grounded;
        emission.rateOverTime = jetpackActive ? 300.0f : 75.0f;
    }
    
    private void DisplayRestartButton()
    {
        bool active = buttonRestart.gameObject.activeSelf;
        if (grounded && dead && !active)
        {
            buttonRestart.gameObject.SetActive(true);
        }
    }
    private void DisplayMenuButton()
    {
        bool active = buttonMenu.gameObject.activeSelf;
        if (grounded && dead && !active)
        {
            buttonMenu.gameObject.SetActive(true);
        }
    }

    public void OnClickedRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickedMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    private void AdjustFootstepsAndJetpackSound(bool jetpackActive)
    {
        footstepsAudio.enabled = !dead && grounded;
        jetpackAudio.enabled = !dead && !grounded;
        jetpackAudio.volume = jetpackActive ? 1.0f : 0.5f;
    }
    IEnumerator LevelCount()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            Debug.Log("레벨 증가");
        }
    }
}
