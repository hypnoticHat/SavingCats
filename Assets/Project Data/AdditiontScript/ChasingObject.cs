using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Watermelon
{
    public class ChasingObject : MonoBehaviour
    {
        [SerializeField] private float initialSpeed = 3f;
        [SerializeField] private float maxSpeed = 7.5f;
        [SerializeField] private float accelerationDuration = 5f;
        [SerializeField] private Vector3 moveDirection = Vector3.forward;
        [SerializeField] private float checkPlayerDistance = 1f;
        [SerializeField] private TextMeshProUGUI animalCountText;
        [SerializeField] private GameObject gameoverCanvas;
        [SerializeField] private AnimalCounter animalCounter;


        private float currentSpeed;
        private float accelerationRate;
        private bool hasReachedMaxSpeed = false;

        private void Start()
        {
            currentSpeed = initialSpeed;
            accelerationRate = (maxSpeed - initialSpeed) / accelerationDuration;
            moveDirection = moveDirection.normalized;
        }

        private void Update()
        {
            HandleAcceleration();
            transform.position += moveDirection * currentSpeed * Time.deltaTime;
        }

        private void HandleAcceleration()
        {
            if (!hasReachedMaxSpeed)
            {

                currentSpeed += accelerationRate * Time.deltaTime;

                if (currentSpeed >= maxSpeed)
                {
                    currentSpeed = maxSpeed;
                    hasReachedMaxSpeed = true;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerLose();
            }
        }

        private void PlayerLose()
        {
            gameoverCanvas.SetActive(true);
            animalCountText.text = $"total rescue cat: {animalCounter.animalNum}/{animalCounter.totalAnimals}";
            Time.timeScale = 0;
        }
    }
}
