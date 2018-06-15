using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //<- nodig omdat we met UI elementen gaan werken (tekst, canvas, ...)
using UnityEngine.SceneManagement; //<- nodig omdat we scenes gaan veranderen (als men op de stopknop drukt)

public class blokkenScript : MonoBehaviour {
    public float blokkenPerRonde = 0.5f; // hoeveel blokken per ronde?
    public int ronde = 0; // welke ronde is het nu?
    public GameObject kistPrefab = null; // waar vind ik de kist prefab?
    public GameObject munitiePrefab = null; // waar vind ik de munitie?
    public int score = 0; // wat is de score
    private List<int> mogelijkePlaatsen = new List<int>(); // maakt een lege lijst aan

    // code onder functie 'start' voert elke keer uit als de scene geladen word. (1x per spel dus)
    void Start () {
        //maak een nieuwe lijst aan (functie)
        maakLijst();
        //start nieuwe ronde (functie)
        nieuweRonde();
        //begin met luisteren of er op de 'stop' knop gedrukt word
        Button stopKnop = GameObject.Find("stopKnop").GetComponent<Button>(); // bepaald wat de stopknop is (zoekt element wat een 'button' is en wat 'stopKnop' noemt)
        stopKnop.onClick.AddListener(stopSpel); //als er op de knop gedrukt word, voer functie 'stopSpel' uit
    }
	
	// code onder 'update' word elke frame uitgevoerd (dus heel snel opnieuw en opnieuw)
	void Update () {
        GameObject.Find("aantalScore").GetComponent<Text>().text = score.ToString(); // zoek element genaamd 'aantalScore' en zet de tekst naar de variabele 'score'
        GameObject.Find("scoreScript").GetComponent<scorescript>().score = score; // zoek het element 'scoreScript' en zet de score variabele van het script naar de variabele 'score'
    }

    // functie stopSpel
    void stopSpel()
    {
        SceneManager.LoadScene(0); // verander de scene op het scherm naar scene 0 (zie build settings)
    }

    // functie maakLijst (deze lijst is nodig om te bepalen hoeveel plekken er zijn tussen de linker en rechter muur, en welke al bezet zijn om blokken te plaatsen)
    void maakLijst()
    {
        // loop 11 keer (van 0 tot 9) (er gaan 10 kistjes tussen de linker en rechter muur)
        for (int i = 0; i < 11; i++)
        {
            // maak een nieuwe plaats aan in de lijst met waarde 0 (0 = niet bezet, 1 = bezet)
            mogelijkePlaatsen.Add(0);
        }
    }

    // functie wisLijst
    void wisLijst()
    {
        // loop 11 keer:
        for (int i = 0; i < 11; i++)
        {
            // zet elke postie in de lijst op 0 (vrij)
            mogelijkePlaatsen[i] = 0;
        }
    }

    // functie rondeKlaar
    public void rondeKlaar()
    {
        //  we zijn klaar met schieten, nu wachten we tot alle kanonskogels van het scherm zijn alvorens we de volgende ronde starten
        if (GameObject.FindGameObjectsWithTag("kanonskogel").Length > 0) // als er meer als 0 kanonskogels op het scherm zijn...
        {
            // voer deze functie opnieuw uit over 1 seconde (zo controleren we elke seconde of alle kanonskogels binnen zijn)
            Invoke("rondeKlaar", 1f);
        }
        else // als er geen kanonskogels meer op het scherm zijn, dan:
        {
            GameObject.Find("kanon").GetComponent<kanonScript>().munitie = GameObject.Find("kanon").GetComponent<kanonScript>().maxMunitie; // zoek element 'kanon', en zet de munitie in het kanonScript terug op het maximum.
            GameObject.Find("aantalKogels").GetComponent<Text>().text = GameObject.Find("kanon").GetComponent<kanonScript>().maxMunitie.ToString(); // zoek het element aantalKogels en zet de tekst op het maximum
            GameObject.Find("kanon").GetComponent<kanonScript>().isAanHetSchieten = false; // vertel het kanonscript dat we niet meer aan het schieten zijn
            nieuweRonde(); // start volgende ronde
        }
    }

    //functie nieuweRonde
    public void nieuweRonde()
    {
        //wis de lijst met mogelijke plaatsen om blokken te plaatsen
        wisLijst();

        GameObject.Find("Canvas").BroadcastMessage("alleBlokkenZakken"); // roep naar alle objecten oner 'Canvas' dat alle blokken moeten zakken (elk script dat hier iets mee moet doen hoort dit en voert een functie uit)
        //verhoog ronde met 1 (had ook met ronde = ronde + 1 gekunnen, maar dit is iets sneller ;))
        ronde++;
        //zoek hoeveel blokken we moeten plaatsen deze ronde (nooit halve blokken, dus we ronden naar boven af tot hele getallen.
        int blokkenDezeRonde = Mathf.CeilToInt(blokkenPerRonde * ronde); // blokkenDezeRonde = blokken per ronde maal 'ronde', en dit afgerond naar boven
        int blokkenVolgendeRonde = Mathf.CeilToInt(blokkenPerRonde * (ronde + 1)); // blokkenVolgendeRonde = blokken per ronde maal 'ronde' + 1, afgerond naar boven
        GameObject.Find("aantalKisten").GetComponent<Text>().text = blokkenVolgendeRonde.ToString(); // zet tekst van object 'aantalKisten' naar blokkenVolgendeRonde

        //Zet blokken
        float positieX = 0f; // positie op de X - as
        int teller = 0; // maak een getal 'teller' en zet de waarde op 0

        // loop: van 0 tot blokkenDezeRonde
        for (int i = 0; i < blokkenDezeRonde; i++)
        {
            positieX = GameObject.Find("muurLinks").transform.position.x + 0.5f + bepaalPositie() * 0.52f; // positieX is, de X waarde van muurLinks PLUS een waarde uit de lijst met mogelijke plaatsen, MAAL 0.52f (breedte van een kist)
            GameObject dezeKist = Instantiate(kistPrefab, new Vector2(positieX, (GameObject.Find("plafond").transform.position.y - 0.5f)), this.transform.rotation); // maak een nieuw object van de kistPrefab, net onder het plafond
            dezeKist.name = "kist"; // naam van deze kist = 'kist'
            dezeKist.transform.parent = this.transform.parent; // zorg dat deze kist onder hetzelfde object komt als dit object (moet onder 'Canvas' komen)
            dezeKist.GetComponent<kistScript>().kistSterkte = ronde; // de sterkte van deze kist is gelijk aan de ronde
            dezeKist.GetComponent<kistScript>().vernieuwTekst(); // vertel het script in de kist om zijn tekst te updaten
            teller++; // verhoog de teller met 1
            
            //als we aan het maximum blokken per rij zitten, wis de lijst en laat alle blokken nog een keer zakken
            if (teller == 11)
            {               
                teller = 0; // zet de teller op 0
                wisLijst(); // wis de lijst
                GameObject.Find("Canvas").BroadcastMessage("alleBlokkenZakken"); // vertel alle scripts onder 'Canvas' dat alle blokken moeten zakken
            }

        }


        // plaats een munitie kogel op het scherm
        // bepaal de positie van deze kogel (willekeurige positie)
        positieX = Random.Range((GameObject.Find("muurLinks").transform.position.x + 0.5f), (GameObject.Find("muurRechts").transform.position.x - 0.5f)); // positie op de X as is willekeurig tussen de linkermuur en rechtermuur
        float positieY = Random.Range(GameObject.Find("plafond").transform.position.y, GameObject.Find("eindelijn").transform.position.y); // positie op de Y as is willekeurig tussen het plafond en de eindelijn
        GameObject dezeKogel = Instantiate(munitiePrefab, new Vector2(positieX, positieY), this.transform.rotation); // maak een nieuwe munitie kogel en plaats op de positie die we gekozen hebben
        dezeKogel.name = "munitie"; // noem deze kogel 'munitie'

        //verplaats het kanon naar een nieuwe willekeurige plek
        positieX = Random.Range((GameObject.Find("muurLinks").transform.position.x + 0.5f), (GameObject.Find("muurRechts").transform.position.x - 0.5f)); // de positie op de X as is willekeurig tussen de linker en rechtermuur
        GameObject.Find("kanon_basis").transform.position = new Vector2(positieX, GameObject.Find("kanon_basis").transform.position.y); // plaats de basis van het kanon op deze positie
        GameObject.Find("kanon").transform.position = new Vector2(positieX, GameObject.Find("kanon").transform.position.y); // plaats het kanon terug op zijn basis
    }

    //functie bepaalpositie (geeft een nummer terug)
    public int bepaalPositie()
    {
        // bepaal waar de blok moet komen op de X-as (willekeurig)
        int plaatsInLijst = 0; // begin met 0
        plaatsInLijst = (int)Random.Range(0, 11); // kies een willekeurige plaats in de lijst
        if (mogelijkePlaatsen[plaatsInLijst] != 0) // als deze plaats niet gelijk is aan 0 (vrij), roep de functie nogmaals aan
        {
            return (bepaalPositie()); // roept de functie nogmaals aan
        }
        else // als de plaats wel gelijk is aan 0 (vrij)
        {
            mogelijkePlaatsen[plaatsInLijst] = 1; // zet deze plaats in de lijst op 1 (bezet)
            return (plaatsInLijst); // geeft de plaats in de lijst terug
        }
    }

    // functie alleBlokkenZakken (word aangeroepen als alle blokken 1 rij moeten zakken, maar dit script doet er niets mee)
    public void alleBlokkenZakken()
    {
        // doe niets :)
    }
}
