﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBL.Exceptions
{
    public class SpelerException : Exception
    {
        public SpelerException(string? message) : base(message)
        {
        }

        public SpelerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
