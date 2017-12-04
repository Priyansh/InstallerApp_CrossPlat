using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallerApp_CrossPlat.Droid
{
    public interface IPartsInfo
    {
        string CabinetName { get; set; }
        string LFinish { get; set; }
        string RFinish { get; set; }
    }
}