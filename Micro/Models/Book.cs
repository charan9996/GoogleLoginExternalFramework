using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Micro.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string BookName { get; set; }
        public double Price { get; set; }
        public string Author { get; set; }
    }
}