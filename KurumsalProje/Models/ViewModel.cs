using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KurumsalProje.Models;
namespace KurumsalProje.Models
{
    public class ViewModel
    {
       
            public IEnumerable<Hakkimizda> Hakkimizda { get; set; }
            public IEnumerable<Hizmet> Hizmet { get; set; }
            public IEnumerable<Iletisim> Iletisim { get; set; }
    }
}