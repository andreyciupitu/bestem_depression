using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed;

    [SerializeField]
    private float loopSize;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float nextPos = Mathf.Repeat(Time.time * scrollSpeed, loopSize);
        transform.position = startPosition + nextPos * Vector3.left;
    }
}
