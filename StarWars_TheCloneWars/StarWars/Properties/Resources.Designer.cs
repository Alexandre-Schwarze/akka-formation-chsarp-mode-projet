﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StarWars.Properties {
    using System;
    
    
    /// <summary>
    ///   Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    /// </summary>
    // Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    // à l'aide d'un outil, tel que ResGen ou Visual Studio.
    // Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    // avec l'option /str ou régénérez votre projet VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("StarWars.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
        ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à C:\temp\ClonesWars\Data\.
        /// </summary>
        internal static string DataPath {
            get {
                return ResourceManager.GetString("DataPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à &lt;?xml version = &quot;1.0&quot; encoding=&quot;ISO8859-1&quot; standalone=&quot;yes&quot; ?&gt;
        ///&lt;PJs&gt;
        ///	&lt;PJ&gt;
        ///		&lt;Name&gt;Anakin&lt;/Name&gt;
        ///		&lt;Description&gt;Jedi de l&apos;école Djem So, pratique un style offensif imprévisible &lt;/Description&gt;
        ///	&lt;/PJ&gt;
        ///	&lt;PJ&gt;
        ///		&lt;Name&gt;Dooku&lt;/Name&gt;
        ///		&lt;Description&gt;Seigneur Sith pratiquant du Makashii, un style léger et rapide&lt;/Description&gt;
        ///	&lt;/PJ&gt;
        ///	&lt;PJ&gt;
        ///		&lt;Name&gt;Dark_Sidious&lt;/Name&gt;
        ///		&lt;Description&gt;Seigneur Sith, maitre des éclairs &lt;/Description&gt;
        ///	&lt;/PJ&gt;
        ///	&lt;PJ&gt;
        ///		&lt;Name&gt;ObiWan&lt;/Name&gt;
        ///		&lt;Description&gt;Maitre Jedi de l&apos;école  [le reste de la chaîne a été tronqué]&quot;;.
        /// </summary>
        internal static string PJs {
            get {
                return ResourceManager.GetString("PJs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à C:\temp\ClonesWars\Saves\.
        /// </summary>
        internal static string SavePath {
            get {
                return ResourceManager.GetString("SavePath", resourceCulture);
            }
        }
    }
}
