using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;

namespace Toolkit
{
    // hihihihihihihihihi200
    public static class FileManager
    {
        public static void SaveData(this RawData rawData, string name = "streamData")
        {
            var formatter = new BinaryFormatter();
            var path = $"{Application.streamingAssetsPath}/{name}.gg";
            var stream = new FileStream(path, FileMode.Create);
            var data = new RawData(rawData);
            
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static RawData LoadData(string name = "streamData")
        {
            var path = $"{Application.streamingAssetsPath}/{name}.gg";

            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Create);

                var data = new RawData (formatter.Deserialize(stream) as RawData);
                return data;
            }
            else
            {
                Debug.LogWarning($"Data with name {name} doesnt exist");
                return null;
            }
        }
    }

    /// <summary>
    /// Edit this class data
    /// </summary>
    public class RawData
    {
        public readonly string name;
        public readonly float[] positions;

        public RawData(RawData x)
        {
            name = x.name;
            positions = x.positions;
        }
    }
}
