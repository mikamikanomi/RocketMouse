using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public Sprite laserOnSprite;
    public Sprite laserOffSprite;
    public float interval = 0.5f;
    public float rotationSpeed;

    private bool isLaserOn = true;
    private float timeUntilNextToggle;
    private Collider2D collider2d;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        timeUntilNextToggle = interval;
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeUntilNextToggle -= Time.fixedDeltaTime;
        if (timeUntilNextToggle <= 0)
        {
            isLaserOn = !isLaserOn;
            collider2d.enabled = isLaserOn;
            if (isLaserOn)
            {
                spriteRenderer.sprite = laserOnSprite;
            }
            else
            {
                spriteRenderer.sprite = laserOffSprite;
            }
            timeUntilNextToggle = interval;
        }
        transform.RotateAround(
            transform.position,
            Vector3.forward,
            rotationSpeed * Time.fixedDeltaTime
            );
    }
}
