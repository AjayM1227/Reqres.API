using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reqres.APITests.Utils.Exceptions
{
    public class FaultMessage : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name => base["name"] as string;

        [ConfigurationProperty("text", IsRequired = true)]
        public string Text => this["text"] as string;
    }

    public class FaultMessageCollection : ConfigurationElementCollection
    {
        public FaultMessageCollection() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public FaultMessage this[int index]
        {
            get { return BaseGet(index) as FaultMessage; }
            set
            {
                if (BaseGet(index) != null)
                { BaseRemoveAt(index); }
                BaseAdd(index, value);
            }
        }

        public new FaultMessage this[string key] => BaseGet(key) as FaultMessage;

        protected override ConfigurationElement CreateNewElement()
        {
            return new FaultMessage();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FaultMessage)element).Name;
        }
    }

    public class FaultMessageConfigurationSection : ConfigurationSection
    {
        private static FaultMessageConfigurationSection _section;
        public const string DefaultSectionName = "FaultMessageConfiguration";

        public static FaultMessageConfigurationSection GetSection()
        {
            return _section ??
                   (_section =
                       ConfigurationManager.GetSection(DefaultSectionName) as FaultMessageConfigurationSection);
        }

        [ConfigurationProperty("FaultMessages", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(FaultMessage), AddItemName = "FaultMessage")]
        public FaultMessageCollection FaultMessages => base["FaultMessages"] as FaultMessageCollection;
    }

    public static class FaultMessageConfigurationHelper
    {
        public static string GetMessage(string key)
        {
            var faultMessageConfiguration = (FaultMessageConfigurationSection)ConfigurationManager.GetSection("FaultMessageConfiguration");
            return faultMessageConfiguration.FaultMessages[key] != null ? faultMessageConfiguration.FaultMessages[key].Text : string.Empty;
        }
    }
}
