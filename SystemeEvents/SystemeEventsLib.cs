namespace UnityEngine
{
    //Namespace personaliser pour la classe d'information du système d'évènement
    namespace SystemeEventsLib
    {
        //Signature des fonctions accepté dans le dictionaire d'évènements
        public delegate void Action<in InfoEvent>(InfoEvent infoEvent);

        //Classe d'informations des évènements
        public class InfoEvent {
            public GameObject Cible { get; set; }
            public Object CibleScript { get; set; }
            //Contructeur par défaut de la classe
            public InfoEvent(GameObject cible = null)
            {
                this.Cible = cible;
            }
        }
    }
}