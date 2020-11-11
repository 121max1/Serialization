using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Serialization
{
    class Program
    {

        

        static void Main(string[] args)
        {
            List<Student> universityPeople = new List<Student>();

            universityPeople.Add(new Student() { Id = 0, FirstName = "Максим", LastName = "Шкодин", Patronymic = "Сергеевич" });
            universityPeople.Add(new Student() { Id = 1, FirstName = "Дмитрий", LastName = "Кузьмин", Patronymic = "Сергеевич" });
            universityPeople.Add(new Teacher()
            {
                Id = 2,
                FirstName = "Марина",
                LastName = "Портенко",
                Patronymic = "Сергеевна",
                AcademicDegree = "Старший_преподаватель",
                Subject = "ООП"
            });
            universityPeople.Add(new Teacher()
            {
                Id = 3,
                LastName = "Кудрина",
                FirstName = "Елена",
                Patronymic = "Вячеславовна",
                Subject = "Стуктуры_и_алгоритмы",
                AcademicDegree = "Доцент"

            });
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            BinarySerialization(universityPeople, binaryFormatter);
            BinaryDeserialization("outputBinary.mksn", binaryFormatter);
            JsonSerialization(universityPeople);
            JsonDeserialization("output.json");

            Teachers persons = new Teachers();
            persons.Collection = new List<Teacher>();
            foreach(var person in universityPeople)
            {
                if(person is Teacher teacher)
                {
                    persons.Collection.Add(teacher);
                }
                else
                {
                   persons.Collection.Add(new Teacher
                   {
                       Id = person.Id,
                       FirstName = person.FirstName,
                       LastName = person.LastName,
                       Patronymic = person.Patronymic,
                       AcademicDegree = "",
                       Subject = ""
                   });
                }
            }
            XMLSerialization(persons);
            XMLDeserialization("output.xml");

        }
        
        private static void BinarySerialization(List<Student> persons, BinaryFormatter binaryFormatter)
        {
            using (FileStream fileStream = new FileStream("outputBinary.mksn", FileMode.Create))
            {
                foreach(var person in persons)
                {
                    if (person is Student teacher)
                    {
                        binaryFormatter.Serialize(fileStream, teacher);
                    }
                    else
                    {
                        binaryFormatter.Serialize(fileStream, person);
                    }
                }
            }
        }
        private static void BinaryDeserialization(string filename, BinaryFormatter binaryFormatter)
        {
            
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                while(fs.Position != fs.Length)
                {
                    var person = binaryFormatter.Deserialize(fs);

                    if (person is Teacher teacher)
                    {
                        Console.WriteLine(teacher.Id + " " + teacher.LastName + " " + teacher.FirstName + " " +
                            teacher.Patronymic + " " + teacher.AcademicDegree + " " + teacher.Subject);
                    }
                    else if (person is Student student)
                    {
                        Console.WriteLine(student.Id + " " + student.LastName + " " + student.FirstName + " " +
                            student.Patronymic);
                    }
                }
            }    
        }

        private static void JsonSerialization(List<Student> persons)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter("output.json", false , encoding: Encoding.Unicode))
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.WriteStartArray();
                    foreach (var person in persons)
                    {
                        if (person is Student student)
                        {
                            jsonSerializer.Serialize(jsonWriter, student);
                        }
                        else if (person is Teacher teacher)
                        {
                            jsonSerializer.Serialize(jsonWriter, teacher);
                        }
                    }
                }
            }
        }

        private static void JsonDeserialization(string filename)
        {
            using (var reader = new StreamReader("output.json"))
            {
                string jsonString = reader.ReadToEnd();
                var personsFromJson = JsonConvert.DeserializeObject<List<Teacher>>(jsonString);

                foreach (var person in personsFromJson)
                {
                    Console.WriteLine(person.Id + " " + person.LastName + " " + person.FirstName + " " + person.Patronymic + " " + person.AcademicDegree + " " + person.Subject);
                }
            }
        }

        private static void XMLSerialization(Teachers persons)
        {
            using (FileStream fileStream = new FileStream("output.xml", FileMode.Create))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Teachers));
                xmlSerializer.Serialize(fileStream, persons);
            }
        }

        private static void XMLDeserialization(string filename)
        {
            using (FileStream fileStream = new FileStream(filename, FileMode.Open))
            {
                var xmlSerializer = new XmlSerializer(typeof(Teachers));
                var collection = (Teachers)xmlSerializer.Deserialize(fileStream);
                foreach (var person in collection.Collection)
                {
                    Console.WriteLine(person.Id + " " + person.LastName + " " + person.FirstName +" " + person.Patronymic+ " " + person.AcademicDegree + " " + person.Subject);
                }
            }
        }

    }
}
