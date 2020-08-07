using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class PneuViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Pneu PneuTipo = new Pneu();
        private string _nome;
        private Pais _pais;
        private ObservableCollection<Pneu> _pneus;
        private Pneu _pneuSelecionado;

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

        public Pais Pais
        {
            get { return _pais; }

            set
            {
                _pais = value;
                NotifyOfPropertyChange(() => Pais);
            }
        }

        public ObservableCollection<Pneu> Pneus
        {
            get { return _pneus; }

            set
            {
                _pneus = value;
                NotifyOfPropertyChange(() => Pneus);
            }
        }

        public Pneu PneuSelecionado
        {
            get { return _pneuSelecionado; }

            set
            {
                _pneuSelecionado = value;
                NotifyOfPropertyChange(() => PneuSelecionado);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Editar()
        {
            if (PneuSelecionado != null)
            {
                var viewModel = new PneuCadViewModel(PneuSelecionado);
                AbreJanela("Editar Pneu", viewModel);
                ListaPneus();
            }
            else
            {
                MessageBox.Show("Selecione um pneu para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (PneuSelecionado != null)
            {
                var messageBoxResult = MessageBox.Show("Deseja mesmo excluir o pneu " + PneuSelecionado.Nome + "?",
                    "Confirme a exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(PneuTipo.GetType(), PneuSelecionado.Nome);
                ListaPneus();
            }
            else
            {
                MessageBox.Show("Selecione um pneu para excluir!", "Erro ao excluir", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Pneu> ListaPneus()
        {
            try
            {
                Pneus = new ObservableCollection<Pneu>();

                var nos = IdadosF1.ListaDados(PneuTipo.GetType());

                if (nos != null)
                    foreach (XmlNode node in nos)
                        Pneus.Add(new Pneu
                        {
                            Nome = node.SelectSingleNode("Nome")?.InnerText,
                            Pais = PaisVm.ListaPaises()
                                .FirstOrDefault(x => x.Sigla == node.SelectSingleNode("Pais")?.InnerText)
                        });

                return Pneus = new ObservableCollection<Pneu>(Pneus.OrderBy(x => x.Nome));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem do registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new PneuCadViewModel(PneuTipo);
            AbreJanela("Novo Pneu", viewModel);
            ListaPneus();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaPneus();
        }

        #endregion Protected Methods
    }
}