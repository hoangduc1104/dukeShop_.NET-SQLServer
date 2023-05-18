﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19T1021035.DomainModels
{
    public class UserAccount
    {
        public string UserId { get; set; }

        /// <summary>
        ///     User Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Full Name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Group Names
        /// </summary>
        public string GroupNames { get; set; }

        /// <summary>
        ///     Photo
        /// </summary>
        public string Photo { get; set; }
    }
}
