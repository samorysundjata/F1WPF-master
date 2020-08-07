using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class PilotoViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Piloto PilotoTipo = new Piloto();
        private DateTime _dataFalecimento;
        private DateTime _dataNascimento;
        private string _nome;
        private Pais _pais;
        private ObservableCollection<Piloto> _pilotos;
        private Piloto _pilotoSelecionado;

        #endregion Private Fields

        #region Public Properties

        public DateTime DataFalecimento
        {
            get { return _dataFalecimento; }

            set
            {
                _dataFalecimento = value;
                NotifyOfPropertyChange(() => DataFalecimento);
            }
        }

        public DateTime DataNascimento
        {
            get { return _dataNascimento; }

            set
            {
                _dataNascimento = value;
                NotifyOfPropertyChange(() => DataNascimento);
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

        public ObservableCollection<Piloto> Pilotos
        {
            get { return _pilotos; }

            set
            {
                _pilotos = value;
                NotifyOfPropertyChange(() => Pilotos);
            }
        }

        public Piloto PilotoSelecionado
        {
            get { return _pilotoSelecionado; }

            set
            {
                _pilotoSelecionado = value;
                NotifyOfPropertyChange(() => PilotoSelecionado);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Editar()
        {
            if (PilotoSelecionado != null)
            {
                var viewModel = new PilotoCadViewModel(PilotoSelecionado);
                AbreJanela("Editar Piloto", viewModel);
                ListaPilotos();
            }
            else
            {
                MessageBox.Show("Selecione um piloto para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (PilotoSelecionado != null)
            {
                var messageBoxResult = MessageBox.Show(
                    "Deseja mesmo excluir o piloto " + PilotoSelecionado.Nome + "?",
                    "Confirme a exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(PilotoTipo.GetType(), PilotoSelecionado.Nome);
                ListaPilotos();
            }
            else
            {
                MessageBox.Show("Selecione um piloto para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Piloto> ListaPilotos()
        {
            try
            {
                Pilotos = new ObservableCollection<Piloto>();

                var nos = IdadosF1.ListaDados(PilotoTipo.GetType());

                DateTime? semData = null;

                if (nos != null)
                    foreach (XmlNode node in nos)
                        Pilotos.Add(new Piloto
                        {
                            Nome = node.SelectSingleNode("Nome")?.InnerText,
                            DataNascimento = DateTime.ParseExact(node.SelectSingleNode("DataNascimento")?.InnerText,
                                "dd/MM/yyyy", null),
                            DataFalecimento = DateTime.ParseExact(node.SelectSingleNode("DataFalecimento")?.InnerText,
                                "dd/MM/yyyy",
                                null).Equals(DateTime.MinValue)
                                ? semData
                                : DateTime.ParseExact(node.SelectSingleNode("DataFalecimento")?.InnerText,
                                    "dd/MM/yyyy", null),
                            Pais = PaisVm.ListaPaises()
                                .FirstOrDefault(x => x.Sigla == node.SelectSingleNode("Pais")?.InnerText)
                        });

                return Pilotos = new ObservableCollection<Piloto>(Pilotos.OrderBy(x => x.Nome));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem do registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new PilotoCadViewModel(PilotoTipo);
            AbreJanela("Novo Piloto", viewModel);
            ListaPilotos();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaPilotos();
        }

        #endregion Protected Methods
    }
}