using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadRunMap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(0);
        }
    }

}
