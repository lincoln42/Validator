using System.Linq;
using Validator;

namespace DAO.Validators
{
    public class RoomValidator : Validator<Room>
    {        
        public RoomValidator()
        {
            var chkCieling = new Validation<Room>().SetProperty(r => r.Cieling).SetValidater(r => r.Cieling != null).SetErrorMessage(@"Property cannot be null");

            var chkFloor = new Validation<Room>().SetProperty(r => r.Floor).SetValidater(r => r.Floor != null).SetErrorMessage(@"Property cannot be null");

            var chkWalls = new Validation<Room>().SetProperty(r => r.Walls).SetValidater(r => r.Walls != null && r.Walls.Count() >= 1).SetErrorMessage("Collection must have at least one element");

            var chkDoorWays = new Validation<Room>().SetProperty(r => r.DoorWays).SetValidater(r => r.DoorWays != null && r.DoorWays.Count() >= 1).SetErrorMessage("Collection must have at least one element");

            this.AddValidation(chkCieling);
            this.AddValidation(chkFloor);
            this.AddValidation(chkWalls);
            this.AddValidation(chkDoorWays);
        }
    }
}
