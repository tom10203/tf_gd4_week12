using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Vector3 moveDirection;
    [SerializeField] float moveDistance;

    PlayerController playerController;

    Vector3 startPosition;
    public bool startCoroutine = false;

    private void Start()
    {
        startPosition = transform.position;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (startCoroutine)
        {
            StartCoroutine(Move());
            startCoroutine = false;
        }
    }
    IEnumerator Move()
    {
        float distanceMoved = 0f;

        while (distanceMoved < moveDistance)
        {
            distanceMoved += Time.deltaTime * moveSpeed;
            transform.position = startPosition + moveDirection * distanceMoved;
            yield return null;
        }

        transform.position = startPosition + moveDirection * moveDistance;

        playerController.startCoroutine = true;
    }
}
