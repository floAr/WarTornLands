using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;

namespace WarTornLands.Infrastructure.Systems.SaveLoad
{
   public class SmartStorage<SaveGameStructure>
    {
       StorageDevice device = new StorageDevice();
       StorageContainer container;
       private Stream openStorage(int slot)
       {
           // Open a storage container.
           IAsyncResult result = device.BeginOpenContainer("WarTornLands", null, null);
           // Wait for the WaitHandle to become signaled.
           result.AsyncWaitHandle.WaitOne();
           container = device.EndOpenContainer(result);
           // Close the wait handle.
           result.AsyncWaitHandle.Close();
           string filename = "WarTornLands"+slot+".sav";

           // Check to see whether the save exists.
           if (!container.FileExists(filename))
           {
               // If not, dispose of the container and return.
               container.Dispose();
               return null;
           }
           // Open the file.
           return container.OpenFile(filename, FileMode.Open);
       }

       public void Save(int slot, SaveGameStructure data)
       {
           Stream stream = openStorage(slot);
           if (stream == null)
               throw new Exception("Stream to storage was not found");
           XmlSerializer serializer = new XmlSerializer(typeof(SaveGameStructure));
           serializer.Serialize(stream, data);
           // Close the file.
           stream.Close();
           // Dispose the container, to commit changes.
           container.Dispose();
       }


       public SaveGameStructure Load(int slot)
       {
           Stream stream = openStorage(slot);
           if (stream == null)
               throw new Exception("Stream to storage was not found");
           XmlSerializer serializer = new XmlSerializer(typeof(SaveGameStructure));
           SaveGameStructure data = (SaveGameStructure)serializer.Deserialize(stream);
           // Close the file.
           stream.Close();
           // Dispose the container.
           container.Dispose();
           return data;
       }
    }
}
