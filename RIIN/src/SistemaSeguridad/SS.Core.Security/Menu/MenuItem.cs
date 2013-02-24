using System.Collections.Generic;

namespace SS.Core.Security.Menu
{
    public class MenuItem
    {
        public MenuItem() { Chose = new List<Chose>(); }

        public string PathImg { get; set; }
        public string Title { get; set; }
        public IList<Chose> Chose { get; set; }
    }
}
