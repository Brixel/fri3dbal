using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //<- nodig omdat we van scene gaan veranderen

public class munitieScript : MonoBehaviour {
    private bool stopMetLuisteren = false; // gaan we gebruiken als we even willen stoppen met zoeken naar botsingen

    // code onder functie 'start' voert elke keer uit als het object geladen word
    void Start () {
        GameObject.Find("aantalKogels").GetComponent<Text>().text = GameObject.Find("kanon").GetComponent<kanonScript>().maxMunitie.ToString(); // zet tekst in aantalKogels naar het maximum dat we hebben
    }

    // functie OnTriggerEnter2D word elke keer aangeroepen als een object botst met de kist, dit is een standaard functie van unity
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!stopMetLuisteren) // indien we nog steeds aan het luisteren zijn...
        {
            stopMetLuisteren = true; // stop even met luisteren naar botsingen.
            if (other.name == "kanonskogel") // als het object waarmee we botsen 'kanonskogel' noemt dan...
            {
                GameObject.Find("kanon").GetComponent<kanonScript>().maxMunitie++; // verhoog de maximum munitie van het kanon met 1
                GameObject.Find("aantalKogels").GetComponent<Text>().text = GameObject.Find("kanon").GetComponent<kanonScript>().maxMunitie.ToString(); // zet de tekst onder aantalKogels naar het nieuwe maximum
                GameObject.Find("blokkenZetter").GetComponent<blokkenScript>().score = GameObject.Find("blokkenZetter").GetComponent<blokkenScript>().score + 10; // verhoog de score in het blokkenScript met 10
                Destroy(this.gameObject); // verwijder dit object
            }
        }
    }
}
