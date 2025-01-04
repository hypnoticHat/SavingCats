using UnityEngine;
using TMPro;

namespace Watermelon
{
    public class AnimalCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI animalCountText;
        [SerializeField] public int animalNum = 0;
        [SerializeField] public int totalAnimals;


        private void Update()
        {
            UpdateAnimalCount();
        }

        private void UpdateAnimalCount()
        {
            GameObject[] allAnimals = GameObject.FindGameObjectsWithTag("Animal");
            totalAnimals = allAnimals.Length;
            int followingAnimals = 0;
            foreach (GameObject animal in allAnimals)
            {
                AnimalBehavior animalBehavior = animal.GetComponent<AnimalBehavior>();
                if (animalBehavior != null && animalBehavior.isFollowing)
                {
                    followingAnimals++;
                }
            }
            animalNum = followingAnimals;


            animalCountText.text = $"{followingAnimals}/{totalAnimals}";
        }
    }
}
