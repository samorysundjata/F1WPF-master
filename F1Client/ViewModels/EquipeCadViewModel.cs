using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class EquipeCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Equipe EquipeTipo = new Equipe();
        private DateTime? _despedida;
        private DateTime _estreia;
        private string _nome;
        private Pais _pais;
        private ObservableCollection<Pais> _paises;

        #endregion Private Fields

        #region Public Constructors

        public EquipeCadViewModel(Equipe equipeSelecionada)
        {
            EquipeSelecionada = equipeSelecionada;
            if (equipeSelecionada.Nome == null &&
                equipeSelecionada.Estreia == DateTime.MinValue &&
                equipeSelecionada.Pais == null)
            {
                EquipeSelecionada = null;
                Estreia = DataInicio;
                Despedida = DateTime.Now;
            }
            else
            {
                Nome = equipeSelecionada.Nome;
                Estreia = equipeSelecionada.Estreia;
                Despedida = equipeSelecionada.Despedida;
                Pais = equipeSelecionada.Pais;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public bool CanSalvar
        {
            get { return true; }
        }

        public DateTime? Despedida
        {
            get { return _despedida; }

            set
            {
                _despedida = value;
                NotifyOfPropertyChange(() => Despedida);
            }
        }

        public Equipe EquipeSelecionada { get; }

        public DateTime Estreia
        {
            get { return _estreia; }

            set
            {
                _estreia = value;
                NotifyOfPropertyChange(() => Estreia);
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

        #endregion Public Properties

        #region Public Methods

        public void Atual()
        {
            Despedida = null;
        }

        public void Salvar()
        {
            try
            {
                var dados = new string[4];
                dados[0] = Nome.Trim();
                dados[1] = Estreia.ToShortDateString();
                dados[2] = Despedida == null
                    ? DateTime.MinValue.ToShortDateString()
                    : Despedida.Value.ToShortDateString();
                dados[3] = Pais.Sigla.ToUpper();

                if (EquipeSelecionada == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(EquipeTipo.GetType(), dados);
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
                    if (!VerificaRepetido(EquipeSelecionada.Nome, dados))
                    {
                        IdadosF1.EditarDados(EquipeTipo.GetType(), EquipeSelecionada.Nome, dados);
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
            var listaEquipe = EquipeVm.ListaEquipes();
            if (!listaEquipe.Any()) return false;
            var itemEquipe = listaEquipe.FirstOrDefault(x => x.Nome == Nome);
            return itemEquipe != null;
        }

        private bool VerificaRepetido(string equipeSelecionadaNome, string[] dados)
        {
            var listaEquipe = EquipeVm.ListaEquipes();
            if (!listaEquipe.Any()) return false;
            var itemEquipe = listaEquipe.FirstOrDefault(x => x.Nome == dados[0] && x.Nome != equipeSelecionadaNome);
            return itemEquipe != null;
        }

        #endregion Private Methods
    }
}