using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class PilotoCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Piloto PilotoTipo = new Piloto();
        private DateTime? _dataFalecimento;
        private DateTime _dataNascimento;
        private string _nome;
        private Pais _pais;
        private ObservableCollection<Pais> _paises;

        #endregion Private Fields

        #region Public Constructors

        public PilotoCadViewModel(Piloto pilotoSelecionado)
        {
            PilotoSelecionado = pilotoSelecionado;
            if (pilotoSelecionado.Nome == null &&
                pilotoSelecionado.DataNascimento == DateTime.MinValue &&
                pilotoSelecionado.Pais == null)
            {
                PilotoSelecionado = null;
                DataNascimento = DateBirth;
            }
            else
            {
                Nome = pilotoSelecionado.Nome;
                DataNascimento = pilotoSelecionado.DataNascimento;
                DataFalecimento = pilotoSelecionado.DataFalecimento;
                Pais = pilotoSelecionado.Pais;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public bool CanSalvar
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Nome) &&
                       DataNascimento > DateBirth &&
                       !string.IsNullOrWhiteSpace(Pais.Sigla);
            }
        }

        public DateTime? DataFalecimento
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
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

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

        public Piloto PilotoSelecionado { get; }

        #endregion Public Properties

        #region Public Methods

        public void Salvar()
        {
            try
            {
                var dados = new string[4];
                dados[0] = Nome.Trim();
                dados[1] = DataNascimento.ToShortDateString();
                dados[2] = DataFalecimento.Equals(DateTime.MinValue) || DataFalecimento == null
                    ? DateTime.MinValue.ToShortDateString()
                    : DataFalecimento.Value.ToShortDateString();
                dados[3] = Pais.Sigla.ToUpper();

                if (PilotoSelecionado == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(PilotoTipo.GetType(), dados);
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
                    if (!VerificaRepetido(PilotoSelecionado.Nome, dados))
                    {
                        IdadosF1.EditarDados(PilotoTipo.GetType(), PilotoSelecionado.Nome, dados);
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
        }

        #endregion Protected Methods

        #region Private Methods

        private bool VerificaExistencia()
        {
            var pilotoVm = new PilotoViewModel();
            var listaPilotos = pilotoVm.ListaPilotos();
            if (!listaPilotos.Any()) return false;
            var itemPiloto = listaPilotos.FirstOrDefault(x => x.Nome == Nome);
            return itemPiloto != null;
        }

        private bool VerificaRepetido(string pilotoSelecionadoNome, string[] dados)
        {
            var pilotoVm = new PilotoViewModel();
            var listaPilotos = pilotoVm.ListaPilotos();
            if (!listaPilotos.Any()) return false;
            var itemPiloto = listaPilotos.FirstOrDefault(x => x.Nome == dados[0] && x.Nome != pilotoSelecionadoNome);
            return itemPiloto != null;
        }

        #endregion Private Methods
    }
}