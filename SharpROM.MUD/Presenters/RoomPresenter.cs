using SharpROM.MUD.Models;
using SharpROM.MUD.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Presenters
{
    public class RoomPresenter : Presenter
    {
        public RoomViewModel CreateViewModel(List<object> models)
        {
            RoomViewModel room = CreateViewModel<RoomViewModel>(models);
            Room r = (Room)models[0];
            //todo, parse colors here?  should we do that just before output?
            room.LongDescription = r.LongDescription;
            room.Name = r.Name;
            return room;
        }
    }
}
