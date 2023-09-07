using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingBox : MonoBehaviour
{
    public float speed = 10;
    public float maxDistance = 15;
    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float movement = Mathf.PingPong(Time.time * speed, maxDistance);
        transform.position = _startPosition + Vector3.forward * movement;
    }
}