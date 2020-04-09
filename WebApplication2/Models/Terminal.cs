using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplom_Autentif.Models
{
    [DisplayName("Терминал")]
    public class Terminal
    {
        [DisplayName("Номер терминала")]
        [HiddenInput]
        public int Id { get; set; }

        [DisplayName("Баланс")]
        public int Balance { get; set; }

        [DisplayName("Филлиал-владелец")]
        public string Filial_owner { get; set; }

        [DisplayName("Операции терминала")]
        public virtual List<Operation> Operations { get; set; }
    }
}