using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WMTest.Models
{
    /// <summary>
    /// Есть таблица 
    /// (id int not null identity primary key,
    /// name varchar (50) unique).
    /// </summary>
    public class FirstTaskModel
    {
        [Key]        
        public int ID { set; get; }
        [Required]
        [MaxLength(50)]
        [Index(IsUnique = true)]
        public string Name { set; get; }
    }
}