using System.Xml.Serialization;

namespace Core
{
    public class Item
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; } = "N/A";
        [XmlElement(ElementName = "Code")]
        public string Code { get; set; } = "N/A";
        [XmlElement(ElementName = "Date")]
        public string Date { get; set; } = "N/A";
    }
}