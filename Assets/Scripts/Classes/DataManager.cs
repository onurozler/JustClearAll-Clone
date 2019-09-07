using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    // Data Manager is using for serializing and saving Player as binary format and deserialize it.

    public class DataManager
    {
        // Path to save data
        private const string SAVE_PATH = "/Save.dat";

        // Saving Player's data as binary
        public static string SaveData(Player player)
        {
            FileStream file = null;

            // Serialize and saving player's data at specific location.
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Create(Application.persistentDataPath + SAVE_PATH);
                bf.Serialize(file, player);

                return "Data was saved at" + Application.persistentDataPath + SAVE_PATH;
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
        }

        // Loading Player's data and Returning data as Player
        public static Player LoadData()
        {
            FileStream file = null;

            // Deserialize player's data and casting it as Player class.
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Open(Application.persistentDataPath + SAVE_PATH, FileMode.Open);
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
        }
    }
}
