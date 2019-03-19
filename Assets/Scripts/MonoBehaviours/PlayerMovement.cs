using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent2D agent;
    public SaveData playerSaveData;
    private GameObject pauseInterface;
    public float turnThreshold = .2f;
    public float inputHoldDelay = .5f;

    private Interactable currentInteractable;
    private Vector3 previousPosition;
    private bool handleInput = true;
    private WaitForSeconds inputHoldWait;
    private float stopDistanceProportion = .1f;

    private readonly int hashSpeedPara = Animator.StringToHash("Speed");
    private readonly int hashLocomotionTag = Animator.StringToHash("Locomotion");
    public const string startingPositionKey = "starting position";

    private void Start()
    {
        inputHoldWait = new WaitForSeconds(inputHoldDelay);

        string startingPositionName = "";
        playerSaveData.Load(startingPositionKey, ref startingPositionName);
        Transform startingPosition = StartingPosition.FindStartingPosition(startingPositionName);

        transform.position = startingPosition.position;
        transform.rotation = startingPosition.rotation;
        previousPosition = transform.position;
    }

    private bool IsPause()
    {
        return pauseInterface = GameObject.Find("PauseInterface");
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseInterface.SetActive(false);
    }

    private void Update()
    {
        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion)
        {
            transform.position = agent.destination;
            agent.isStopped = true;
            animator.SetFloat(hashSpeedPara, 0f);

            if (currentInteractable)
            {
                transform.rotation = currentInteractable.interactionLocation.rotation;

                currentInteractable.Interact();
                currentInteractable = null;

                StartCoroutine(WaitForInteraction());
            }
        }
        else
        {
            agent.isStopped = false;
            animator.SetFloat(hashSpeedPara, 1f);
            SetRotation();
        }

        if (Input.GetMouseButton(0) /*&& currentInteractable == null*/ && !IsPause())
        {
            if (!handleInput)
                return;

            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            agent.destination = targetPosition;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pauseInterface = GameObject.Find("PauseInterface");
            pauseInterface.SetActive(true);
        }
    }

    private void SetRotation()
    {
        Vector3 direction = transform.position - previousPosition;

        if (direction.magnitude >= turnThreshold)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, .5f);

            previousPosition = transform.position;
        }
    }

    public void OnInteractableClick(Interactable interactable)
    {
        if (!handleInput)
            return;

        currentInteractable = interactable;

        agent.SetDestination(currentInteractable.interactionLocation.position);
        agent.isStopped = false;
        animator.SetFloat(hashSpeedPara, 1f);
    }

    private IEnumerator WaitForInteraction()
    {
        handleInput = false;

        yield return inputHoldWait;

        while (animator.GetCurrentAnimatorStateInfo(0).tagHash != hashLocomotionTag)
        {
            yield return null;
        }

        handleInput = true;
    }
}


//using System.Collections;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class PlayerMovement : MonoBehaviour
//{
//    public Animator animator;
//    public NavMeshAgent2D agent;
//    public SaveData playerSaveData;
//    public float turnSmoothing = 15f;
//    public float inputHoldDelay = .5f;

//    private Interactable currentInteractable;
//    private Vector2 destinationPosition;
//    private Vector3 previousPosition;
//    private bool handleInput = true;
//    private WaitForSeconds inputHoldWait;

//    private readonly int hashSpeedPara = Animator.StringToHash("Speed");
//    private readonly int hashLocomotionTag = Animator.StringToHash("Locomotion");

//    public const string startingPositionKey = "starting position";

//    private const float stopDistanceProportion = .1f;
//    private const float navMeshSampleDistance = 4f;

//    private void Start()
//    {
//        inputHoldWait = new WaitForSeconds(inputHoldDelay);

//        string startingPositionName = "";
//        playerSaveData.Load(startingPositionKey, ref startingPositionName);
//        Transform startingPosition = StartingPosition.FindStartingPosition(startingPositionName);

//        transform.position = startingPosition.position;
//        transform.rotation = startingPosition.rotation;

//        destinationPosition = transform.position;
//        previousPosition = transform.position;
//    }

//    private void Update()
//    {
//        if (agent.pathPending)
//            return;

//        float speed = (!agent.isStopped) ? 0f : 1f;

//        if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion)
//        {
//            Stopping(out speed);
//        }
//        else
//        {
//            Moving();
//        }
//    }

//    private void Stopping(out float speed)
//    {
//        agent.isStopped = true;

//        transform.position = destinationPosition;

//        speed = 0f;

//        if (currentInteractable)
//        {
//            transform.rotation = currentInteractable.interactionLocation.rotation;

//            currentInteractable.Interact();
//            currentInteractable = null;

//            StartCoroutine(WaitForInteraction());
//        }
//    }

//    private void Moving()
//    {
//        Vector3 direction = transform.position - previousPosition;

//        Quaternion targetRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
//        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, .5f);
//    }

//    public void OnGroundClick(BaseEventData data)
//    {
//        if (!handleInput)
//            return;

//        currentInteractable = null;

//        destinationPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//        //PointerEventData pData = (PointerEventData)data;

//        //NavMeshHit hit;
//        //if (NavMesh.SamplePosition(pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas))
//        //{
//        //    destinationPosition = hit.position;
//        //}
//        //else
//        //{
//        //    destinationPosition = pData.pointerCurrentRaycast.worldPosition;
//        //}

//        //agent.SetDestination(destinationPosition);
//        agent.isStopped = false;
//    }

//    public void OnInteractableClick(Interactable interactable)
//    {
//        if (!handleInput)
//            return;

//        currentInteractable = interactable;

//        Debug.Log(currentInteractable.name);
//        Debug.Log(currentInteractable.interactionLocation.position);

//        agent.SetDestination(currentInteractable.interactionLocation.position);
//        agent.isStopped = false;
//    }

//    private IEnumerator WaitForInteraction()
//    {
//        handleInput = false;

//        yield return inputHoldWait;

//        while (animator.GetCurrentAnimatorStateInfo(0).tagHash != hashLocomotionTag)
//        {
//            yield return null;
//        }

//        handleInput = true;
//    }
//}