using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class EventoCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Evento EventoTipo = new Evento();
        private GP _gp;
        private ObservableCollection<GP> _gps;
        private Grid _grid;
        private ObservableCollection<Grid> _grids;
        private int _numero;
        private int _posicao;

        #endregion Private Fields

        #region Public Constructors

        public EventoCadViewModel(Evento eventoSelecionado)
        {
            EventoSelecionado = eventoSelecionado;
        }

        #endregion Public Constructors

        #region Public Properties

        public Evento EventoSelecionado { get; }

        public GP Gp
        {
            get { return _gp; }
            set
            {
                _gp = value;
                NotifyOfPropertyChange(() => Gp);
            }
        }

        public ObservableCollection<GP> Gps
        {
            get { return _gps; }
            set
            {
                _gps = value;
                NotifyOfPropertyChange(() => Gps);
            }
        }

        public Grid Grid
        {
            get { return _grid; }
            set
            {
                _grid = value;
                NotifyOfPropertyChange(() => Grid);
            }
        }

        public ObservableCollection<Grid> Grids
        {
            get { return _grids; }
            set
            {
                _grids = value;
                NotifyOfPropertyChange(() => Grids);
            }
        }

        public int Numero
        {
            get { return _numero; }
            set
            {
                _numero = value;
                NotifyOfPropertyChange(() => Numero);
            }
        }

        public int Posicao
        {
            get { return _posicao; }
            set
            {
                _posicao = value;
                NotifyOfPropertyChange(() => Posicao);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Salvar()
        {
            try
            {
                var dados = new string[4];
                dados[0] = GeraPontuacao();
                dados[1] = Gp.DataCorrida.ToShortDateString();
                dados[2] = Grid.Numero.ToString();
                dados[3] = Posicao.ToString();

                if (EventoSelecionado == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(EventoTipo.GetType(), dados);
                        TryClose();
                    }
                    else
                    {
                        MessageBox.Show("O registro já existe!", "Erro ao salvar", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (!VerificaRepetido(EventoSelecionado.Numero, dados))
                    {
                        IdadosF1.EditarDados(EventoTipo.GetType(), EventoSelecionado.Numero.ToString(), dados);
                        TryClose();
                    }
                    else
                    {
                        MessageBox.Show("O registro já existe!", "Erro ao salvar", MessageBoxButton.OK,
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
            Gps = GPVm.ListaGPs();
            Grids = GridVm.ListaGrids();
        }

        #endregion Protected Methods

        #region Private Methods

        private string GeraPontuacao()
        {
            var maxId = 0;
            var listaPontuacao = EventoVm.ListarEventos();

            if (listaPontuacao.Any())
                maxId = listaPontuacao.Max(x => x.Numero);

            maxId++;

            return maxId.ToString();
        }

        private bool VerificaExistencia()
        {
            var listaEventos = EventoVm.ListarEventos();
            if (!listaEventos.Any()) return false;
            var itemEvento = listaEventos.FirstOrDefault(x =>
                x.GP == Gp && x.Grid.Equals(Grid) && x.Posicao.Equals(Posicao));
            return itemEvento != null;
        }

        private bool VerificaRepetido(int eventoSelecionadonumero, string[] dados)
        {
            var listaEventos = EventoVm.ListarEventos();
            if (!listaEventos.Any()) return false;
            var itemEvento = listaEventos.FirstOrDefault(x => x.Numero != eventoSelecionadonumero);

            return itemEvento != null;
        }

        #endregion Private Methods
    }
}