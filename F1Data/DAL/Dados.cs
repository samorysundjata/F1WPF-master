using F1Data.Util;
using System;
using System.Configuration;
using System.Xml;

namespace F1Data.DAL
{
    public class Dados
    {
        #region Protected Fields

        protected static readonly GerenciaArquivos gaArquivos = new GerenciaArquivos();

        protected static readonly Utils util = new Utils();

        #endregion Protected Fields

        #region Private Fields

        private static readonly string DiretorioData = ConfigurationManager.AppSettings["data:local"];

        #endregion Private Fields

        #region Protected Properties

        protected static string NomeArquivo { get; set; }

        #endregion Protected Properties

        #region Protected Methods

        protected string CriaNomeArquivo(Type entidade)
        {
            return NomeArquivo = DiretorioData + entidade.Name + ".xml";
        }

        protected bool VerificaArquivo(Type entidade)
        {
            NomeArquivo = CriaNomeArquivo(entidade);
            var xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(NomeArquivo);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        #endregion Protected Methods
    }
}