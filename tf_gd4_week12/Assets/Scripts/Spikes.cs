using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class Spikes : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform leftSpike;
    [SerializeField] Transform rightSpike;
    [SerializeField] float minDistanceToStartAction;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 moveDirection;
    [SerializeField] string actionToPerform;
    [SerializeField] float distanceToMove;


    Transform player;
    Vector3 startPosition;

    int rotationDirection;

    public int direction = 1;

    public bool test = false;

    public bool startCoroutine = true;
    
    public bool phase1 = true;
    bool fall = false;
    bool startMove = false;
    BoxCollider2D playerCollider;
    float distanceMoved = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCollider = player.GetComponent<BoxCollider2D>();
        startPosition = transform.position;
    }

    private void Update()
    {

        float leftSpikeDistance = Vector3.Distance(leftSpike.position, player.position);
        float rightSpikeDistance = Vector3.Distance(rightSpike.position, player.position);

        if (actionToPerform == "Move")
        {
            if (leftSpikeDistance < minDistanceToStartAction || rightSpikeDistance < minDistanceToStartAction)
            {
                
                if (startCoroutine)
                {
                    StartCoroutine(Move(distanceToMove, moveDirection));
                    startCoroutine = false;
                }
            }
        }
        else if (actionToPerform  == "FallDown1")
        {
            if (playerCollider.bounds.max.y >= leftSpike.transform.position.y && !phase1)
            {
                StopAllCoroutines();
                distanceToMove = Mathf.Abs(playerCollider.bounds.max.y - leftSpike.transform.position.y);
                transform.position -= moveDirection * distanceToMove;
            }

            if (transform.position.y > 0.6362522 && !phase1)
            {
                transform.position = new Vector3(transform.position.x, 0.6362522f, transform.position.z);
            }

            if (phase1)
            {
                RaycastHit2D lefthit = Physics2D.Raycast(leftSpike.position, moveDirection, 10f, 1 << 7);
                RaycastHit2D righthit = Physics2D.Raycast(rightSpike.position, moveDirection, 10f, 1 << 7);

                if (lefthit || righthit)
                {
                    phase1 = false;
                    distanceToMove = Mathf.Max(lefthit.distance, righthit.distance);
                    StartCoroutine(Move(distanceToMove, moveDirection));
                }

            }

         
        }
        else if (actionToPerform == "Rotate")
        {

            if (startCoroutine)
            {
                if (leftSpikeDistance < minDistanceToStartAction)
                {
                    StartCoroutine(Rotate(1));
                    startCoroutine = false;
                }
                else if (rightSpikeDistance < minDistanceToStartAction)
                {
                    StartCoroutine(Rotate(-1));
                    startCoroutine = false;
                }
            }
        }
    }

    IEnumerator Rotate(int direction)
    {
        float startRotation = 0f;
        float endRotation = 180f;
        Vector3 eulerangles = transform.eulerAngles;

        while (startRotation < endRotation)
        {
            startRotation += Time.deltaTime * rotationSpeed;
            transform.eulerAngles = new Vector3(eulerangles.x, eulerangles.y, eulerangles.z + startRotation * direction);
            yield return null;
        }

        transform.eulerAngles = new Vector3(eulerangles.x, eulerangles.y, eulerangles.z + endRotation);
        startCoroutine = true;

    }

    IEnumerator Move(float distance, Vector3 direction)
    {
        Vector3 startPosition = transform.position;
        float distanceMoved = 0f;

        while (distanceMoved < distance)
        {
            distanceMoved += Time.deltaTime * moveSpeed;
            transform.position = startPosition + direction * distanceMoved;
            yield return null;
        }
    }

}
