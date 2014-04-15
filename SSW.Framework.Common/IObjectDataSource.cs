﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.Framework.Common
{
    public interface IObjectDataSource<T>
    {
        IList<T> GetObjects();
    }

}
