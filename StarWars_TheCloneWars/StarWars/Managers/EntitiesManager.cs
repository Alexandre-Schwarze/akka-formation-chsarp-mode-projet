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
    public static class EntitiesManager
    {
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

        public static List<Tuple<string, string>> Playablesstats;

        public static IEnumerable<Type> GetTypesInNamespaceEnumerable(string nameSpace)
        {
            return
            AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.IsClass && t.Namespace == nameSpace);
        }

        public static void InitCharactersTypes()
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
    }
}
