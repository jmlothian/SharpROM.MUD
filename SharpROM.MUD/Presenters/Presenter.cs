using SharpROM.MUD.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Presenters
{
    public class Presenter
    {
        public TViewModel CreateViewModel<TViewModel>(List<object> models) where TViewModel : IViewModel, new()
        {
            TViewModel vm = new TViewModel();
            vm.Models = models;
            return vm;
        }
        
    }
}
