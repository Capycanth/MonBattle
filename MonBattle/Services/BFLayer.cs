using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MonBattle.Services
{
    public static class BFLayer
    {
        private static string FILE_PATH_PREPEND = "/SaveData/";
        public static void SerializeToFile<T>(string fileName, T gameData)
        {
            try
            {
                // Serialize data and save to file
                using (FileStream fileStream = new FileStream(FILE_PATH_PREPEND + fileName, FileMode.Create))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, gameData);
                }
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize: " + e.Message);
            }
        }

        public static T SerializeToObject<T>(string fileName)
        {
            using (FileStream fileStream = new FileStream(FILE_PATH_PREPEND + fileName, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(fileStream);
            }
        }
    }
}
