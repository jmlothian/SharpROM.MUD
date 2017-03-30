using SharpROM.MUD.MVPVM.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.MVPVM
{
    public class ViewModelBinding
    {
        public string ViewName { get; set; } = "";
        public object ViewModel { get; set; } = null;
    }
}
