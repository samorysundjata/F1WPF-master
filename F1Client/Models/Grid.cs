namespace F1Client.Models
{
    public class Grid
    {
        #region Public Properties

        public int Numero { get; set; }

        public Temporada Temporada { get; set; }

        public Piloto Piloto { get; set; }

        public int NumeroPiloto { get; set; }

        public Equipe Equipe { get; set; }

        public Motor Motor { get; set; }
        
        #endregion Public Properties
    }
}