using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.IO;

namespace UiC.Network.Protocol.Types
{
    public class Player
    {
        public const short Id = 10;

        public virtual short TypeId
        {
            get { return Id; }
        }

        public string Name;
        public int EntRef;
        public string XnAddr;
        public string Hwid;
        public string NewHwid;
        public long Guid;
        public string IP;

        public Player()
        {

        }

        public Player(string name, int entRef, string xnAddr, string hwid, string newHwid, long guid, string ip)
        {
            this.Name = name;
            this.EntRef = entRef;
            this.XnAddr = xnAddr;
            this.Hwid = hwid;
            this.NewHwid = newHwid;
            this.Guid = guid;
            this.IP = ip;
        }

        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(Name);
            writer.WriteInt(EntRef);
            writer.WriteUTF(XnAddr);
            writer.WriteUTF(Hwid);
            writer.WriteUTF(NewHwid);
            writer.WriteLong(Guid);
            writer.WriteUTF(IP);
        }

        public virtual void Deserialize(IDataReader reader)
        {
            Name = reader.ReadUTF();
            EntRef = reader.ReadInt();
            XnAddr = reader.ReadUTF();
            Hwid = reader.ReadUTF();
            NewHwid = reader.ReadUTF();
            Guid = reader.ReadLong();
            IP = reader.ReadUTF();
        }
    }
}
