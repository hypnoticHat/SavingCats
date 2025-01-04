using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class AnimalBehavior : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private GameObject playerObj;
        [SerializeField] private float runAwayDistance = 5f;
        [SerializeField] private float followDistance = 1.5f;
        [SerializeField] private float followDelay = 3f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float followSpeed = 3f;
        [SerializeField] private float rotationSpeed = 5f;
        public bool isFollowing = false;
        private Vector3 moveDirection;
        [Header("UI Components")]
        [SerializeField] private Canvas waitingIndicatorCanvas;
        [SerializeField] private Image progressFillBar;
        [SerializeField] private AnimalCounter animalCounter;
        private PlayerControll playerControll;


        private enum State { Idle, RunAway, FollowPlayer }
        private State currentState = State.Idle;

        private void Start()
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
            player = playerObj.transform;
            playerControll = playerObj.GetComponent<PlayerControll>();

            if (progressFillBar != null) progressFillBar.fillAmount = 0f;
            if (waitingIndicatorCanvas != null) waitingIndicatorCanvas.gameObject.SetActive(false);
        }
        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            switch (currentState)
            {
                case State.Idle:
                    HandleIdleState(distanceToPlayer);
                    break;
                case State.RunAway:
                    HandleRunAwayState(distanceToPlayer);
                    break;
                case State.FollowPlayer:
                    HandleFollowPlayerState();
                    break;
            }
            if (distanceToPlayer > followDistance && !isFollowing)
            {
                progressFillBar.fillAmount -= Time.deltaTime / followDelay / 3;
                progressFillBar.fillAmount = Mathf.Clamp01(progressFillBar.fillAmount);

                if (progressFillBar.fillAmount < 0)
                {
                    waitingIndicatorCanvas.gameObject.SetActive(false);
                }
            }
            followSpeed = playerControll.GetMaxSpeed() - 1;
        }

        private void HandleIdleState(float distanceToPlayer)
        {
            if (distanceToPlayer <= runAwayDistance && !isFollowing)
            {

                currentState = State.RunAway;
            }
            else if (distanceToPlayer <= followDistance)
            {
                waitingIndicatorCanvas.gameObject.SetActive(true);

                if (progressFillBar != null)
                {
                    progressFillBar.fillAmount += Time.deltaTime / followDelay;
                    progressFillBar.fillAmount = Mathf.Clamp01(progressFillBar.fillAmount);

                    if (progressFillBar.fillAmount >= 1f)
                    {
                        StartFollowingPlayer();
                    }
                }
            }
        }

        private void HandleRunAwayState(float distanceToPlayer)
        {
            if (isFollowing)
            {
                currentState = State.FollowPlayer;
                return;
            }

            moveDirection = (transform.position - player.position).normalized;

            moveDirection.y = 0;

            transform.position += moveDirection * runSpeed * Time.deltaTime;

            RotateTowardsDirection(moveDirection);
            if (distanceToPlayer > runAwayDistance * 1.5f)
            {
                currentState = State.Idle;
            }
            else if (distanceToPlayer <= followDistance)
            {
                waitingIndicatorCanvas.gameObject.SetActive(true);

                if (progressFillBar != null)
                {
                    progressFillBar.fillAmount += Time.deltaTime / followDelay;
                    progressFillBar.fillAmount = Mathf.Clamp01(progressFillBar.fillAmount);

                    if (progressFillBar.fillAmount >= 1f)
                    {
                        StartFollowingPlayer();
                    }
                }
            }
        }

        private void HandleFollowPlayerState()
        {
            moveDirection = (player.position - transform.position).normalized;


            moveDirection.y = 0;


            transform.position += moveDirection * followSpeed * Time.deltaTime;


            RotateTowardsDirection(moveDirection);
        }

        private void RotateTowardsDirection(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void StartFollowingPlayer()
        {
            waitingIndicatorCanvas.gameObject.SetActive(false);
            isFollowing = true;
            currentState = State.FollowPlayer;
        }
    }
}
