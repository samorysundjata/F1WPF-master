using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class TipoPneuViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly TipoPneu TipoPneuTipo = new TipoPneu();
        private string _nome;
        private TipoPneu _tipopneuSelecionado;
        private ObservableCollection<TipoPneu> _tiposPneus;

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

        public TipoPneu TipoPneuSelecionado
        {
            get { return _tipopneuSelecionado; }

            set
            {
                _tipopneuSelecionado = value;
                NotifyOfPropertyChange(() => TipoPneuSelecionado);
            }
        }

        public ObservableCollection<TipoPneu> TiposPneus
        {
            get { return _tiposPneus; }

            set
            {
                _tiposPneus = value;
                NotifyOfPropertyChange(() => TiposPneus);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Editar()
        {
            if (TipoPneuSelecionado != null)
            {
                var viewModel = new TipoPneuCadViewModel(TipoPneuSelecionado);
                AbreJanela("Editar Tipo de Pneu", viewModel);
                ListaTiposPneus();
            }
            else
            {
                MessageBox.Show("Selecione um tipo de pneu para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (TipoPneuSelecionado != null)
            {
                var messageBoxResult = MessageBox.Show(
                    "Deseja mesmo excluir o tipod de pneu " + TipoPneuSelecionado.Nome + "?",
                    "Confirme a exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(TipoPneuTipo.GetType(), TipoPneuSelecionado.Nome);
                ListaTiposPneus();
            }
            else
            {
                MessageBox.Show("Selecione um tipo de pneu para excluir!", "Erro ao excluir", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<TipoPneu> ListaTiposPneus()
        {
            try
            {
                TiposPneus = new ObservableCollection<TipoPneu>();

                var nos = IdadosF1.ListaDados(TipoPneuTipo.GetType());

                if (nos != null)
                    foreach (XmlNode node in nos)
                        TiposPneus.Add(new TipoPneu
                        {
                            Nome = node.SelectSingleNode("Nome")?.InnerText
                        });

                return TiposPneus = new ObservableCollection<TipoPneu>(TiposPneus.OrderBy(x => x.Nome));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem do registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new TipoPneuCadViewModel(TipoPneuTipo);
            AbreJanela("Novo Tipo de Pneu", viewModel);
            ListaTiposPneus();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaTiposPneus();
        }

        #endregion Protected Methods
    }
}