using FindFiddo_server.Entities;
using FindFiddo.Abstractions;
using FindFiddo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
namespace FindFiddo_server.Services
{
    public static class XMLservice
    {
        public static string generateXML(Organizacion orgn)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Organizacion));

            using (MemoryStream memoryStream = new MemoryStream())
            {

                serializer.Serialize(memoryStream, orgn);

                byte[] byteArray = memoryStream.ToArray();

                string base64String = Convert.ToBase64String(byteArray);

                return base64String;
            }
        }

        public static Organizacion deserializarXML(string xml)
        {
            byte[] xmlBytes = Convert.FromBase64String(xml);

            using (MemoryStream memoryStream = new MemoryStream(xmlBytes))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Organizacion));

                return (Organizacion)serializer.Deserialize(memoryStream);
            }
        }

        public static List<Traduccion> DeserializarTraduccionXML(string xml)
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Traduccion>));
                return (List<Traduccion>)serializer.Deserialize(memoryStream);
            }
        }
    }
}
