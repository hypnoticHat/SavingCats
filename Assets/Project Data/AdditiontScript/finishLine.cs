using TMPro;
using UnityEngine;
namespace Watermelon
{
    public class finishLine : MonoBehaviour
    {
        [SerializeField] private GameObject WinCanvas;
        [SerializeField] private AnimalCounter animalCounter;
        [SerializeField] private TextMeshProUGUI animalCountText;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerWin();
            }
        }

        private void PlayerWin()
        {
            WinCanvas.SetActive(true);
            animalCountText.text = $"total rescue cat: {animalCounter.animalNum}/{animalCounter.totalAnimals}";
            Time.timeScale = 0;
        }
    }
}