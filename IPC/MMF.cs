using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using System.Threading;
using System.IO;

namespace IPC
{
    

    // Memory-mapped file API
    // Main usage for Unity Game Engine
    public class MMF
    {
        
        // File configurations
        private MemoryMappedFile File;
        public enum DataType
        {
            DataBool,
            DataByte,
            DataByteArray,
            DataChar,
            DataCharArray,
            DataString
        }

        private string mapName;
        private long capacity;
        private bool created;
        private string additionalInformation;

        public bool Created
        {
            set
            {
                created = value;
            }
            get
            {
                return created;
            }
        }

        public string MapName
        {
            set
            {
                mapName = value;
            }
            get
            {
                return mapName;
            }
        }

        public long Capacity {
            set
            {
                capacity = value;
            }
            get
            {
                return capacity;
            }
        }

        public string AdditionalInformation
        {
            get
            {
                return additionalInformation;
            }
        }
        
        //public MMF(string mapName, long capacity)
        //{
        //    MapName = mapName;
        //    Capacity = capacity;
        //    try
        //    {
        //        File = MemoryMappedFile.CreateNew(MapName, Capacity);
        //        Created = true;
        //    }
        //    catch (Exception err)
        //    {   
        //        Created = false;
        //    }
        //    created = true;
        //}
        
        public void CreateNewFile(string MapName, long capacity)
        {
            this.MapName = MapName;
            this.Capacity = capacity;
            try
            {
                File = MemoryMappedFile.CreateOrOpen(this.MapName, Capacity);
                Created = true;
            }
            catch (Exception err)
            {
                Created = false;
                additionalInformation = err.Message;

            }
        }

        public void OpenExisting(string MapName)
        {
            if (Created)
            {
                return;
            }

            try
            {
                File = MemoryMappedFile.OpenExisting(MapName);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        public void DeleteMMF()
        {
            File.Dispose();
        }
        

#region AddInformation
        public void AddInformation(bool Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }

        public void AddInformation(byte Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(byte[] Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(char Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(char[] Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(decimal Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(double Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(Int16 Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(Int32 Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(Int64 Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(sbyte Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(Single Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(string Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(UInt16 Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(UInt32 Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(UInt64 Content)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
        public void AddInformation(string Content, bool clearFile = false)
        {
            using (MemoryMappedViewStream stream = File.CreateViewStream())
            {
                File.Dispose();
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Content);
            }
        }
#endregion
        public dynamic ReadContent(DataType type, int count = 0)
        {
            object Value = null;
            if (type == DataType.DataBool)
            {
                using (MemoryMappedViewStream stream = File.CreateViewStream())
                {
                    BinaryReader reader = new BinaryReader(stream);
                    return reader.ReadBoolean();
                }
            }
            else if (type == DataType.DataByte)
            {
                using (MemoryMappedViewStream stream = File.CreateViewStream())
                {
                    BinaryReader reader = new BinaryReader(stream);
                    return reader.ReadByte();
                }
            }
            else if (type == DataType.DataByteArray)
            {
                using (MemoryMappedViewStream stream = File.CreateViewStream())
                {
                    BinaryReader reader = new BinaryReader(stream);
                    return reader.ReadBytes(count);
                }
            }
            else if (type == DataType.DataString)
            {
                using (MemoryMappedViewStream stream = File.CreateViewStream())
                {
                    BinaryReader reader = new BinaryReader(stream);
                    return reader.ReadString();
                }
            }
            return null;
        }
    }
}
