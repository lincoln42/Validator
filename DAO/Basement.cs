using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class Basement : Room
    {
        private readonly uint? _undergroundLevel;

        public Basement(Cieling cieling, Floor floor, IEnumerable<Wall> walls, IEnumerable<DoorWay> doorways, uint? undergroundLevel):base(cieling, floor, walls, doorways)
        {
            _undergroundLevel = undergroundLevel;
        }

      public uint? UndergroundLevel { get { return _undergroundLevel; } }
    }
}
