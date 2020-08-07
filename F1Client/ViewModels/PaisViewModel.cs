using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class PaisViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Pais PaisTipo = new Pais();
        private string _nome;
        private ObservableCollection<Pais> _paises;
        private Pais _paisSelecionado;
        private string _sigla;

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

        public ObservableCollection<Pais> Paises
        {
            get { return _paises; }

            set
            {
                _paises = value;
                NotifyOfPropertyChange(() => Paises);
            }
        }

        public Pais PaisSelecionado
        {
            get { return _paisSelecionado; }

            set
            {
                _paisSelecionado = value;
                NotifyOfPropertyChange(() => PaisSelecionado);
            }
        }

        public string Sigla
        {
            get { return _sigla; }

            set
            {
                _sigla = value;
                NotifyOfPropertyChange(() => Sigla);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Editar()
        {
            if (PaisSelecionado != null)
            {
                var viewModel = new PaisCadViewModel(PaisSelecionado);
                AbreJanela("Editar País", viewModel);
                ListaPaises();
            }
            else
            {
                MessageBox.Show("Selecione um país para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (PaisSelecionado != null)
            {
                var messageBoxResult = MessageBox.Show("Deseja mesmo excluir o país " + PaisSelecionado.Nome + "?",
                    "Confirme a exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(PaisTipo.GetType(), PaisSelecionado.Sigla);
                ListaPaises();
            }
            else
            {
                MessageBox.Show("Selecione um país para excluir!", "Erro ao excluir", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Pais> ListaPaises()
        {
            try
            {
                Paises = new ObservableCollection<Pais>();

                var nos = IdadosF1.ListaDados(PaisTipo.GetType());

                if (nos != null)
                    foreach (XmlNode node in nos)
                        Paises.Add(new Pais
                        {
                            Sigla = node.SelectSingleNode("Sigla")?.InnerText.ToUpper(),
                            Nome = node.SelectSingleNode("Nome")?.InnerText
                        });

                return Paises = new ObservableCollection<Pais>(Paises.OrderBy(x => x.Nome));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem dos registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new PaisCadViewModel(PaisTipo);
            AbreJanela("Novo País", viewModel);
            ListaPaises();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaPaises();
        }

        #endregion Protected Methods
    }
}