using System;
using System.Collections.Generic;
using System.Text;
using Core.Interfaces;

namespace Infrastructure
{
    public class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
