using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] float parallaxEffect;

    Transform myCamera;
    float xPosition;
    float lenght;

    void Start()
    {
        myCamera = Camera.main.transform;

        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    void Update()
    {
        float distanceMoved = myCamera.position.x * (1 - parallaxEffect);
        float distanceToMove = myCamera.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > xPosition + lenght)
            xPosition = xPosition + lenght;
        else if (distanceMoved < xPosition - lenght)
            xPosition = xPosition - lenght;
    }
}
