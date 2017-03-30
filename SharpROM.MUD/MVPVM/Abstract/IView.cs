using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.MVPVM.Abstract
{
    //http://stackoverflow.com/questions/38247080/using-razor-outside-of-mvc-in-net-core
    public interface IView
    {
        string Render(IMUDOutputViewModel ViewModel);
    }
}
