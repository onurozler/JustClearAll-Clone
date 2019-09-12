using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    // Data Manager is using for serializing and saving Player as binary format and deserialize it.

    public class DataManager
    {
        // Path to save data on PC
        private const string SAVE_PATH_PC = "/Save.dat";

        // Path to save data on Android
        private const string SAVE_PATH_ANDROID = "Save.bytes";

        // Saving Player's data as binary
        public static string SaveData(Player player)
        {

#if UNITY_STANDALONE
            FileStream file = null;

            // Serialize and saving player's data at specific location.
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Create(Application.persistentDataPath + SAVE_PATH_PC);
                bf.Serialize(file, player);

                return "Data was saved at" + Application.persistentDataPath + SAVE_PATH_PC;
            }
            // If it throws a catch, can be seen in console.
            catch (Exception e)
            {
                return "Data couldn't be saved!!!!";
            }
            // Closing file, when it is done.
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
#elif UNITY_ANDROID
            var fi = new System.IO.FileInfo(@"Assets//Resources//"+SAVE_PATH_ANDROID);

            try
            {
                // Open file stream, serialize the dictionary and write it to the bin file.
                using (var binaryFile = fi.Create())
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(binaryFile, player);
                    binaryFile.Flush();
                }

                return "succesfull!";
            }
            catch (System.Exception e)
            {
                throw e;
            }
#endif
        }

        // Loading Player's data and Returning data as Player
        public static Player LoadData()
        {
#if UNITY_STANDALONE
            FileStream file = null;

            // Deserialize player's data and casting it as Player class.
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Open(Application.persistentDataPath + SAVE_PATH_PC, FileMode.Open);
                Player player = bf.Deserialize(file) as Player;

                return player;
            }
            // If it throws a catch, returns null.
            catch (Exception e)
            {
                return null;
            }
            // Closing file, when it is done.
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
#elif UNITY_ANDROID
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(SAVE_PATH_ANDROID);
            TextAsset textAsset = Resources.Load(fileNameWithoutExtension) as TextAsset;

            try
            {
                // Open the bin file in byte format and read it directly to our dictionary
                using (Stream stream = new MemoryStream(textAsset.bytes))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Player player = formatter.Deserialize(stream) as Player;

                    Debug.Log("Read bin successful!");

                    return player;
                }


            }
            catch (System.Exception e)
            {
                Debug.Log("!!!Read bin unsuccessful!!" + e);
                return null;
            }

#endif
        }
    }
}
