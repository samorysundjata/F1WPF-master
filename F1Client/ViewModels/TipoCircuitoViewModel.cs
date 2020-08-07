using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class TipoCircuitoViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly TipoCircuito TipoCircuitoTipo = new TipoCircuito();
        private string _nome;
        private TipoCircuito _tipoCircuitoSelecionado;
        private ObservableCollection<TipoCircuito> _tiposCircuitos;

        #endregion Private Fields

        #region Public Properties

        public string Nome
        {
            get { return _nome; }

            set
            {
                _nome = value;
                NotifyOfPropertyChange(() => Nome);
            }
        }

        public TipoCircuito TipoCircuitoSelecionado
        {
            get { return _tipoCircuitoSelecionado; }

            set
            {
                _tipoCircuitoSelecionado = value;
                NotifyOfPropertyChange(() => TipoCircuitoSelecionado);
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

        public void Editar()
        {
            if (TipoCircuitoSelecionado != null)
            {
                var viewModel = new TipoCircuitoCadViewModel(TipoCircuitoSelecionado);
                AbreJanela("Editar Tipo de Circuito", viewModel);
                ListaTiposCircuitos();
            }
            else
            {
                MessageBox.Show("Selecione um tipo de circuito para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (TipoCircuitoSelecionado != null)
            {
                var messageBoxResult = MessageBox.Show(
                    "Deseja mesmo excluir o tipo de circuito " + TipoCircuitoSelecionado.Nome + "?",
                    "Confirme a exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(TipoCircuitoTipo.GetType(), TipoCircuitoSelecionado.Nome);
                ListaTiposCircuitos();
            }
            else
            {
                MessageBox.Show("Selecione um tipo de circuito para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<TipoCircuito> ListaTiposCircuitos()
        {
            try
            {
                TiposCircuitos = new ObservableCollection<TipoCircuito>();

                var nos = IdadosF1.ListaDados(TipoCircuitoTipo.GetType());

                if (nos != null)
                    foreach (XmlNode node in nos)
                        TiposCircuitos.Add(new TipoCircuito
                        {
                            Nome = node.SelectSingleNode("Nome")?.InnerText
                        });

                return TiposCircuitos = new ObservableCollection<TipoCircuito>(TiposCircuitos.OrderBy(x => x.Nome));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem do registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new TipoCircuitoCadViewModel(TipoCircuitoTipo);
            AbreJanela("Novo Tipo de Circuito", viewModel);
            ListaTiposCircuitos();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaTiposCircuitos();
        }

        #endregion Protected Methods
    }
}