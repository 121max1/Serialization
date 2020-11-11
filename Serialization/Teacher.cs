using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Serialization
{
    [Serializable]
    public class Teacher : Student
    {
        public string AcademicDegree { get; set; }
        
        public string Subject { get; set; }

    }

    public class Teachers
    {
        [XmlArray("Collection"), XmlArrayItem("Item")]
        public List<Teacher> Collection { get; set; }
    }
}
