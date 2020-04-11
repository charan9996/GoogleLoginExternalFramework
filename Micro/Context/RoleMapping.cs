using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Micro.Context
{
    public class RoleMapping
    {
        [Key]
        public int Id { get; set; }

      
        public int RoleId { get; set; }
   
    
        public int UserId { get; set; }

        [ForeignKey("RoleId")]
        public  Roles Roles { get; set; }
        [ForeignKey("UserId")]
        public  Users Users { get; set; }
    }
}