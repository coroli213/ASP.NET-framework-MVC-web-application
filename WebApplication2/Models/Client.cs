using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplom_Autentif.Models
{
    [DisplayName("Клиент")]
    public class Client
    {
        [DisplayName("Id")]
        [HiddenInput]
        public int Id { get; set; }

        [DisplayName("Имя")]
        public string First_name { get; set; }

        [DisplayName("Фамилия")]
        public string Second_name { get; set; }

        [DisplayName("Отчество")]
        public string Third_name { get; set; }

        [DisplayName("Серия")]
        public int Serial { get; set; }

        [DisplayName("Номер паспорта")]
        public int Numb { get; set; }

        [DisplayName("Владеет картами")]
        public virtual List<Card> Cards { get; set; }
    }
}