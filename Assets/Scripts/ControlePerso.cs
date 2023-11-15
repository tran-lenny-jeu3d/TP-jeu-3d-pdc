using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ControlePerso : MonoBehaviour
{
    // Variables de contrôle du personnage
    public Rigidbody rigidBodyPerso;
    public float vitesseDeplacement;
    public float vitesseRotation;

    // Variables du compteur
    float nbrBurgers = 1;
    public TextMeshProUGUI compteurBurgers;

    // Variables gérant la vie
    float nbrViesActuel = 50;
    float nbrViesMax = 100;
    public Image barreVie;

    // Variable des soins
    float nbrBandages;
    public TextMeshProUGUI compteurBandages;

    //Variable bloquant les contrôles
    public bool blocageControles;

    // Variables des caméras
    public GameObject camSortie;
    public GameObject camPerso;

    //Variable de la zone de sortie
    public GameObject zoneSortie;

    // Start is called before the first frame update
    void Start()
    {
        rigidBodyPerso = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > 0)
        {
            GetComponent<Animator>().SetBool("course", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("course", false);
        }

        // Affichage du compteur et modification
        compteurBurgers.text = nbrBurgers.ToString();
        compteurBandages.text = nbrBandages.ToString();

        // Gestion de la barre de vie
        float pourcentageVie = nbrViesActuel / nbrViesMax;
        barreVie.fillAmount = pourcentageVie;

        // Gestion du fonctionnement du soin
        if (nbrViesActuel != 100 && nbrBandages != 0 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            nbrViesActuel += 25;
            nbrBandages -= 1;
        }
    }

    void FixedUpdate()
    {
        if (blocageControles == false)
        {
            //Déplacement du Boss selon les axes horizontal et vertical
            float axeH = Input.GetAxisRaw("Horizontal");
            float axeV = Input.GetAxisRaw("Vertical");

            // Calcul de la direction à laquelle le personnage fait face
            Vector3 direction = (transform.forward * axeV) + (transform.right * axeH);

            //Vélocité du personnage selon les axes horizontal et vertical
            GetComponent<Rigidbody>().velocity = direction.normalized * vitesseDeplacement;

            // Rotation du personnage
            float tourne = Input.GetAxis("Mouse X") * vitesseRotation;
            transform.Rotate(0, tourne, 0);
        }
        
    }
    private void OnTriggerEnter(Collider infoCollider)
    {
        if (infoCollider.gameObject.name == "HamburgerBoite")
        {
            Destroy(infoCollider.gameObject);
            nbrBurgers -= 1;
        }
        if (infoCollider.gameObject.name == "BandageBoite")
        {
            Destroy(infoCollider.gameObject);
            nbrBandages += 1;
        }
        if (infoCollider.gameObject.name == "HamburgerBoite" && nbrBurgers == 0)
        {
            Invoke("AfficherZoneSortie", 0f);
        }
        if (nbrBurgers == 0 && infoCollider.gameObject.name == "zone sortie")
        {
            Invoke("RelancerJeu", 0f);
        }
    }
    // Dans la version finale, le joeur sera renvoyé dans la scène de victoire à la place
    void RelancerJeu()
    {
        Scene sceneActuelle = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneActuelle.name);
    }
    void AfficherZoneSortie()
    {
        camPerso.SetActive(false);
        camSortie.SetActive(true);
        blocageControles = true;
        zoneSortie.SetActive(true);
        Invoke("DesafficherZoneSortie", 3f);
    }
    void DesafficherZoneSortie()
    {
        camPerso.SetActive(true);
        camSortie.SetActive(false);
        blocageControles = false;
    }
}
