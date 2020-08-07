using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class MotorCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Motor MotorTipo = new Motor();
        private string _nome;
        private Pais _pais;
        private ObservableCollection<Pais> _paises;

        #endregion Private Fields

        #region Public Constructors

        public MotorCadViewModel(Motor motorSelecionado)
        {
            MotorSelecionado = motorSelecionado;
            if (motorSelecionado.Nome == null && motorSelecionado.Pais == null)
            {
                MotorSelecionado = null;
            }
            else
            {
                Nome = motorSelecionado.Nome;
                Pais = motorSelecionado.Pais;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public bool CanSalvar
        {
            get { return !string.IsNullOrWhiteSpace(Nome) && !string.IsNullOrWhiteSpace(Pais.Sigla); }
        }

        public Motor MotorSelecionado { get; }

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

        public void Salvar()
        {
            try
            {
                var dados = new string[2];
                dados[0] = Nome.Trim();
                dados[1] = Pais.Sigla;

                if (MotorSelecionado == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(MotorTipo.GetType(), dados);
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
                    if (!VerificaRepetido(MotorSelecionado.Nome, dados))
                    {
                        IdadosF1.EditarDados(MotorTipo.GetType(), MotorSelecionado.Nome, dados);
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
            var listaMotores = MotorVm.ListaMotores();
            if (!listaMotores.Any()) return false;
            var itemMotor = listaMotores.FirstOrDefault(x => x.Nome == Nome);
            return itemMotor != null;
        }

        private bool VerificaRepetido(string motorSelecionadoNome, string[] dados)
        {
            var listaMotores = MotorVm.ListaMotores();
            if (!listaMotores.Any()) return false;
            var itemMotor = listaMotores.FirstOrDefault(x => x.Nome == dados[0] && x.Nome != motorSelecionadoNome);
            return itemMotor != null;
        }

        #endregion Private Methods
    }
}