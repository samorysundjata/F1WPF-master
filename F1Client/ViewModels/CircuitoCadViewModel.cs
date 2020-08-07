using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class CircuitoCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Circuito CircuitoTipo = new Circuito();
        private string _cidade;
        private string _nome;
        private Pais _pais;
        private ObservableCollection<Pais> _paises;
        private TipoCircuito _tipoCircuito;
        private ObservableCollection<TipoCircuito> _tiposCircuitos;

        #endregion Private Fields

        #region Public Constructors

        public CircuitoCadViewModel(Circuito circuitoSelecionado)
        {
            CircuitoSelecionado = circuitoSelecionado;
            if (circuitoSelecionado.Nome == null &&
                circuitoSelecionado.Cidade == null &&
                circuitoSelecionado.Pais == null &&
                circuitoSelecionado.TipoCircuito == null)
            {
                CircuitoSelecionado = null;
            }
            else
            {
                Nome = circuitoSelecionado.Nome;
                Cidade = circuitoSelecionado.Cidade;
                Pais = circuitoSelecionado.Pais;
                TipoCircuito = circuitoSelecionado.TipoCircuito;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public bool CanSalvar
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Nome) &&
                       !string.IsNullOrWhiteSpace(Cidade) &&
                       !string.IsNullOrWhiteSpace(Pais.Sigla) &&
                       !string.IsNullOrWhiteSpace(TipoCircuito.Nome);
            }
        }

        public string Cidade
        {
            get { return _cidade; }

            set
            {
                _cidade = value;
                NotifyOfPropertyChange(() => Cidade);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public Circuito CircuitoSelecionado { get; }

        public string Nome
        {
            get { return _nome; }

            set
            {
                _nome = value;
                NotifyOfPropertyChange(() => Nome);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

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

        public TipoCircuito TipoCircuito
        {
            get { return _tipoCircuito; }

            set
            {
                _tipoCircuito = value;
                NotifyOfPropertyChange(() => TipoCircuito);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public ObservableCollection<TipoCircuito> TiposCircuitos
        {
            get { return _tiposCircuitos; }

            set
            {
                _tiposCircuitos = value;
                NotifyOfPropertyChange(() => TiposCircuitos);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Salvar()
        {
            try
            {
                var dados = new string[4];
                dados[0] = Nome.Trim();
                dados[1] = Cidade.Trim();
                dados[2] = Pais.Sigla.ToUpper();
                dados[3] = TipoCircuito.Nome;

                if (CircuitoSelecionado == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(CircuitoTipo.GetType(), dados);
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
                    if (!VerificaRepetido(CircuitoSelecionado.Nome, dados))
                    {
                        IdadosF1.EditarDados(CircuitoTipo.GetType(), CircuitoSelecionado.Nome, dados);
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
            Paises = PaisVm.ListaPaises();
            TiposCircuitos = TipoCircuitoVm.ListaTiposCircuitos();
        }

        #endregion Protected Methods

        #region Private Methods

        private bool VerificaExistencia()
        {
            var listaCircuito = CircuitoVm.ListaCircuitos();
            if (!listaCircuito.Any()) return false;
            var itemCircuito =
                listaCircuito.FirstOrDefault(x => x.Nome == Nome || x.Cidade == Cidade);
            return itemCircuito != null;
        }

        private bool VerificaRepetido(string circuitoSelecionadoNome, string[] dados)
        {
            var listaCircuito = CircuitoVm.ListaCircuitos();
            if (!listaCircuito.Any()) return false;
            var itemCircuito = listaCircuito.FirstOrDefault(x =>
                x.Nome == dados[0] && Nome != circuitoSelecionadoNome ||
                x.Cidade == Cidade && x.Nome != circuitoSelecionadoNome);
            return itemCircuito != null;
        }

        #endregion Private Methods
    }
}