using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //<- dit is nodig omdat we UI elementen gaan aanspreken (tekst bijvoorbeeld)
using UnityEngine.SceneManagement; //<- dit is nodig omdat we van scene gaan veranderen

public class kistScript : MonoBehaviour {

    public int kistSterkte = 10; // bepaald de sterkte van de kist in punten
    private bool stopMetLuisteren = false; // gaan we gebruiken om tijdelijk de boxCollider2D te doen stoppen met het luisteren naar botsingen
    public GameObject fri3dprefab = null; // waar vinden we het fri3d prefab object?

    // code onder 'start' voert elke keer uit als het script geladen word (1 maal per object)
    void Start ()
    {
        vernieuwTekst(); // roep de functie vernieuwTekst aan
    }

    // functie vernieuwTekst
    public void vernieuwTekst()
    {
        GetComponentInChildren<Text>().text = kistSterkte.ToString(); // verander de tekst in de kist naar de sterkte van de kist
    }

    // functie OnTriggerEnter2D word elke keer aangeroepen als een object botst met de kist, dit is een standaard functie van unity
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!stopMetLuisteren) // als stopMetLuisteren 'false' is:
        {
            if (other.name == "kanonskogel") // als het object waarmee we botsen 'kanonskogel' noemt:
            {
                stopMetLuisteren = true; // stop even met luisteren
                kistSterkte--; // kist sterkte - 1
                GameObject.Find("blokkenZetter").GetComponent<blokkenScript>().score++; // verhoog de score in het blokkenScript met 1
                vernieuwTekst(); // vernieuw de tekst in deze kist
                if (kistSterkte == 0) // als de kist sterkte 0 is:
                {
                    GameObject.Find("blokkenZetter").GetComponent<blokkenScript>().score = GameObject.Find("blokkenZetter").GetComponent<blokkenScript>().score + 100; // verhoog de score in het blokkenScript met 100 punten
                    // we hebben een willekeurige kans van 1 op 10 om een fri3dlogo te krijgen: 
                    int kans = Random.Range(0, 10); // bereken kans
                    if(kans == 5) // als kans 5 is
                    {
                        GameObject ditLogo = Instantiate(fri3dprefab, this.transform.position, this.transform.rotation); // plaats een fri3dlogo op het scherm, waar deze kist staat
                        ditLogo.name = "fri3dlogo";
                    }

                    Invoke("verwijderDezeKist", 0.02f); // roep de functie verwijderDezeKist aan over 0.02 seconden (zo zie je de kist eerst op 0 sterkte komen, en dan wegfloepen
                }

                Invoke("beginTerugMetLuisteren", 0.01f); // roep de functie beginTerugMetLuisteren aan over 0.01 seconden zodat we opnieuw kunnen botsen
            }
        }
    }

    // functie beginTerugMetLuisteren
    private void beginTerugMetLuisteren()
    {
        stopMetLuisteren = false; // zet stopMetLuisteren terug op false zodat we weer kunnen botsen
    }

    // functie verwijderDezeKist
    public void verwijderDezeKist()
    {
        Destroy(this.gameObject); // verwijderd dit object
    }

    // functie alleBlokkenZakken (word aangeroepen als alle blokken 1 rij moeten zakken)
    public void alleBlokkenZakken()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 0.45f); // verlaag deze kist op de Y as met 0.45 (hoogte van een kist)

        if (this.transform.position.y < GameObject.Find("eindelijn").transform.position.y) // als de Y positie van deze kist onder de eindelijn komt...
        {
            Debug.Log("game over!"); // schrijf 'game over!' naar de log (console)
            SceneManager.LoadScene(2); // laad scene 2 (game over scherm)
        }
    }
}
