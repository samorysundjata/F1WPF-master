using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class CircuitoViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Circuito CircuitoTipo = new Circuito();
        private string _cidade;
        private ObservableCollection<Circuito> _circuitos;
        private Circuito _circuitoSelecionado;
        private string _nome;
        private Pais _pais;
        private TipoCircuito _tipoCircuito;

        #endregion Private Fields

        #region Public Properties

        public string Cidade
        {
            get { return _cidade; }

            set
            {
                _cidade = value;
                NotifyOfPropertyChange(() => Cidade);
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

        public Circuito CircuitoSelecionado
        {
            get { return _circuitoSelecionado; }

            set
            {
                _circuitoSelecionado = value;
                NotifyOfPropertyChange(() => CircuitoSelecionado);
            }
        }

        public string Nome
        {
            get { return _nome; }

            set
            {
                _nome = value;
                NotifyOfPropertyChange(() => Nome);
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

        public TipoCircuito TipoCircuito
        {
            get { return _tipoCircuito; }

            set
            {
                _tipoCircuito = value;
                NotifyOfPropertyChange(() => TipoCircuito);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Editar()
        {
            if (CircuitoSelecionado != null)
            {
                var viewModel = new CircuitoCadViewModel(CircuitoSelecionado);
                AbreJanela("Editar Circuito", viewModel);
                ListaCircuitos();
            }
            else
            {
                MessageBox.Show("Selecione um circuito para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (CircuitoSelecionado != null)
            {
                var messageBoxResult = MessageBox.Show(
                    "Deseja mesmo excluir o circuito " + CircuitoSelecionado.Nome + "?",
                    "Confirme a exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(CircuitoTipo.GetType(), CircuitoSelecionado.Nome);
                ListaCircuitos();
            }
            else
            {
                MessageBox.Show("Selecione um circuito para excluir!", "Erro ao excluir", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Circuito> ListaCircuitos()
        {
            try
            {
                Circuitos = new ObservableCollection<Circuito>();

                var nos = IdadosF1.ListaDados(CircuitoTipo.GetType());

                if (nos != null)
                    foreach (XmlNode node in nos)
                        Circuitos.Add(new Circuito
                        {
                            Nome = node.SelectSingleNode("Nome")?.InnerText,
                            Cidade = node.SelectSingleNode("Cidade")?.InnerText,
                            TipoCircuito = TipoCircuitoVm.ListaTiposCircuitos().FirstOrDefault(x =>
                                x.Nome == node.SelectSingleNode("TipoCircuito")?.InnerText),
                            Pais = PaisVm.ListaPaises()
                                .FirstOrDefault(x => x.Sigla == node.SelectSingleNode("Pais")?.InnerText)
                        });

                return Circuitos = new ObservableCollection<Circuito>(Circuitos.OrderBy(x => x.Nome));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem do registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new CircuitoCadViewModel(CircuitoTipo);
            AbreJanela("Novo Circuito", viewModel);
            ListaCircuitos();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaCircuitos();
        }

        #endregion Protected Methods
    }
}