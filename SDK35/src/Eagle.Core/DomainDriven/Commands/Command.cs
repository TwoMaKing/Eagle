﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EApp.Core.DomainDriven.Commands
{
    public class Command : ICommand
    {
        public int Id
        {
            get;
            set;
        }


    }
}
