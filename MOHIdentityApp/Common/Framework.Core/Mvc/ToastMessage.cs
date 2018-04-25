using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Mvc
{
   

    public class ToastMessage

    {

        public string Title { get; set; }

        public string Message { get; set; }

        public ToastType ToastType { get; set; }

        public bool IsSticky { get; set; }

    }
}
