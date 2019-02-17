using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project2
{
    class Data
    {
        const string xmlPath = @"../../Geographic.xml";

        private NetworkModel networkModel;

        public NetworkModel NetworkModel
        {
            get
            {
                if (networkModel == null)
                {
                    networkModel = DeserializeNetworkModel();
                }
                return networkModel;
            }
        }

        private NetworkModel DeserializeNetworkModel()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NetworkModel));
            using (FileStream fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                return (NetworkModel)serializer.Deserialize(fileStream);
            }
        }
    }
}
