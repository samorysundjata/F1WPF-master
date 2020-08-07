using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class GridCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Grid GridTipo = new Grid();
        private Equipe _equipe;
        private ObservableCollection<Equipe> _equipes;
        private Motor _motor;
        private ObservableCollection<Motor> _motores;
        private int _numero;
        private int _numeroPiloto;
        private Piloto _piloto;
        private ObservableCollection<Piloto> _pilotos;
        private Temporada _temporada;
        private ObservableCollection<Temporada> _temporadas;

        #endregion Private Fields

        #region Public Constructors

        public GridCadViewModel(Grid gridSelecionado)
        {
            GridSelecionado = gridSelecionado;
            if (gridSelecionado.Numero.Equals(0) &&
                gridSelecionado.Temporada == null &&
                gridSelecionado.NumeroPiloto.Equals(0) &&
                gridSelecionado.Piloto == null &&
                gridSelecionado.Equipe == null &&
                gridSelecionado.Motor == null)
            {
                GridSelecionado = null;
            }
            else
            {
                Numero = gridSelecionado.Numero;
                Temporada = gridSelecionado.Temporada;
                NumeroPiloto = gridSelecionado.NumeroPiloto;
                Piloto = gridSelecionado.Piloto;
                Equipe = gridSelecionado.Equipe;
                Motor = gridSelecionado.Motor;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public bool CanSalvar
        {
            get
            {
                return Temporada != null &&
                       !Piloto.Equals(null) &&
                       !NumeroPiloto.Equals(-1) &&
                       !Equipe.Equals(null) &&
                       !Motor.Equals(null);
            }
        }

        public Equipe Equipe
        {
            get { return _equipe; }
            set
            {
                _equipe = value;
                NotifyOfPropertyChange(() => Equipe);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public ObservableCollection<Equipe> Equipes
        {
            get { return _equipes; }
            set
            {
                _equipes = value;
                NotifyOfPropertyChange(() => Equipes);
            }
        }

        public Grid GridSelecionado { get; }

        public Motor Motor
        {
            get { return _motor; }
            set
            {
                _motor = value;
                NotifyOfPropertyChange(() => Motor);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public ObservableCollection<Motor> Motores
        {
            get { return _motores; }
            set
            {
                _motores = value;
                NotifyOfPropertyChange(() => Motores);
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

        public int NumeroPiloto
        {
            get { return _numeroPiloto; }
            set
            {
                _numeroPiloto = value;
                NotifyOfPropertyChange(() => NumeroPiloto);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public Piloto Piloto
        {
            get { return _piloto; }
            set
            {
                _piloto = value;
                NotifyOfPropertyChange(() => Piloto);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public ObservableCollection<Piloto> Pilotos
        {
            get { return _pilotos; }
            set
            {
                _pilotos = value;
                NotifyOfPropertyChange(() => Pilotos);
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
                var dados = new string[6];
                dados[0] = GeraPontuacao();
                dados[1] = Temporada.Numero.ToString();
                dados[2] = Piloto.Nome.Trim();
                dados[3] = NumeroPiloto.ToString();
                dados[4] = Equipe.Nome.Trim();
                dados[5] = Motor.Nome.Trim();

                if (GridSelecionado == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(GridTipo.GetType(), dados);
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
                    if (!VerificaRepetido(GridSelecionado.Numero, dados))
                    {
                        IdadosF1.EditarDados(GridSelecionado.GetType(), GridSelecionado.Numero.ToString(), dados);
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
            Pilotos = PilotoVm.ListaPilotos();
            Temporadas = TemporadaVm.ListaTemporadas();
            Equipes = EquipeVm.ListaEquipes();
            Motores = MotorVm.ListaMotores();
        }

        #endregion Protected Methods

        #region Private Methods

        private string GeraPontuacao()
        {
            var maxId = 0;
            var listaPontuacao = GridVm.ListaGrids();

            if (listaPontuacao.Any())
                maxId = listaPontuacao.Max(x => x.Numero);

            maxId++;

            return maxId.ToString();

            //return pontuacao;
        }

        private bool VerificaExistencia()
        {
            var listaGrids = GridVm.ListaGrids();
            if (!listaGrids.Any()) return false;
            var itemGrid = listaGrids.FirstOrDefault(x =>
                x.Temporada == Temporada && x.Equipe == Equipe && x.Piloto == Piloto && x.Motor == Motor);
            return itemGrid != null;
        }

        private bool VerificaRepetido(int gridSelecionadoNumero, string[] dados)
        {
            var listaGrids = GridVm.ListaGrids();
            if (!listaGrids.Any()) return false;
            var itemGrid = listaGrids.FirstOrDefault(x => x.Numero != gridSelecionadoNumero &&
                                                          x.Temporada.Numero == Convert.ToInt32(dados[1]) &&
                                                          x.Piloto.Nome == dados[2] &&
                                                          x.NumeroPiloto == Convert.ToInt32(dados[3]) &&
                                                          x.Equipe.Nome == dados[4] &&
                                                          x.Motor.Nome == dados[5]);
            return itemGrid != null;
        }

        #endregion Private Methods
    }
}