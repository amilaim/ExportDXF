
using Newtonsoft.Json;

using System.IO;

namespace DFXGenerator.JasonHelper
{
    public static class JsonSerialization
    {
        public static T ReadFromFile<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
