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

        public static IEnumerable<Type> GetTypesInNamespaceEnumerable(string nameSpace)
        {
            return
            AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.IsClass && t.Namespace == nameSpace);
        }

        public static void InitCharactersTypes()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(Path.Combine(Tools.Tools.DataFolder, "PJs.xml"));
            XmlNodeList list = xml.SelectNodes("PJs/PJ");

            List<string> playablesnames = new List<string>();
            foreach (XmlElement element in list)
                playablesnames.Add(element["Name"].InnerText);

            Playables = troops.Where(e => playablesnames.Contains(e.Name)).ToList();
            Nonplayables = troops.Where(e => !playablesnames.Contains(e.Name)).ToList();
        }

    }
}
