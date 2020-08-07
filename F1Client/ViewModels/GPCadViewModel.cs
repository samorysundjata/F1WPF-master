using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class GPCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly GP GPTipo = new GP();
        private Circuito _circuito;
        private ObservableCollection<Circuito> _circuitos;
        private DateTime _dataCorrida;
        private Pais _pais;
        private ObservableCollection<Pais> _paises;
        private Temporada _temporada;
        private ObservableCollection<Temporada> _temporadas;

        #endregion Private Fields

        #region Public Constructors

        public GPCadViewModel(GP gPSelecionado)
        {
            GPSelecionado = gPSelecionado;
            if (gPSelecionado.DataCorrida.Equals(DateTime.MinValue) &&
                gPSelecionado.Temporada == null &&
                gPSelecionado.Circuito == null &&
                gPSelecionado.Pais == null)
            {
                GPSelecionado = null;
                DataCorrida = UltimaData();
            }
            else
            {
                DataCorrida = gPSelecionado.DataCorrida;
                Temporada = gPSelecionado.Temporada;
                Circuito = gPSelecionado.Circuito;
                Pais = gPSelecionado.Pais;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public bool CanSalvar
        {
            get
            {
                return Temporada != null &&
                       Circuito != null &&
                       Pais != null &&
                       DataCorrida.Date.Year >= Temporada.Ano.Year &&
                       DataCorrida != DateTime.MinValue;
            }
        }

        public Circuito Circuito
        {
            get { return _circuito; }

            set
            {
                _circuito = value;
                NotifyOfPropertyChange(() => Circuito);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public ObservableCollection<Circuito> Circuitos
        {
            get { return _circuitos; }

            set
            {
                _circuitos = value;
                NotifyOfPropertyChange(() => Circuitos);
            }
        }

        public DateTime DataCorrida
        {
            get { return _dataCorrida; }

            set
            {
                _dataCorrida = value;
                NotifyOfPropertyChange(() => DataCorrida);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public GP GPSelecionado { get; }

        public Pais Pais
        {
            get { return _pais; }

            set
            {
                _pais = value;
                NotifyOfPropertyChange(() => Pais);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public ObservableCollection<Pais> Paises
        {
            get { return _paises; }

            set
            {
                _paises = value;
                NotifyOfPropertyChange(() => Paises);
            }
        }

        public Temporada Temporada
        {
            get { return _temporada; }

            set
            {
                _temporada = value;
                NotifyOfPropertyChange(() => Temporada);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public ObservableCollection<Temporada> Temporadas
        {
            get { return _temporadas; }

            set
            {
                _temporadas = value;
                NotifyOfPropertyChange(() => Temporadas);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Salvar()
        {
            try
            {
                var dados = new string[4];
                dados[0] = DataCorrida.ToShortDateString();
                dados[1] = Temporada.Numero.ToString();
                dados[2] = Circuito.Nome.Trim();
                dados[3] = Pais.Sigla;

                if (GPSelecionado == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(GPTipo.GetType(), dados);
                        TryClose();
                    }
                    else
                    {
                        MessageBox.Show("O registro já existe", "Erro ao salvar", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (!VerificaRepetido(DataCorrida, dados))
                    {
                        IdadosF1.EditarDados(GPTipo.GetType(), GPSelecionado.DataCorrida.ToShortDateString(), dados);
                        TryClose();
                    }
                    else
                    {
                        MessageBox.Show("O registro já existe", "Erro ao salvar", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na inclusão do registro!", "Erro na inclusão");
                TryClose();
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            Circuitos = CircuitoVm.ListaCircuitos();
            Paises = PaisVm.ListaPaises();
            Temporadas = TemporadaVm.ListaTemporadas();
        }

        #endregion Protected Methods

        #region Private Methods

        private DateTime UltimaData()
        {
            var maxData = DataInicio;
            var gpsVm = new GPViewModel();
            var listaGPs = gpsVm.ListaGPs();
            if (listaGPs.Any())
                maxData = listaGPs.Max(x => x.DataCorrida).AddDays(7);

            return maxData;
        }

        private bool VerificaExistencia()
        {
            var gpVm = new GPViewModel();
            var listaGp = gpVm.ListaGPs();
            if (!listaGp.Any()) return false;
            var itemGp = listaGp.FirstOrDefault(x =>
                x.DataCorrida == DataCorrida ||
                x.Circuito.Nome == Circuito.Nome &&
                x.Temporada.Numero == Temporada.Numero);
            return itemGp != null;
        }

        private bool VerificaRepetido(DateTime dataCorrida, string[] dados)
        {
            var gpVm = new GPViewModel();
            var listaGp = gpVm.ListaGPs();
            if (!listaGp.Any()) return false;
            var itemGp = listaGp.FirstOrDefault(x => x.DataCorrida != dataCorrida &&
                                                     x.Circuito.Nome == dados[2] &&
                                                     x.Temporada.Numero == Convert.ToInt32(dados[1]));
            return itemGp != null;
        }

        #endregion Private Methods
    }
}