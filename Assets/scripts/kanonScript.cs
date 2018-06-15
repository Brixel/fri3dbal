using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kanonScript : MonoBehaviour {
    private GameObject deRichtLijn; // bepaald wat de richtlijn is
    public GameObject kanonskogelPrefab = null; // waar vind ik de kanonskogel prefab?
    public int maxMunitie = 10; // wat is het maximum aan muntie dat we kunnen houden?
    public bool isAanHetSchieten = false; // zijn we aan het schieten?
    public int munitie = 0; // hoeveel munitie hebben we nog over?

    // de functie FixedUpdate is iets accurater dan 'update' en is een standaard unity functie die de hele tijd herhaald word.
    void FixedUpdate()
    {
        if (!isAanHetSchieten && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > this.transform.position.y) // als we niet aan het schieten zijn, en de muiscursor staat hoger dan het kanon, dan...
        {
            // ik weet zelf niet helemaal wat deze code doet, het zorgt ervoor dat het kanon in de richting van de muis 'richt'
            #region richt kanon
            // zoek de workdpoint coordinaten van de cursor
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // zet de Z waarde naar 0 (we werken in 2D, niet 3D)
            pz.z = 0;
            // zoek de hoek in radialen
            float AngleRad = Mathf.Atan2(pz.y - transform.position.y, pz.x - transform.position.x);
            // zoek de hoek in graden
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            // draai het kanon
            this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
            #endregion
            #region richtlijn
            // plaats de richtlijn
            deRichtLijn = GameObject.Find("richtlijn"); // zoek object 'richtlijn'
            deRichtLijn.transform.position = new Vector2(this.transform.position.x, this.transform.position.y); // plaats richtlijn op dezelfde plek als het kanon
            deRichtLijn.transform.rotation = this.transform.rotation; // draai de richtlijn in dezelfde richting als het kanon
            deRichtLijn.transform.position = deRichtLijn.transform.position + (deRichtLijn.transform.right / 4); // zorg dat de richtlijn op het einde van het kanon komt te staan
            #endregion
            #region input
            // luister of er geklikt word, zo ja: vuur!
            if (Input.GetButtonUp("Fire1") && !isAanHetSchieten) // als de knop 'fire1' (zie edit/project settings/input) ingedrukt is, en we zijn NIET aan het schieten...
            {
                isAanHetSchieten = true; // dan zijn we nu WEL aan het schieten
                munitie = maxMunitie; // de munitie is gelijk aan het maximum
                schietKanon(); // we roepen de functie schietKanon aan
            }
            #endregion
        }
    }

    // functie schietKanon
    private void schietKanon()
    {
        if (munitie > 0) // als de munitie meer als 0 is...
        {
            GameObject dezeKogel = Instantiate(kanonskogelPrefab, this.transform.position, this.transform.rotation); // we maken een nieuwe kanonskogel aan op basis van de prefab
            dezeKogel.GetComponent<Rigidbody2D>().AddForce(dezeKogel.transform.right * 10, ForceMode2D.Impulse); // we 'schieten' deze kogel af door hem een duw (van 10 * positie.rechts) te geven, verhoog of verlaag indien gewenst.
            dezeKogel.name = "kanonskogel"; // we noemen deze kogel 'kanonskogel'
            munitie--; // de munitie verminderen we met 1,
            Invoke("schietKanon", 0.1f); // en we voeren het script opnieuw uit na 0.1 seconde (verhoog of verlaag om sneller/trager te schieten)
        }
        else // als de kogels op zijn...
        {
            GameObject.Find("blokkenZetter").GetComponent<blokkenScript>().rondeKlaar(); // vertel het blokkenScript dat we klaar zijn met schieten
        }
    }
}
