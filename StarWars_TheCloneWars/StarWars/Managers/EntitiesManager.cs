using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace StarWars.Managers
{
    public sealed class EntitiesManager
    {
        #region Attributs
        private static EntitiesManager _instance;
        static readonly object instanceLock = new object();
        public static EntitiesManager Instance
        {
            get
            {
                if (_instance == null) //Les locks prennent du temps, il est préférable de vérifier d'abord la nullité de l'instance.
                {
                    lock (instanceLock)
                    {
                        if (_instance == null) //on vérifie encore, au cas où l'instance aurait été créée entretemps.
                            _instance = new EntitiesManager();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Tous les personnages
        /// </summary>
        private static List<Type> troops = GetTypesInNamespaceEnumerable("StarWars.Entities.Implements.Childs").ToList();

        /// <summary>
        /// Tous les personnages jouables
        /// </summary>
        public static List<Type> Playables;

        /// <summary>
        /// Tous les personnages non-jouables
        /// </summary>
        public static List<Type> Nonplayables;

        /// <summary>
        /// Tous les personnages jouables avec Nom et Description
        /// </summary>
        public static List<Tuple<string, string>> Playablesstats;

        #endregion

        #region Ctor
        private EntitiesManager ()
        { }
        #endregion

        #region Methods
        public static IEnumerable<Type> GetTypesInNamespaceEnumerable(string nameSpace)
        {
            return
            AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.IsClass && t.Namespace == nameSpace);
        }

        public void InitCharactersTypes()
        {            
            Playablesstats = new List<Tuple<string, string>>();
            XDocument xml = XDocument.Parse(
                 File.ReadAllText(Path.Combine(Tools.Tools.DataFolder, "PJs.xml"), System.Text.Encoding.UTF8));
            XContainer list = xml.Element("PJs");         
            List<string> playablesnames = new List<string>();
            foreach (XElement element in list.Elements())
            {
                playablesnames.Add(element.Element("Name").Value);
                Playablesstats.Add(Tuple.Create(element.Element("Name").Value.ToString(), element.Element("Description").Value.ToString()));
            }
            Playables = troops.Where(e => playablesnames.Contains(e.Name)).ToList();
            Nonplayables = troops.Where(e => !playablesnames.Contains(e.Name)).ToList();
        }

        #endregion
    }
}
