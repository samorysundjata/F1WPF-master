using System;
using System.Text;
using System.Windows;
using System.Xml;

namespace F1Data.DAL
{
    public class GerenciaArquivos : Dados
    {
        #region Public Methods

        public void CriaArquivo(Type entidade)
        {
            NomeArquivo = CriaNomeArquivo(entidade);

            if (NomeArquivo.Length > 0)
                try
                {
                    var writer = new XmlTextWriter(NomeArquivo, Encoding.UTF8);

                    writer.WriteStartDocument(true);
                    writer.Formatting = Formatting.Indented;
                    writer.Indentation = 2;
                    writer.WriteStartElement(util.EscreveNoPlural(entidade));

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocorreu o erro: " + ex.Message + "!");
                    throw;
                }
            else
                throw new Exception();
        }

        #endregion Public Methods
    }
}