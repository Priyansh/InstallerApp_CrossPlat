using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallerApp_CrossPlat
{
    public interface IIndividualRoomInfo
    {
        string Rooms { get; set; }
        string Style { get; set; }
        string Colour { get; set; }
        string Hardware { get; set; }
        string CounterTop { get; set; }
        int deliveryPhoto { get; set; }
        int installationPhoto { get; set; }
    }
}