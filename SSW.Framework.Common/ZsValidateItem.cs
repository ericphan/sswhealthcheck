using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SSW.Framework.Common
{
    /// <summary>
    /// model for an item to be displayed on zsValidate view
    /// </summary>

    public class ZsValidateItem
    {
        public ZsValidateState State { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public enum ZsValidateState
    {
        Ok,
        Fail,
        Unknown
    }
}