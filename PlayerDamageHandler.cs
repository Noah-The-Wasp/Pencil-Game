using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamageHandler : MonoBehaviour
{

    public static float PlayerHealth = 150f;
    public bool TouchingEnemy;
    public GameObject Pencil;

    void FixedUpdate()
    {
        
        //Reduces the size of the player pencil when they take damage 

        if (TouchingEnemy == true)
        {

            PlayerHealth -= 0.1f;

            Vector3 TempScale = new Vector3(Pencil.transform.localScale.x, Pencil.transform.localScale.y, PlayerHealth);
            Pencil.transform.localScale = TempScale;

        }

        //Reloads the scene if the players health runs out 

        if (PlayerHealth < 1)
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            PlayerHealth = 150f;

        }

    }

    //Checks various trigger tags which are associated with different types of damage taking 

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {

            TouchingEnemy = true;

        }
        else if (other.gameObject.tag == "Scissors")
        {

            TouchingEnemy = true;

        }

        if (other.gameObject.tag == "Death")
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            PlayerHealth = 150f;

        }

    }

    //Stops damage when the player stops touching an enemy

    private void OnTriggerExit(Collider other)
    {

        TouchingEnemy = false;

    }

}
