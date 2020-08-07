using System;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace F1Data.DAL
{
    public class GerenciaDados : Dados, IDadosF1
    {
        #region Public Methods

        public void EditarDados(Type entidade, string chave, string[] dados)
        {
            if (VerificaArquivo(entidade))
                try
                {
                    NomeArquivo = CriaNomeArquivo(entidade);
                    var doc = XElement.Load(CriaNomeArquivo(entidade));

                    if (entidade.GetProperties().Length <= 0) return;
                    var elementos = doc.Elements();
                    var elementoSelecionado = elementos.Where(x =>
                        x.Element(((PropertyInfo[]) ((TypeInfo) entidade).DeclaredProperties)[0].Name)?.Value
                            .Trim() ==
                        chave);

                    var items = from item in doc.Elements()
                            .Where(x => x.Element(((PropertyInfo[]) ((TypeInfo) entidade).DeclaredProperties)[0].Name)
                                            ?.Value.Trim() ==
                                        chave)
                        select item;

                    foreach (var idd in items)
                        for (var j = 0; j < entidade.GetProperties().Length; j++)
                            idd.Element(((PropertyInfo[]) ((TypeInfo) entidade).DeclaredProperties)[j].Name)
                                ?.SetValue(dados[j]);

                    doc.Save(NomeArquivo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
        }

        public void ExcluirDados(Type entidade, string chave)
        {
            if (!VerificaArquivo(entidade)) return;
            NomeArquivo = CriaNomeArquivo(entidade);
            var doc = XElement.Load(CriaNomeArquivo(entidade));

            if (entidade.GetProperties().Length > 0)
            {
                var elementos = doc.Elements();
                var elementoSelecionado = elementos.Where(x =>
                    x.Element(((PropertyInfo[]) ((TypeInfo) entidade).DeclaredProperties)[0].Name)?.Value.Trim() ==
                    chave);
                elementoSelecionado.Remove();
            }

            doc.Save(NomeArquivo);
        }

        public XmlNodeList ListaDados(Type entidade)
        {
            if (!VerificaArquivo(entidade)) return null;
            try
            {
                NomeArquivo = CriaNomeArquivo(entidade);
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(NomeArquivo);
                var nodeList = xmlDoc.DocumentElement?.SelectNodes(
                    "/" + util.EscreveNoPlural(entidade) + "/" + util.EscreveNoSingular(entidade));

                return nodeList;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SalvarDados(Type entidade, string[] dados)
        {
            if (VerificaArquivo(entidade))
            {
                var doc = XElement.Load(NomeArquivo);
                var elemento = new XElement(util.EscreveNoSingular(entidade));

                var i = 0;

                if (entidade.GetProperties().Length > 0)
                {
                    foreach (var item in ((TypeInfo)entidade).DeclaredProperties)
                    {
                        elemento.Add(new XElement(item.Name, dados[i]));
                        i++;
                    }
                }
                else
                {
                    foreach (var item in ((TypeInfo) entidade).DeclaredFields)
                    {
                        elemento.Add(new XElement(item.Name, dados[i]));
                        i++;
                    }
                }

                doc.Add(elemento);
                doc.Save(NomeArquivo);
            }
            else
            {
                gaArquivos.CriaArquivo(entidade);
                IDadosF1 idf = new GerenciaDados();
                idf.SalvarDados(entidade, dados);
            }
        }

        #endregion Public Methods
    }
}