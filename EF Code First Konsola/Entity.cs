using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EF_Code_First_Konsola
{
    //id imieNazwisko takObiad takNocleg takOplata dataPrzyjazdu dataOdjazdu
    class Uczestnik
    {
        public int ID { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public bool takObiad { get; set; }
        public bool takNocleg { get; set; }
        public bool takOplata { get; set; }
        public DateTime dataPrzyjazdu { get; set; }
        public DateTime dataOdjazdu { get; set; }
    }
    class UczestnicyKonferencji : DbContext
    {
        public UczestnicyKonferencji(string ConnectionString) : base(ConnectionString)
        {

        }
        public DbSet<Uczestnik> Uczestnicy { get; set; }
    }
    class test
    {
        public string imie { get; set; }
        public string nazwisko { get; set; }

    }

    class Entity
    {
    }
    
}
