using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMTest.Models
{
    public class ThirdTaskModel
    {
        //(id int primary key, balance money).
        [Key]
        public int ID { set; get; }
        [DataType(DataType.Currency)]

        [Range(0.0, Double.PositiveInfinity)]
        [Column(TypeName = "Money")]
        public decimal Balance { set; get; }
    }
}