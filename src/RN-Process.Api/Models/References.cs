﻿
using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.DataAccess;

namespace RN_Process.Api.Models
{
    public class References :  Entity<Guid>
    {
        public string UniqCode { get; set; }
    }
}