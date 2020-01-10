using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafikPracy.Models
{
    public class StanowiskoToSend
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public List<StanowiskoMiejscaToSend> Miejsca { get; set; }
        
        public StanowiskoToSend(Stanowisko stanowisko, Boolean czySzczegoly)
        {
            Id = stanowisko.Id;
            Nazwa = stanowisko.Nazwa;

            if (czySzczegoly)
            {
                Miejsca = new List<StanowiskoMiejscaToSend>();

                foreach (StanowiskoMiejsca sm in stanowisko.StanowiskoMiejsca)
                {
                    Miejsca.Add(new StanowiskoMiejscaToSend(sm));
                }
            }
        }
        public StanowiskoToSend(Stanowisko stanowisko)
            : this(stanowisko, true)
        {
        }
    }
}