using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace lab120
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Car transport = new Car { id = "86", Manufacturer = "gelic", Speed = 124 };

            Console.WriteLine("*******************BINAR********************************");
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream f = new FileStream("D:/ЛЕНННННА/C#/lab120/transport.dat", FileMode.OpenOrCreate)) //в качестве параметров принимает поток, куда помещает сериализованные данные 
            {
                formatter.Serialize(f, transport);

                Console.WriteLine("Объект сериализован");
            }
            using (FileStream f = new FileStream("D:/ЛЕНННННА/C#/lab120/transport.dat", FileMode.OpenOrCreate))
            {
                Car newtransport = (Car)formatter.Deserialize(f);

                Console.WriteLine("Объект десериализован");
                Console.WriteLine($"Speed: {newtransport.Speed} --- Id: {newtransport.id}");
            }
            Console.WriteLine("-----------------------------------------JSON-----------------------------------------------------------");
           
            var jsonSerializer = new DataContractJsonSerializer(typeof(Car));
            using (FileStream f = new FileStream("D:/ЛЕНННННА/C#/lab120/transport.json", FileMode.OpenOrCreate)) //DataContractJsonSerializer
            {
                jsonSerializer.WriteObject(f,transport);
            }
            using (FileStream f = new FileStream("D:/ЛЕНННННА/C#/lab120/transport.json", FileMode.OpenOrCreate))
            {
                Car trans1 = (Car)jsonSerializer.ReadObject(f);
                Console.WriteLine(trans1.Speed);
            }
            Console.WriteLine("------------------------------------------XML----------------------------------------------------------");
            
            XmlSerializer formatter1 = new XmlSerializer(typeof(Car));
            using (FileStream f = new FileStream("D:/ЛЕНННННА/C#/lab120/transport.xml", FileMode.OpenOrCreate)) //XmlSerializer
            {
                formatter.Serialize(f, transport);

                Console.WriteLine("Объект сериализован");
            }
            using (FileStream f = new FileStream("D:/ЛЕНННННА/C#/lab120/transport.xml", FileMode.OpenOrCreate))
            {
                Car trans2 = (Car)formatter.Deserialize(f);

                Console.WriteLine("Объект десериализован");
                Console.WriteLine($"Speed: trans2.Speed}} --- Id: {trans2.id}");
            }
            Console.WriteLine("----------------------------------------COLLECTION/MASSIVE------------------------------------------------------------");
        


            Car[] transports = new Car[5];
            transports[0] = new Car() { id = "555", Speed = 155 };
            transports[1] = new Car() { id = "5666", Speed = 15 };
            transports[2] = new Car() { id = "5", Speed = 245 };

            XmlSerializer xmlFormatter2 = new XmlSerializer(typeof(Car[]));
            using (FileStream file = new FileStream("D:/ЛЕНННННА/C#/lab120/transport.xml", FileMode.OpenOrCreate))
            {
                xmlFormatter2.Serialize(file, transports);
            }
            using (FileStream file = new FileStream("D:/ЛЕНННННА/C#/lab120/transport.xml", FileMode.Open))
            {
                Car[] newCars = (Car[])xmlFormatter2.Deserialize(file);
                for (int i = 0; transports[i] != null; i++)
                {
                    Console.WriteLine(newCars[i].Speed);
                }
            }

            //-------------------------------------------------------------------------------------
            //XPATH

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("D:/ЛЕНННННА/C#/lab120/transport.xml");    /// считать xml-документ из out.xml
            XmlElement xRoot = xDoc.DocumentElement;

            Console.WriteLine("\n\n\n\t\t\t  XPath for XML:\n");
            Console.WriteLine("All child nodes:");                              /// получить все дочерние записи 
            XmlNodeList childNodes = xRoot.SelectNodes("*");                    /// корневого элемента
            foreach (XmlNode n in childNodes)                                   /// и вывести их //позволяет выбирать элементы, соответствующие определенному селектору.
                Console.WriteLine(n.OuterXml);

            Console.WriteLine("\nAll <Names> nodes:");                          /// вывести все записи <Name>
            XmlNodeList namesNodes = xRoot.SelectNodes("//transports");
            foreach (XmlNode n in namesNodes)                                   
                Console.WriteLine(n.InnerText);                                 
         


            XDocument xdoc = new XDocument(new XElement("transports",
                   new XElement("transport",
                      new XAttribute("id", "555"),
                    new XElement("company", "autotrans")),
                        new XElement("transport",
                     new XAttribute("id", "963"),
                         new XElement("company", "autotransMinsk")),
                      new XElement("transport",
                     new XAttribute("id", "9"),
                     new XElement("company", "autotransGomel")),
                     new XElement("transport",
                      new XAttribute("id", "553"),
                       new XElement("company", "autotransBrest"))));
            xdoc.Save("transport.xml");
            var tr = from xe in xdoc.Element("transports").Elements("transport") where xe.Element("company").Value == "autotransMinsk" select xe;  //LINQ TO XML Value С просто извлечь из документа данные и обработать их
            foreach (XElement el in tr)
                Console.WriteLine($"id: {el.Attribute("id").Value}, company: {el.Element("company").Value}");
            Console.WriteLine("=====================================================\n");

        }
    }

   
        [Serializable]
    public class Vehicle
    {
        public int Speed;
    }
    [Serializable]
    public class Car : Vehicle
    {
        [NonSerialized]
        public string Manufacturer;
        public string id;
    }
}