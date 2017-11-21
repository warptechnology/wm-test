using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WMTest.Models
{
    public class SecondTaskModel
    {
        //(id int not null primary key, value int not null).
        [Key]
        public int ID { set; get; }
        [Required]
        public  int Value { set; get; }
    }
}