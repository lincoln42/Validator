using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class BedRoom : Room
    {
        private readonly IEnumerable<Window> _windows;

        public BedRoom(Cieling cieling, Floor floor, IEnumerable<Wall> walls, IEnumerable<DoorWay> doorways, IEnumerable<Window> windows):base(cieling, floor, walls, doorways)
        {
            _windows = windows;
        }

        public IEnumerable<Window> Windows
        {
            get { return _windows; }
        }
    }
}
