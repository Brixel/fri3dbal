using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //<- dit is nodig omdat we van scene gaan veranderen


public class gameOverScript : MonoBehaviour {

    // code onder 'start' voert elke keer uit als het script geladen word (1 maal per object)
    void Start () {
        Invoke("terugNaarMenu", 5f); // roep functie terugNaarMenu aan over 5 seconden
    }


    // functie terugNaarMenu
    private void terugNaarMenu()
    {
        SceneManager.LoadScene(0); // laad scene 0 (menu)
    }
}
