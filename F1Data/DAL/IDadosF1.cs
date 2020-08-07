using System;
using System.Xml;

namespace F1Data.DAL
{
    public interface IDadosF1
    {
        #region Public Methods

        void EditarDados(Type entidade, string chave, string[] dados);

        void ExcluirDados(Type entidade, string chave);

        XmlNodeList ListaDados(Type entidade);

        void SalvarDados(Type entidade, string[] dados);

        #endregion Public Methods
    }
}