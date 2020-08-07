namespace F1Client.Models
{
    public class Circuito
    {
        #region Public Properties

        public string Nome { get; set; }

        public string Cidade { get; set; }

        public Pais Pais { get; set; }

        public TipoCircuito TipoCircuito { get; set; }

        #endregion Public Properties
    }
}