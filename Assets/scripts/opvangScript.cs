using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opvangScript : MonoBehaviour {

    // functie OnTriggerEnter2D word elke keer aangeroepen als een object botst met de kist, dit is een standaard functie van unity
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "fri3dlogo")
        {
            GameObject[] kistenLijst; ; // maakt een array voor alle kist objecten in te plaatsen
            kistenLijst = GameObject.FindGameObjectsWithTag("kist"); // zoekt alle kist objecten
            // haal 5 willekeurige kisten weg
            for (int i = 0; i < 5; i++)
            {
                kistenLijst[Random.Range(0, kistenLijst.Length)].GetComponent<kistScript>().verwijderDezeKist(); // roep de 'verwijderdezekist' functie aan op deze kist
                GameObject.Find("blokkenZetter").GetComponent<blokkenScript>().score = GameObject.Find("blokkenZetter").GetComponent<blokkenScript>().score + 100; // verhoog de score in het blokkenScript met 100 punten
            }           
        }
        Destroy(other.gameObject); // verwijder het object waarmee we botsen (hopelijk enkel kanonskogels ;)
    }
}
