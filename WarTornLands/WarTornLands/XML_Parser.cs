using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Serialization;
using System.IO;
using WarTornLands.EntityClasses; 

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
        public string _ebene_2_obereObjekts;
        public string _unitsposition;   
        public string _ebenengroeße;
    }

    public struct GameDialogData
    {
        public string _einmaligegespraeche;
        public string _standardtext;
    }

    class XML_Parser
    {
        #region Variablen

        StorageDevice _storagedevice;
        string _filename;
        GameDialogData _dialog;
        Game _game;

        IAsyncResult _result;

        GameLevelData _level;
        #endregion 

        #region Konstruktor

        public XML_Parser(Game game)
        {
            this._game = game;
        }
        #endregion

        #region Funktionen

        public void SetFilename(String name)
        {
            _filename = name;
        }

        public void SaveText()
        {
            _dialog._einmaligegespraeche = "Ich bin ein Zwerg.,Du nicht!;Tötet die Zwerge,Wir sind Zwerge";
            _dialog._standardtext = "Eine Axt im Haus erspart den Zimmerman!";

            Initialise();

            String typ = "Dialog";

            //Open a storage container

            _result = _storagedevice.BeginOpenContainer(typ, null, null); // hier lässt sich der Pfad setzen


            typ = typ + "_";

            //Wait for the WaitHandle to become signaled
            _result.AsyncWaitHandle.WaitOne();

            StorageContainer container = _storagedevice.EndOpenContainer(_result);

            //Close the wait handle.
            _result.AsyncWaitHandle.Close();

            //Check to see whether the save exists

            if (container.FileExists(typ + _filename + ".sav"))
            {
                //Delete it so that we can create one fresh.
                container.DeleteFile(typ + _filename + ".sav");
            }

            // Create the file

            Stream stream = container.CreateFile(typ + _filename + ".sav");

            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer;

            serializer = new XmlSerializer(typeof(GameDialogData));
            serializer.Serialize(stream, _dialog);


            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
        }

        public bool LoadText()
        {
            Initialise();

            _filename = "Dialog_" + _filename;

            //Open a storage container
            _result = _storagedevice.BeginOpenContainer("Dialog", null, null);

            //Wait for the WaitHandle to become signaled
            _result.AsyncWaitHandle.WaitOne();

            StorageContainer container = _storagedevice.EndOpenContainer(_result);

            //Close the wait handle.
            _result.AsyncWaitHandle.Close();

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

            XmlSerializer serializer = new XmlSerializer(typeof(GameDialogData));

            _dialog = (GameDialogData)serializer.Deserialize(stream);

            //Close the file.
            stream.Close();

            //Dispose the Container.
            container.Dispose();

            return true;
        }

        public bool LoadLevel()
        {
            Initialise();

            _filename = "Level_" + _filename;

            //Open a storage container
            _result = _storagedevice.BeginOpenContainer("Level", null, null);

            //Wait for the WaitHandle to become signaled
            _result.AsyncWaitHandle.WaitOne();

            StorageContainer container = _storagedevice.EndOpenContainer(_result);

            //Close the wait handle.
            _result.AsyncWaitHandle.Close();

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

        public void SetLevel()
        {
            string level = "0,0,0,0,1,1,0,0;0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0";
            _level._ebene_0_Grundtextur = level;
            _level._ebene_2_obereObjekts = level;
            _level._ebenengroeße = "8,8;8,8;8,8";
            _level._unitsposition = "0,0;6,6";
        }

        public void SaveLevel()
        {
            Initialise();
            
            String typ = "Level";
            
            //Open a storage container
            
            _result = _storagedevice.BeginOpenContainer(typ, null, null); // hier lässt sich der Pfad setzen


            typ = typ + "_";

            //Wait for the WaitHandle to become signaled
            _result.AsyncWaitHandle.WaitOne();

            StorageContainer container = _storagedevice.EndOpenContainer(_result);

            //Close the wait handle.
            _result.AsyncWaitHandle.Close();

            //Check to see whether the save exists

            if (container.FileExists(typ + _filename + ".sav"))
            {
                //Delete it so that we can create one fresh.
                container.DeleteFile(typ + _filename + ".sav");
            }

            // Create the file
            
            Stream stream = container.CreateFile(typ + _filename + ".sav");

            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer;
            
            serializer = new XmlSerializer(typeof(GameLevelData));
            serializer.Serialize(stream, _level);
            

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
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
            split = _level._unitsposition.Split(';');
            List<Entity> units = new List<Entity>();
            Entity unit;

            Level level = new Level(_game);

            for (int i = 0; i < split.Length; i++)
            {
                splitvektor = split[i].Split(',');
                Vector2 vektor = new Vector2(int.Parse(splitvektor[0]), int.Parse(splitvektor[1]));
                switch (i)
                {
                    case 0:
                        unit = new EntityGruselUte(_game, vektor, (_game as Game1)._gruselUteTexture);
                        break;
                    case 1:
                        unit = new EntityTree(_game, vektor, (_game as Game1)._treeTexture);
                        break;
                    default:
                        unit = new EntityJumpPoint(_game, vektor, (_game as Game1)._blackHoleTexture,
                            level, new Vector2(500,500));
                        break;
                }
                level.AddDynamics(unit);
            }

            /*level.AddLayer(0, ebene0);
            level.AddLayer(2, ebene2);*/
            level.AddFloor(ebene0);
            level.AddCeiling(ebene2);

            return level;
        }

        public List<String> GetDialouge(bool modus, int textstelle)
        {
            List<String> dialogue = new List<string>();
            if (modus)
            {
                String[] split = _dialog._einmaligegespraeche.Split(';');
                String[] splittext = split[textstelle].Split(',');
                for (int i = 0; i < splittext.Length; i++)
                {
                    dialogue.Add(splittext[i]);
                }
            }
            else
            {
                dialogue.Add(_dialog._standardtext);
            }

            return dialogue;
        }

        public void Initialise()
        {            
            _result = StorageDevice.BeginShowSelector(null, null);
            _storagedevice = StorageDevice.EndShowSelector(_result);
        }

        #endregion
    }
}
