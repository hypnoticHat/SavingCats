using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Watermelon
{
    public class PlayerControll : MonoBehaviour
    {
        // Constants
        public static readonly Vector3 STORAGE_OFFSET = new Vector3(0.0289999992f, 1.09800005f, -0.230000004f);
        public static readonly int RUN_HASH = Animator.StringToHash("Run");
        public static readonly int MOVEMENT_MULTIPLIER_HASH = Animator.StringToHash("Movement Multiplier");

        // Movement
        [SerializeField]public Transform playerSpawmPoint; 
        [SerializeField] private NavMeshAgent agent;
        private bool isRunning;
        private float speed = 0;
        public float maxSpeed = 5.0f; 
        private float acceleration = 10.0f;
        // Animation
        [SerializeField] private GameObject playerPrefab;
        private PlayerGraphics playerGraphics;
        private Animator playerAnimator;

        [SerializeField] private ParticleSystem stepParticleSystem;
        private Transform leftFootTransform;
        private Transform rightFootTransform;

        private Transform cameraTarget;
        private Vector3 cameraCurrentOffset;
        private float cameraResetTime;
        private float cameraOffsetSpeed = 2.0f;

        private Vector3 inputDirection;

        private void Start()
        {
            SpawnPlayer(playerSpawmPoint);
        }

        private void Update()
        {
            HandleInput();
            HandleMovement();
        }

        private void HandleInput()
        {
            if (SimpleJoystick.Instance.IsActive)
            {
                inputDirection = SimpleJoystick.Instance.FormatInput;
            }
            else
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                inputDirection = new Vector3(horizontal, 0, vertical).normalized;
            }
        }

        private void HandleMovement()
        {
            if (inputDirection.sqrMagnitude > 0.1f)
            {
                if (!isRunning)
                {
                    isRunning = true;
                    playerAnimator.SetBool(RUN_HASH, true);
                    speed = 0;
                }

                float maxAllowedSpeed = inputDirection.magnitude * maxSpeed;

                if (speed > maxAllowedSpeed)
                {
                    speed -= acceleration * Time.deltaTime;
                }
                else
                {
                    speed += acceleration * Time.deltaTime;
                }

                transform.position += inputDirection * Time.deltaTime * speed;
                playerAnimator.SetFloat(MOVEMENT_MULTIPLIER_HASH, speed / maxSpeed);

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(inputDirection), 0.2f);
            }
            else
            {
                if (isRunning)
                {
                    isRunning = false;
                    playerAnimator.SetBool(RUN_HASH, false);
                }

                if (Time.time >= cameraResetTime)
                {
                    cameraCurrentOffset = Vector3.Lerp(cameraCurrentOffset, Vector3.zero, cameraOffsetSpeed * Time.deltaTime);
                }
            }

            if (cameraTarget != null)
            {
                cameraTarget.position = transform.position + cameraCurrentOffset;
            }
        }

        private void SpawnPlayer(Transform spawnPosition)
        {

            GameObject playerObject = Instantiate(playerPrefab, spawnPosition.position, spawnPosition.rotation);
            playerObject.transform.SetParent(transform);

            playerGraphics = playerObject.GetComponent<PlayerGraphics>();


            playerAnimator = playerGraphics.GetComponent<Animator>();


            Transform leftFoot = playerAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
            if (leftFoot.childCount > 0)
            {
                leftFootTransform = leftFoot.GetChild(0);
            }

            Transform rightFoot = playerAnimator.GetBoneTransform(HumanBodyBones.RightFoot);
            if (rightFoot.childCount > 0)
            {
                rightFootTransform = rightFoot.GetChild(0);
            }


        }

        public void IncreaseSpeed(float amount)
        {
            maxSpeed += amount;
        }

        public void LeftFootParticle()
        {
            if (!isRunning)
                return;

            stepParticleSystem.transform.position = leftFootTransform.position - transform.forward * 0.4f;
            stepParticleSystem.Play();
        }

        public void RightFootParticle()
        {
            if (!isRunning)
                return;

            stepParticleSystem.transform.position = rightFootTransform.position - transform.forward * 0.4f;
            stepParticleSystem.Play();
        }

        public float GetMaxSpeed()
        {
            return maxSpeed;
        }
    }
}
