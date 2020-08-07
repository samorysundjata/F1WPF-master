using System;

namespace F1Client.Models
{
    public class Equipe
    {
        #region Public Properties

        public string Nome { get; set; }

        public DateTime Estreia { get; set; }

        public DateTime? Despedida { get; set; }

        public Pais Pais { get; set; }

        #endregion Public Properties
    }
}