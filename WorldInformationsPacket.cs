using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackResearch.Network.Models;

namespace BlackResearch.Network.Packets.Client
{
    class WorldInformationsPacket : Packet
    {
        public override string Name
        {
            get { return "WorldInformationsPacket"; }
        }

        private int unk;
        private long serverTime;
        private short serverCount;
        public static List<WorldServerInfo> WorldServers;


        public WorldInformationsPacket(BinaryPacket binaryPacket) : base(binaryPacket)
        {
            WorldServers = new List< WorldServerInfo>();
        }

        public override string ToString()
        {
            StringBuilder stringBuiler = new StringBuilder();
            
            return stringBuiler.ToString();
        }

        protected override void Deserialize()
        {
            BinaryPacket.SetPosition(0);
            BinaryPacket.ReadByteArray(7);                                          //Offset
            unk = BinaryPacket.ReadInt16();                                         //unk
            serverTime = BinaryPacket.ReadInt64();                                  //Server time
            serverCount = BinaryPacket.ReadInt16();                                 //Channel count
            for(int serverId = 0; serverId < serverCount; ++serverId)
            {
                WorldServerInfo wsi = new WorldServerInfo();
                wsi.ChannelId = BinaryPacket.ReadInt16();                           //Channel ID
                wsi.Id = BinaryPacket.ReadInt16();                                  //Server ID
                Int16 unkValue = BinaryPacket.ReadInt16();                          //unk
                wsi.ChannelName = BinaryPacket.ReadString(62, Encoding.Unicode);    //Channel name
                wsi.ServerName = BinaryPacket.ReadString(62, Encoding.Unicode);     //Server name

                BinaryPacket.ReadByte();                                            //unk
                wsi.ServerIp = BinaryPacket.ReadString(16, Encoding.ASCII);         //IP address
                BinaryPacket.ReadByte();                                            //unk
                BinaryPacket.ReadByteArray(84);                                     //unk
                wsi.ServerPort = BinaryPacket.ReadInt16();                          //Server port
                wsi.PopulationStatus = BinaryPacket.ReadByte();                     //Population status (light, crowded, etc)
                wsi.IsPublic = BinaryPacket.ReadByte();                             //Is publicly joinable
                BinaryPacket.ReadByte();                                            //unk
                wsi.CharacterCount = BinaryPacket.ReadByte();                       //Number of character own by the player on this channel
                wsi.CharacterDeleteCount = BinaryPacket.ReadByte();                 //Number of character in delete state on this channel
                BinaryPacket.ReadInt16();                                           //unk
                wsi.RelogingDelayTime = BinaryPacket.ReadInt64();                   //Reloging time allowed
                wsi.LastLoginTime = BinaryPacket.ReadInt64();                       //Last login time
                wsi.XpDropBonus = BinaryPacket.ReadByte();                          //Wp ordrop bonus available
                BinaryPacket.ReadByteArray(13);                                     //unk
                wsi.Medal = BinaryPacket.ReadByte();                                //Medal
            }
        }
    }
}
