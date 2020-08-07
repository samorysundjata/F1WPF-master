using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class GPViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly GP GPTipo = new GP();
        private Circuito _circuito;
        private DateTime _dataCorrida;
        private ObservableCollection<GP> _gPs;
        private GP _gPSelecionado;
        private Pais _pais;
        private Temporada _temporada;

        #endregion Private Fields

        #region Public Properties

        public Circuito Circuito
        {
            get { return _circuito; }

            set
            {
                _circuito = value;
                NotifyOfPropertyChange(() => Circuito);
            }
        }

        public DateTime DataCorrida
        {
            get { return _dataCorrida; }

            set
            {
                _dataCorrida = value;
                NotifyOfPropertyChange(() => DataCorrida);
            }
        }

        public ObservableCollection<GP> GPs
        {
            get { return _gPs; }

            set
            {
                _gPs = value;
                NotifyOfPropertyChange(() => GPs);
            }
        }

        public GP GPSelecionado
        {
            get { return _gPSelecionado; }

            set
            {
                _gPSelecionado = value;
                NotifyOfPropertyChange(() => GPSelecionado);
            }
        }

        public Pais Pais
        {
            get { return _pais; }

            set
            {
                _pais = value;
                NotifyOfPropertyChange(() => Pais);
            }
        }

        public Temporada Temporada
        {
            get { return _temporada; }

            set
            {
                _temporada = value;
                NotifyOfPropertyChange(() => Temporada);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Editar()
        {
            if (GPSelecionado != null)
            {
                var viewModel = new GPCadViewModel(GPSelecionado);
                AbreJanela("Editar GP", viewModel);
                ListaGPs();
            }
            else
            {
                MessageBox.Show("Selecione um GP para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (GPSelecionado != null)
            {
                var messageBoxResult = MessageBox.Show("Deseja mesmo excluir o GP de " +
                                                       GPSelecionado.Circuito.Nome +
                                                       " da temporada de " +
                                                       GPSelecionado.Temporada.Ano.Year +
                                                       "?", "Confirme a exclusão", MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (messageBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(GPTipo.GetType(), GPSelecionado.DataCorrida.ToString("dd/MM/yyyy"));
                ListaGPs();
            }
            else
            {
                MessageBox.Show("Selecione um GP para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<GP> ListaGPs()
        {
            try
            {
                GPs = new ObservableCollection<GP>();

                var nos = IdadosF1.ListaDados(GPTipo.GetType());

                if (nos != null)
                    foreach (XmlNode node in nos)
                        GPs.Add(new GP
                        {
                            DataCorrida = Convert.ToDateTime(node.SelectSingleNode("DataCorrida")?.InnerText),
                            Temporada = TemporadaVm.ListaTemporadas()
                                .FirstOrDefault(x =>
                                    x.Numero == Convert.ToInt32(node.SelectSingleNode("Temporada")?.InnerText)),
                            Circuito = CircuitoVm.ListaCircuitos()
                                .FirstOrDefault(x => x.Nome == node.SelectSingleNode("Circuito")?.InnerText),
                            Pais = PaisVm.ListaPaises()
                                .FirstOrDefault(x => x.Sigla == node.SelectSingleNode("Pais")?.InnerText)
                        });

                return GPs = new ObservableCollection<GP>(GPs.OrderBy(x => x.DataCorrida));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem do registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new GPCadViewModel(GPTipo);
            AbreJanela("Nova GP", viewModel);
            ListaGPs();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaGPs();
        }

        #endregion Protected Methods
    }
}