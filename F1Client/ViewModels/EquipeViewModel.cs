using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class EquipeViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Equipe EquipeTipo = new Equipe();
        private DateTime? _despedida;
        private ObservableCollection<Equipe> _equipes;
        private Equipe _equipeSelecionada;
        private DateTime _estreia;
        private string _nome;
        private Pais _pais;

        #endregion Private Fields

        #region Public Properties

        public DateTime? Despedida
        {
            get { return _despedida; }

            set
            {
                _despedida = value;
                NotifyOfPropertyChange(() => Despedida);
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

        public Equipe EquipeSelecionada
        {
            get { return _equipeSelecionada; }

            set
            {
                _equipeSelecionada = value;
                NotifyOfPropertyChange(() => EquipeSelecionada);
            }
        }

        public DateTime Estreia
        {
            get { return _estreia; }
            set
            {
                _estreia = value;
                NotifyOfPropertyChange(() => Estreia);
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

        #endregion Public Properties

        #region Public Methods

        public void Editar()
        {
            if (EquipeSelecionada != null)
            {
                var viewModel = new EquipeCadViewModel(EquipeSelecionada);
                AbreJanela("Editar Equipe", viewModel);
                ListaEquipes();
            }
            else
            {
                MessageBox.Show("Selecione uma equipe para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (EquipeSelecionada != null)
            {
                var messageBoxResult = MessageBox.Show(
                    "Deseja mesmo excluir a equipe " + EquipeSelecionada.Nome + "?",
                    "Confirme a exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(EquipeTipo.GetType(), EquipeSelecionada.Nome);
                ListaEquipes();
            }
            else
            {
                MessageBox.Show("Selecione uma equipe para excluir!", "Erro ao excluir", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Equipe> ListaEquipes()
        {
            try
            {
                Equipes = new ObservableCollection<Equipe>();

                var nos = IdadosF1.ListaDados(EquipeTipo.GetType());

                DateTime? semData = null;

                if (nos != null)
                    foreach (XmlNode node in nos)
                        Equipes.Add(new Equipe
                        {
                            Nome = node.SelectSingleNode("Nome")?.InnerText,
                            Estreia = DateTime.ParseExact(node.SelectSingleNode("Estreia")?.InnerText,
                                "dd/MM/yyyy", null),
                            Despedida = DateTime.ParseExact(node.SelectSingleNode("Despedida")?.InnerText, "dd/MM/yyyy",
                                null).Equals(DateTime.MinValue)
                                ? semData
                                : DateTime.ParseExact(node.SelectSingleNode("Despedida")?.InnerText, "dd/MM/yyyy",
                                    null),
                            Pais = PaisVm.ListaPaises()
                                .FirstOrDefault(x => x.Sigla == node.SelectSingleNode("Pais")?.InnerText)
                        });

                return Equipes = new ObservableCollection<Equipe>(Equipes.OrderBy(x => x.Nome));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem do registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new EquipeCadViewModel(EquipeTipo);
            AbreJanela("Nova Equipe", viewModel);
            ListaEquipes();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaEquipes();
        }

        #endregion Protected Methods
    }
}