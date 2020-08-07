namespace F1Client.Models
{
    public class Evento
    {
        #region Public Properties

        public int Numero { get; set; }

        public GP GP { get; set; }

        //public int NumeroPiloto { get; set; }
        public Grid Grid { get; set; }

        public int Posicao { get; set; }

        #endregion Public Properties
    }
}