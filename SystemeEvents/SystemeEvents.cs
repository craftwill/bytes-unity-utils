using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class SystemeEvents : MonoBehaviour {

    //SINGLETON : Vérification de l'existence d'une seule instance de se script à la fois dans le programme
    #region SINGLETON
    static SystemeEvents _instance; //Référence privée de l'instance
    public static SystemeEvents Instance //Référence public de l'instance
    {
        get
        {
            //Si la référence de l'instance est null...
            if (!_instance)
            {
                //Trouver celle-ci et l'enregistrer
                _instance = FindObjectOfType(typeof(SystemeEvents)) as SystemeEvents;
                //Si la référence de l'instance n'est pas null...
                if (_instance)
                {
                    //Initialiser une nouvelle instance du dictionaire d'évènements
                    _instance.Init();
                }
            }
            return _instance; //Retouner la référence
        }
    }

    void Awake()
    {
        //DontDestroyOnLoad(this);
    }
    #endregion

    //Dictionaire d'évènements, contient les différents listes d'évènements du système
    Dictionary<string, Action<InfoEvent>> dictionaireEvents;

    /*
     * Fonction d'initialisation du dictionaire
     * @param void
     * @return void
     */
    void Init()
    {
        //Si le dictionaire n'éxiste pas...
        if (dictionaireEvents == null)
        {
            //Créer un nouvelle intance
            dictionaireEvents = new Dictionary<string, Action<InfoEvent>>();
        }
    }

    /*
     * Fonction d'abonnement à une des listes d'évènements du dictionaire
     * @param NomEvent nomEvent
     * @param Action<InfoEvent> fonc
     * @return void
     */
    public void AbonnementEvent(string nomEvent, Action<InfoEvent> fonc)
    {
        Action<InfoEvent> cetEvent; //Nouvelle instance par défaut de la classe d'information
        //Si l'évènement existe dans le dictionaire...
        if (Instance.dictionaireEvents.TryGetValue(nomEvent, out cetEvent))
        {
            //Ajout du script dans la liste éxistente
            cetEvent += fonc;
            Instance.dictionaireEvents[nomEvent] = cetEvent;
        }
        //Sinon...
        else
        {
            //Ajout du script dans une nouvelle liste
            cetEvent += fonc;
            Instance.dictionaireEvents.Add(nomEvent, cetEvent);
        }
    }

    /*
     * Fonction de désabonnement à une des listes d'évènements du dictionaire
     * @param NomEvent nomEvent
     * @param Action<InfoEvent> fonc
     * @return void
     */
    public void DesabonnementEvent(string nomEvent, Action<InfoEvent> fonc)
    {
        Action<InfoEvent> cetEvent; //Nouvelle instance par défaut de la classe d'information
        //Si l'évènement existe dans le dictionaire...
        if (Instance.dictionaireEvents.TryGetValue(nomEvent, out cetEvent))
        {
            //Retrait du script dans la liste éxistente
            cetEvent -= fonc;
            Instance.dictionaireEvents[nomEvent] = cetEvent;
        }
    }

    /*
     * Fonction de lancement d'un évènement
     * @param NomEvent nomEvent
     * @param InfoEvent infoEvent
     * @return void
     */
    public void LancerEvent(string nomEvent, InfoEvent infoEvent)
    {
        Action<InfoEvent> cetEvent = null; //Nouvelle instance vide de la classe d'information
        //Si l'évènement existe dans le dictionaire...
        if (Instance.dictionaireEvents.TryGetValue(nomEvent, out cetEvent))
        {
            //Éxécute toutes les fonctions abonnées à l'évènement
            cetEvent.Invoke(infoEvent);
        }
    }
}
