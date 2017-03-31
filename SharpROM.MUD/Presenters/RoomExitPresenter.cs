using SharpROM.MUD.Models;
using SharpROM.MUD.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Presenters
{
    public class RoomExitPresenter : Presenter
    {
        public ExitViewModel CreateViewModel(List<object> models)
        {
            ExitViewModel exits = CreateViewModel<ExitViewModel>(models);
            ExitCollection e = (ExitCollection)models[0];
            //todo, parse colors here?  should we do that just before output?
            exits.North = (e.North != null && e.North.OutVNUM != "" && e.North.Open == true) ? true : false;
            exits.South = (e.East != null && e.South.OutVNUM != "" && e.South.Open == true) ? true : false;
            exits.East = (e.South != null && e.East.OutVNUM != "" && e.East.Open == true) ? true : false;
            exits.West = (e.West != null && e.West.OutVNUM != "" && e.West.Open == true) ? true : false;
            exits.Up = (e.Up != null && e.Up.OutVNUM != "" && e.Up.Open == true) ? true : false;
            exits.Down = (e.Down != null && e.Down.OutVNUM != "" && e.Down.Open == true) ? true : false;
            return exits;
        }
    }
}
