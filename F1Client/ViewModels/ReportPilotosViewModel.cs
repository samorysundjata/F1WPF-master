using F1Client.Models;
using F1Client.Reports;
using System.Data;
using System.Linq;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class ReportPilotosViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Piloto PilotoTipo = new Piloto();

        #endregion Private Fields

        #region Public Methods

        public rptPilotos CarregarRelatorio()
        {
            var dataSet = new DsPilotos();

            var nos = IdadosF1.ListaDados(PilotoTipo.GetType());
            var listaP = PaisVm.ListaPaises();

            if (nos != null)
                foreach (XmlNode node in nos)
                {
                    var nome = listaP?.FirstOrDefault(x => x.Sigla == node.SelectSingleNode("Pais")?.InnerText)?.Nome;
                    if (nome != null)
                        dataSet.DtPilotos.AddDtPilotosRow(node.SelectSingleNode("Nome")?.InnerText,
                            nome);
                }

            var dtPilotosTable = dataSet.Tables["DtPilotos"];
            var pilotosView = dtPilotosTable.AsDataView();

            pilotosView.Sort = "Pais asc, Nome asc";

            var reportDoc = new rptPilotos();
            reportDoc.SetDataSource(pilotosView);
            return reportDoc;
        }

        #endregion Public Methods
    }
}