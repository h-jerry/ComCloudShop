﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace ComCloudShop.WeixinOauth.Pay.Others
{

    [Serializable]
    public class SDKRuntimeException : Exception
    {

        private const long serialVersionUID = 1L;

        public SDKRuntimeException(String str)
            : base(str)
        {

        }
    }
}
