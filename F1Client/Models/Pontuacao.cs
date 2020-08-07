namespace F1Client.Models
{
    public class Pontuacao
    {
        #region Public Properties

        public int Numero { get; set; }

        public int Pontos { get; set; }

        public int Posicao { get; set; }

        public Temporada Temporada { get; set; }

        #endregion Public Properties
    }
}