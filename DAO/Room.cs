using System.Collections.Generic;

namespace DAO
{
    public class Room
    {
        private readonly Cieling _cieling;

        private readonly Floor _floor;

        private readonly IEnumerable<Wall> _walls;

        private readonly IEnumerable<DoorWay> _doorWays;

        public Room(Cieling cieling, Floor floor, IEnumerable<Wall> walls, IEnumerable<DoorWay> doorWays)
        {
            _cieling = cieling;
            _floor = floor;
            _walls = walls;
            _doorWays = doorWays;
        }

        public Cieling Cieling { get { return _cieling; } }

        public Floor Floor {get { return _floor; } }

        public IEnumerable<Wall> Walls { get { return _walls; } }
             
        public IEnumerable<DoorWay> DoorWays { get { return _doorWays; } }
    }
}
