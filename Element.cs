using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOD2_App
{
    [Serializable]
    public class Element : Item
    {
        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }
    }
}