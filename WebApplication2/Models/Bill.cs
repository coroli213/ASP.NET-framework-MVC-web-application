using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplom_Autentif.Models
{
    [DisplayName("Счет")]
    public class Bill
    {
        [DisplayName("Номер счета")]
        [HiddenInput]
        public int Id { get; set; }

        [DisplayName("Баланс")]
        public int Balance { get; set; }

        [DisplayName("Банк-владелец")]
        public string Bank_owner { get; set; }

        [DisplayName("Карта связаная")]
        public virtual List<Card> Cards { get; set; }

        [DisplayName("Операции")]
        public virtual List<Operation> Operations { get; set; }

    }
}