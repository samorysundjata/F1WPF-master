using System;

namespace F1Client.Models
{
    public class GP
    {
        #region Public Properties

        public DateTime DataCorrida { get; set; }

        public Temporada Temporada { get; set; }
        
        public Circuito Circuito { get; set; }

        public Pais Pais { get; set; }

        #endregion Public Properties
    }
}