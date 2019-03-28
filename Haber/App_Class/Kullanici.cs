using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haber.App_Class
{
    public class Kullanici
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string KulaniciAdi { get; set; }
        public string Mail { get; set; }
        public string Sifre { get; set; }
        public Guid id { get; set; }
        //public string GizliSoru { get; set; }
        //public string GizliCevap { get; set; }
    }
}