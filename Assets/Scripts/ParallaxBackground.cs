using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] float _parallaxEffect;

    Transform _camera;
    float xPosition;
    float lenght;

    void Start()
    {
        _camera = Camera.main.transform;

        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    void Update()
    {
        float distanceMoved = _camera.position.x * (1 - _parallaxEffect);
        float distanceToMove = _camera.position.x * _parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > xPosition + lenght)
            xPosition = xPosition + lenght;
        else if (distanceMoved < xPosition - lenght)
            xPosition = xPosition - lenght;
    }
}
