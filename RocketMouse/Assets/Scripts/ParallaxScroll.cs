using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public Renderer background;
    public Renderer foreground;
    public float backgroundSpeed = 0.01f;
    public float foregroundSpeed = 0.03f;
    public float offset = 0.0f;

    // Update is called once per frame
    void Update()
    {
        float backgroundOffset = offset * backgroundSpeed;
        float foregroundOffset = offset * foregroundSpeed;

        background.material.mainTextureOffset = new Vector2(backgroundOffset, 0);
        foreground.material.mainTextureOffset = new Vector2(foregroundOffset, 0);
    }
}
