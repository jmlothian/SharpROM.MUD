using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.MVPVM
{
    public class ViewOutputInformation
    {
        public DateTime LastOutput { get; set; } = DateTime.Now;
        Queue<ViewModelBinding> ViewQueue { get; set; } = new Queue<ViewModelBinding>();
        Dictionary<string, Queue<ViewModelBinding>> SecondaryViews { get; set; } = new Dictionary<string, Queue<ViewModelBinding>>();
    }
}
