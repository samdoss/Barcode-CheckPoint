﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPoint.Model.Reports.Interfaces
{
    interface IReportExporter
    {
        void Export(string fileName);
    }
}
