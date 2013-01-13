using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.SaveLoad
{
    public class SmartStorage<SaveGameStructure>
    {
         static StorageDevice device;
         static StorageContainer container;

        public static void Init()
        {
            StorageDevice.DeviceChanged += new EventHandler<EventArgs>(StorageDevice_DeviceChanged);
        }
        private static void initDevice()
        {
            AsyncCallback ac = new AsyncCallback(GetDevice);
            Object stateobj = (Object)"GetDevice for Player One";
            StorageDevice.BeginShowSelector(PlayerIndex.One, ac, stateobj);
        }

       static   void   GetDevice(IAsyncResult result)
        {
            device = StorageDevice.EndShowSelector(result);
        }

       static   void   StorageDevice_DeviceChanged(object sender, EventArgs e)
        {
            device = null;
        }
        // If a save is pending, save as soon as the
        // storage device is chosen

        private  static Stream openStorage(int slot,bool load=false)
        {
                initDevice();
            while (device == null)
            {
                //showloading screen
                Console.WriteLine("test");
            }
            // Open a storage container.
            IAsyncResult result = device.BeginOpenContainer("WarTornLands", null, null);
            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();
            container = device.EndOpenContainer(result);
            // Close the wait handle.
            result.AsyncWaitHandle.Close();
            string filename = "WarTornLands" + slot + ".sav";

            // Check to see whether the save exists.
            if (load)
            {
                if (!container.FileExists(filename))
                {
                    // If not, dispose of the container and return.
                    container.Dispose();
                    return null;
                }
            }
            else
            {
                if (!container.FileExists(filename))
                {
                    // If not, dispose of the container and return.
                    container.CreateFile(filename);
                }
            }
            // Open the file.
            return container.OpenFile(filename, FileMode.Open,FileAccess.ReadWrite);
        }

        public static void Save(int slot, SaveGameStructure data)
        {
            try
            {
                Stream stream = openStorage(slot);
                if (stream == null)
                    throw new Exception("Stream to storage was not found");
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SaveGameStructure));
                    serializer.Serialize(stream, data);
                }
                finally
                {
                    stream.Close();
                    stream.Dispose();
                }
                // Close the file.
            }
            finally
            {
                // Dispose the container, to commit changes.
                container.Dispose();
            }
        }


        public static  SaveGameStructure Load(int slot)
        {
            SaveGameStructure data = default(SaveGameStructure);
            try
            {
           
                Stream stream = openStorage(slot, true);
                if (stream == null)
                    throw new Exception("Stream to storage was not found");
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SaveGameStructure));
                    data = (SaveGameStructure)serializer.Deserialize(stream);
                }
                finally
                {
                    // Close the file.
                    stream.Close();
                    stream.Dispose();
                }
            }
            finally
            {
                // Dispose the container.
                container.Dispose();
            }
            return data;
        }
    }
}
