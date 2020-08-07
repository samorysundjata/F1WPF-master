using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class MotorViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Motor MotorTipo = new Motor();
        private ObservableCollection<Motor> _motores;
        private Motor _motorSelecionado;
        private string _nome;
        private Pais _pais;

        #endregion Private Fields

        #region Public Properties

        public ObservableCollection<Motor> Motores
        {
            get { return _motores; }

            set
            {
                _motores = value;
                NotifyOfPropertyChange(() => Motores);
            }
        }

        public Motor MotorSelecionado
        {
            get { return _motorSelecionado; }

            set
            {
                _motorSelecionado = value;
                NotifyOfPropertyChange(() => MotorSelecionado);
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
            if (MotorSelecionado != null)
            {
                var viewModel = new MotorCadViewModel(MotorSelecionado);
                AbreJanela("Editar Motor", viewModel);
                ListaMotores();
            }
            else
            {
                MessageBox.Show("Selecione um motor para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (MotorSelecionado != null)
            {
                var messageBoxResult = MessageBox.Show("Deseja mesmo excluir o motor " + MotorSelecionado.Nome + "?",
                    "Confirme a exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(MotorTipo.GetType(), MotorSelecionado.Nome);
                ListaMotores();
            }
            else
            {
                MessageBox.Show("Selecione um motor para excluir!", "Erro ao excluir", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Motor> ListaMotores()
        {
            try
            {
                Motores = new ObservableCollection<Motor>();

                var nos = IdadosF1.ListaDados(MotorTipo.GetType());

                if (nos != null)
                    foreach (XmlNode node in nos)
                        Motores.Add(new Motor
                        {
                            Nome = node.SelectSingleNode("Nome")?.InnerText,
                            Pais = PaisVm.ListaPaises()
                                .FirstOrDefault(x => x.Sigla == node.SelectSingleNode("Pais")?.InnerText)
                        });

                return Motores = new ObservableCollection<Motor>(Motores.OrderBy(x => x.Nome));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem do registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new MotorCadViewModel(MotorTipo);
            AbreJanela("Novo Motor", viewModel);
            ListaMotores();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaMotores();
        }

        #endregion Protected Methods
    }
}