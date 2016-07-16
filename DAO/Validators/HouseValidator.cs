using System.Collections.Generic;
using System.Linq;
using Validator;

namespace DAO.Validators
{
    public class HouseValidator : Validator<House>
    {

        public HouseValidator()
        {
            var roomChk = new Validation<House>().SetProperty(h => h.Rooms).SetValidater(h => h.Rooms != null && h.Rooms.Count() >= 1).SetErrorMessage("Collection must have at least one element");

            var valChk = new ValidatorValidation<House>()
            {
                NestedPropertyType = typeof(IEnumerable<Room>),
                NestedPropertyValidatorInstance = new RoomValidator()
            }.SetNestedPropertyAccessor(h => h.Rooms);
            
            this.AddValidation(roomChk);
            this.AddValidatorValidation(valChk);
        }
        
    }
}
