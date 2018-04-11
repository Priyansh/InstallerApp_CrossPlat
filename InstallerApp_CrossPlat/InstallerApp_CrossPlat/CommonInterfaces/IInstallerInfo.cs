using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallerApp_CrossPlat
{
    public interface IInstallerInfo
    {
        string Company { get; set; }
        string Project { get; set; }
        int CSID { get; set; }
        string Lot { get; set; }
        string JobNum { get; set; }
        string MasterNum { get; set; }
        string InstallAssignDate { get; set; }
        string InstallAssignPerson { get; set; }
        string ShippedDone { get; set; }
        int InstallerAssignID { get; set; }        
    }
}