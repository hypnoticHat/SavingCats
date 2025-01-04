using UnityEngine;
namespace Watermelon
{
    public class BuffObject : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 100f; 
        [SerializeField] private float shrinkDuration = 1f;
        [SerializeField] private float speedIncreaseAmount = 1f;
        [SerializeField] private AudioClip pickupSound; 
        [SerializeField] private AudioSource audioSource;

        private bool isCollected = false;
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
        private void Update()
        {
            if (!isCollected)
            {
                transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isCollected)
            {
                isCollected = true;
                PlayerControll player = other.GetComponent<PlayerControll>();
                if (player != null)
                {
                    player.IncreaseSpeed(speedIncreaseAmount);
                }
                audioSource.PlayOneShot(pickupSound);
                StartCoroutine(ShrinkAndDestroy());
            }
        }

        private System.Collections.IEnumerator ShrinkAndDestroy()
        {
            float elapsedTime = 0f;
            Vector3 originalScale = transform.localScale;

            while (elapsedTime < shrinkDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / shrinkDuration;
                transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
