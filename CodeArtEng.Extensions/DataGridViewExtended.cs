﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeArtEng.Extensions
{
    public class DataGridViewExtended : DataGridView
    {
        public DataGridViewExtended() : base()
        {
            this.EnableAdvanceControl();
        }
    }
}
