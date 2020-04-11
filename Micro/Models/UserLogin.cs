using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Micro.Models
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Password of the user 
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}