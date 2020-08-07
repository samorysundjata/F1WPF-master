using System;

namespace F1Client.Models
{
    public class Piloto
    {
        #region Public Properties

        public string Nome { get; set; }

        public DateTime DataNascimento { get; set; }

        public DateTime? DataFalecimento { get; set; }

        public Pais Pais { get; set; }

        #endregion Public Properties
    }
}