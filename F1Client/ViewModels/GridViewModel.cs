using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class GridViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Grid GridTipo = new Grid();
        private Equipe _equipe;
        private ObservableCollection<Grid> _grids;
        private Grid _gridSelecionado;
        private Motor _motor;
        private int _numero;
        private int _numeroPiloto;
        private Piloto _piloto;
        private Temporada _temporada;

        #endregion Private Fields

        #region Public Properties

        public Equipe Equipe
        {
            get { return _equipe; }
            set
            {
                _equipe = value;
                NotifyOfPropertyChange(() => Equipe);
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

        public Grid GridSelecionado
        {
            get { return _gridSelecionado; }
            set
            {
                _gridSelecionado = value;
                NotifyOfPropertyChange(() => GridSelecionado);
            }
        }

        public Motor Motor
        {
            get { return _motor; }
            set
            {
                _motor = value;
                NotifyOfPropertyChange(() => Motor);
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
            }
        }

        public Piloto Piloto
        {
            get { return _piloto; }
            set
            {
                _piloto = value;
                NotifyOfPropertyChange(() => Piloto);
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
            if (GridSelecionado != null)
            {
                var viewModel = new GridCadViewModel(GridSelecionado);
                AbreJanela("Editar Grid", viewModel);
                ListaGrids();
            }
            else
            {
                MessageBox.Show("Selecione um grid para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (GridSelecionado != null)
            {
                var msgBoxResult = MessageBox.Show(
                    "Deseja mesmo excluir o grid da temporada " + GridSelecionado.Temporada.Ano.ToString("yyyy") + "?",
                    "Confirme a exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (msgBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(GridTipo.GetType(), GridSelecionado.Numero.ToString());
                ListaGrids();
            }
            else
            {
                MessageBox.Show("Selecione um circuito para excluir!", "Erro ao excluir", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Grid> ListaGrids()
        {
            try
            {
                Grids = new ObservableCollection<Grid>();

                var nos = IdadosF1.ListaDados(GridTipo.GetType());

                if (nos != null)
                    foreach (XmlNode node in nos)
                        Grids.Add(new Grid
                        {
                            Numero = Convert.ToInt32(node.SelectSingleNode("Numero")?.InnerText),
                            Temporada = TemporadaVm.ListaTemporadas().FirstOrDefault(x =>
                                x.Numero == Convert.ToInt32(node.SelectSingleNode("Temporada")?.InnerText)),
                            NumeroPiloto = Convert.ToInt32(node.SelectSingleNode("NumeroPiloto")?.InnerText),
                            Piloto = PilotoVm.ListaPilotos()
                                .FirstOrDefault(x => x.Nome == node.SelectSingleNode("Piloto")?.InnerText),
                            Equipe = EquipeVm.ListaEquipes()
                                .FirstOrDefault(x => x.Nome == node.SelectSingleNode("Equipe")?.InnerText),
                            Motor = MotorVm.ListaMotores()
                                .FirstOrDefault(x => x.Nome == node.SelectSingleNode("Motor")?.InnerText)
                        });

                return Grids = new ObservableCollection<Grid>(Grids.OrderBy(x => x.Temporada.Ano)
                    .ThenBy(t => t.Equipe.Nome).ThenBy(z => z.NumeroPiloto));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem do registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new GridCadViewModel(GridTipo);
            AbreJanela("Novo Grid", viewModel);
            ListaGrids();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaGrids();
        }

        #endregion Protected Methods
    }
}