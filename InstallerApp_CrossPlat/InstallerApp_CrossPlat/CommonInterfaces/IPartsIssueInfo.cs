﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallerApp_CrossPlat
{
    public interface IPartsIssueInfo
    {
        string PartDescription { get; set; }
        bool IsCbSelected { get; set; }
    }
}