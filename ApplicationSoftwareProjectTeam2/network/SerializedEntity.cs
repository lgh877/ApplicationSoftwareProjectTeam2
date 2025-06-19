using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.network
{
    public class SerializedEntity
    {
        public byte Id { get; set; }
        public byte EntityLevel { get; set; }
        public int X { get; set; }
        public int Z { get; set; }
        public byte ItemId1 { get; set; }
        public byte ItemId2 { get; set; }
        public byte ItemId3 { get; set; }
    }
}

