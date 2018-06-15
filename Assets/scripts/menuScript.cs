using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //<- omdat we UI elementen gaan aanspreken
using UnityEngine.SceneManagement; //<- omdat we van scene gaan veranderen

public class menuScript : MonoBehaviour {

    // code onder functie 'start' voert elke keer uit als het object geladen word.
    void Start () {
        Button btn_newGame = GameObject.Find("startKnop").GetComponent<Button>(); // zoek de start knop
        btn_newGame.onClick.AddListener(nieuwSpel); // luister of er geklikt word, als dat zo is, roep functie nieuwspel aan
        Button btn_exitgame = GameObject.Find("stopKnop").GetComponent<Button>(); // zoek stop knop
        btn_exitgame.onClick.AddListener(stopSpel); // luister of er geklikt word, als dat zo is, roep functie stopspel aan
    }

    // functie nieuw spel
    void nieuwSpel()
    {
        SceneManager.LoadScene(1); // laad scene 1 (spel)
    }

    // functie stop spel
    void stopSpel()
    {
        Application.Quit(); // sluit de applicatie
    }
}
