using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class House
    {
        private readonly IEnumerable<Room> _rooms;

        public House(IEnumerable<Room> rooms)
        {
            if(rooms == null)
            {
                throw new ArgumentNullException("rooms");
            }

            _rooms = rooms;
        }

        public IEnumerable<Room> Rooms
        {
            get
            {
                return _rooms;
            }
        }
    }
}
