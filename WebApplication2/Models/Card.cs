using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplom_Autentif.Models
{
    [DisplayName("Карта")]//задаем имя, которое будет видеть пользователь на страничке
    public class Card
    {
        [DisplayName("Номер карты")]
        [HiddenInput]
        public int Id { get; set; }

        [DisplayName("Серийный номер")]
        public int Number { get; set; }

        [DisplayName("Срок службы")]
        public string Date_of { get; set; }

        [DisplayName("CSV")]
        public int Csv { get; set; }

        [DisplayName("Pin код")]
        public int Pin { get; set; }

        [DisplayName("Связаный счет")]
        public virtual Bill Account { get; set; }

        [DisplayName("Владелец карты")]
        public virtual Client Owner { get; set; }
    }
}