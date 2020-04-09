using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplom_Autentif.Models
{
    [DisplayName("Операции")]
    public class Operation
    {
        [DisplayName("Номер операции")]
        [HiddenInput]
        public int Id { get; set; }

        [DisplayName("Тип операции")]
        public string Type { get; set; }

        [DisplayName("Сумма")]
        public int Amount { get; set; }

        [DisplayName("Номер терминала")]
        public virtual Terminal Terminal { get; set; }

        [DisplayName("Счет приемник")]
        public virtual Bill Account_to { get; set; }

        [DisplayName("Счет источник")]
        public virtual Bill Account_from { get; set; }
    }
}