using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAO;
using DAO.Validators;
using System.Linq;
using System.Collections.Generic;

namespace Validator.Tests
{
    [TestClass]
    public class HouseTests
    {
        private IValidator<House> _validator;

        [TestInitialize]
        public void TestSetup()
        {
            _validator = new HouseValidator();
        }

        [TestCleanup]
        public void TestTearDown()
        {
            _validator = null;
        }

        [TestMethod]
        public void HouseHasAtLeastOneRoom()
        {
            var haus = new House(new List<Room>());

            var res = _validator.Validate(haus);

            Assert.IsTrue(res.Any(r => r.PropertyName == "Rooms"), "Invalid property not found");
        }

        [TestMethod]
        public void HouseInvalidRoomFound()
        {
            var haus = new House(new List<Room>
            {
                new Room(new Cieling(), new Floor(), new List<Wall> { new Wall() }, new List<DoorWay> { new DoorWay() }),
                new Room(new Cieling(), null, new List<Wall> { new Wall() }, new List<DoorWay> { new DoorWay() })
            });



            var res = _validator.Validate(haus);

            Assert.IsTrue(res.Any(r => r.PropertyName == "Rooms[1].Floor"), "Invalid property not found");
        }
    }
}
