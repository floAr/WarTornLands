using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Serialization;
using System.IO; 

namespace WarTornLands
{
    /*
     * Anleitung :
     * 1. Definition des Filename mit SetFilename(Filename). Filename : Level_Filename.sav
     * 2. LoadLevel ausführen
     * 3. GetLevel ausführen um Level zu bekommen.
     */
    public struct GameLevelData
    {
        public string _ebene_0_Grundtextur;
        public string _ebene_1_untereObjekts;
        public string _ebene_2_obereObjekts;
        public string _unitsposition;
        public string _ebenengroeße;
    }

    class XML_Parser
    {
        #region Variablen

        StorageDevice _storagedevice;
        string _filename;
        Game _game;

        IAsyncResult _result;

        GameLevelData _level;
        #endregion 

        #region Konstruktor

        public XML_Parser(Game game)
        {
            _game = game;
        }
        #endregion

        #region Funktionen

        public void SetFilename(String name)
        {
            _filename = name;
        }

        public bool LoadLevel()
        {
            Initialise();

            _filename = "Level_" + _filename;

            //Open a storage container
            IAsyncResult result = _storagedevice.BeginOpenContainer("Level", null, null);

            //Wait for the WaitHandle to become signaled
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = _storagedevice.EndOpenContainer(result);

            //Close the wait handle.
            result.AsyncWaitHandle.Close();

            _filename = _filename + ".sav";

            //Check to see whether the save exists.
            if (!container.FileExists(_filename))
            {
                //If not, dispose of the container an return.
                container.Dispose();
                return false;
            }

            //Open the file.
            Stream stream = container.OpenFile(_filename, FileMode.Open);

            XmlSerializer serializer = new XmlSerializer(typeof(GameLevelData));

            _level = (GameLevelData)serializer.Deserialize(stream);

            //Close the file.
            stream.Close();

            //Dispose the Container.
            container.Dispose();

            return true;
        }

        public Level GetLevel()
        {
            // Variablen
            
            String[] splitvektor;
            String[] split;

            // Ermittlung Ebenengroeße
            split = _level._ebenengroeße.Split(';');
            int[,] levelgroeße = new int[split.Length, 2];

            for (int i = 0; i < split.Length; i++)
            {
                splitvektor = split[i].Split(',');
                levelgroeße[i, 0] = int.Parse(splitvektor[0]);
                levelgroeße[i, 1] = int.Parse(splitvektor[1]);
            }

            // Ermittlung Ebene0
            split = _level._ebene_0_Grundtextur.Split(';');
            int[,] ebene0 = new int[levelgroeße[0, 0], levelgroeße[0, 1]];

            for (int i = 0; i < levelgroeße[0, 0]; i++)
            {
                splitvektor = split[i].Split(',');
                for (int j = 0; j < levelgroeße[0, 1]; j++)
                {
                    ebene0[i, j] = int.Parse(splitvektor[j]);
                }
            }

            // Ermittlung Ebene1
            split = _level._ebene_1_untereObjekts.Split(';');
            int[,] ebene1 = new int[levelgroeße[1, 0], levelgroeße[1, 1]];

            for (int i = 0; i < levelgroeße[1, 0]; i++)
            {
                splitvektor = split[i].Split(',');
                for (int j = 0; j < levelgroeße[1, 1]; j++)
                {
                    ebene1[i, j] = int.Parse(splitvektor[j]);
                }
            }

            // Ermittlung Ebene2
            split = _level._ebene_2_obereObjekts.Split(';');
            int[,] ebene2 = new int[levelgroeße[2, 0], levelgroeße[2, 1]];

            for (int i = 0; i < levelgroeße[2, 0]; i++)
            {
                splitvektor = split[i].Split(',');
                for (int j = 0; j < levelgroeße[2, 1]; j++)
                {
                    ebene2[i, j] = int.Parse(splitvektor[j]);
                }
            }

            // Erzeugung von Liste mit Einheiten

            Level level = new Level(_game, ebene0, ebene1, ebene2);

            return level;
        }

        public void Initialise()
        {            
            _result = StorageDevice.BeginShowSelector(null, null);
            _storagedevice = StorageDevice.EndShowSelector(_result);
        }

        #endregion
    }
}
