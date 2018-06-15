using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kanonskogelScript : MonoBehaviour {

	// code onder 'start' voert elke keer uit als het script geladen word (1 maal per object)
	void Start () {
        Invoke("verhoogZwaartekracht", 10f); // roep functie 'verhoogZwaartekracht' aan over 10 seconden (dit is gemaakt zodat kogels niet vast komen te zitten)
    }

    // functie verhoogZwaartekracht
    private void verhoogZwaartekracht()
    {
        GetComponent<Rigidbody2D>().gravityScale = GetComponent<Rigidbody2D>().gravityScale * 10; // verhoog de huidige zwaartekracht in het rigidbody2D element 10 maal (maakt de kogel heel zwaar zodat hij sneller valt)
        Invoke("springOmhoog", 10f); // roep functie springOmhoog aan over 10 seconden (als het verzwaren van de zwaartekracht niet genoeg is geven we de kogel een duwtje)
    }

    // functie springOmhoog
    private void springOmhoog()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse); // geef de kogel een 'duw' naar boven zodat hij terug begint te bewegen
        Invoke("verwijderKogel", 10f); // roep functie 'verwijderKogel' aan over 10 seconden (als het duwtje en zwaartekracht verhogen niet helpen, verwijderen we de kogel gewoon)
    }

    private void verwijderKogel()
    {
        Destroy(this.gameObject); // verwijder de kogel van het scherm
    }
}
