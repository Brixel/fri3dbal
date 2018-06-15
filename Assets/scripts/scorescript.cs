using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //<- omdat we van scene gaan veranderen
using UnityEngine.UI; //<- omdat we UI elementen gaan aansrepeken

public class scorescript : MonoBehaviour {
    public int score = 0; // maak een variabele score

    // code onder functie 'start' voert elke keer uit als het object geladen word
    void Start()
    {
        Screen.SetResolution(640, 960, false); // forceer de resolutie zodat het op alle schermen hetzelfde uitziet.
        // omdat we dit script over alle scenes willen meenemen, zorgen we ervoor dat als het aangeroepen word op index 0 (het menu), we erbij vertellen dat dit object niet verwijderd mag worden
        if (SceneManager.GetActiveScene().buildIndex == 0) // als de huidige scene index 0 heeft:
        {
            // verwijder dit object niet
            DontDestroyOnLoad(this);
        }

    }

    // code onder 'update' word elke frame uitgevoerd (dus heel snel opnieuw en opnieuw)
    void Update () {
        // als de scene 2 is (game over): update het score label, en verwijder dit object
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            GameObject.Find("aantalScore").GetComponent<Text>().text = score.ToString(); // zoek 'aantalScore' object en zet tekst
            Destroy(this.gameObject); // verwijder dit object
        }
    }
}
