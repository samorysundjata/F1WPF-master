using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace F1Client.ViewModels
{
    internal class PontuacaoCadViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Pontuacao PontuacaoTipo = new Pontuacao();

        private int _numero;

        private int _pontos;

        private int _posicao;

        private Temporada _temporada;

        private ObservableCollection<Temporada> _temporadas;

        #endregion Private Fields

        #region Public Constructors

        public PontuacaoCadViewModel(Pontuacao pontuacaoSelecionada)
        {
            PontuacaoSelecionada = pontuacaoSelecionada;
            if (pontuacaoSelecionada.Numero.Equals(0) &&
                pontuacaoSelecionada.Temporada == null &&
                pontuacaoSelecionada.Posicao.Equals(0) &&
                pontuacaoSelecionada.Pontos.Equals(0))
            {
                PontuacaoSelecionada = null;
            }
            else
            {
                Numero = pontuacaoSelecionada.Numero;
                Temporada = pontuacaoSelecionada.Temporada;
                Posicao = pontuacaoSelecionada.Posicao;
                Pontos = pontuacaoSelecionada.Pontos;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public bool CanSalvar
        {
            get
            {
                return !Pontos.Equals(0) &&
                       !Posicao.Equals(0) &&
                       Temporada != null;
            }
        }

        public int Numero
        {
            get { return _numero; }

            set
            {
                _numero = value;
                NotifyOfPropertyChange(() => Numero);
            }
        }

        public int Pontos
        {
            get { return _pontos; }

            set
            {
                _pontos = value;
                NotifyOfPropertyChange(() => Pontos);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public Pontuacao PontuacaoSelecionada { get; }

        public int Posicao
        {
            get { return _posicao; }

            set
            {
                _posicao = value;
                NotifyOfPropertyChange(() => Posicao);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public Temporada Temporada
        {
            get { return _temporada; }

            set
            {
                _temporada = value;
                NotifyOfPropertyChange(() => Temporada);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        public ObservableCollection<Temporada> Temporadas
        {
            get { return _temporadas; }

            set
            {
                _temporadas = value;
                NotifyOfPropertyChange(() => Temporadas);
                NotifyOfPropertyChange(() => CanSalvar);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Salvar()
        {
            try
            {
                var dados = new string[4];
                dados[0] = GeraPontuacao();
                dados[1] = Pontos.ToString();
                dados[2] = Posicao.ToString();
                dados[3] = Temporada.Numero.ToString();

                if (PontuacaoSelecionada == null)
                {
                    if (!VerificaExistencia())
                    {
                        IdadosF1.SalvarDados(PontuacaoTipo.GetType(), dados);
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
                    if (!VerificaRepetido(PontuacaoSelecionada.Numero, dados))
                    {
                        IdadosF1.EditarDados(PontuacaoTipo.GetType(), PontuacaoSelecionada.Numero.ToString(), dados);
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
            Temporadas = TemporadaVm.ListaTemporadas();
        }

        #endregion Protected Methods

        #region Private Methods

        private string GeraPontuacao()
        {
            var maxId = 0;
            var listaPontuacao = PontuacaoVm.ListaPontuacoes();

            if (listaPontuacao.Any())
                maxId = listaPontuacao.Max(x => x.Numero);

            maxId++;

            var pontuacao = maxId.ToString();

            return pontuacao;
        }

        private bool VerificaExistencia()
        {
            var listaPontuacao = PontuacaoVm.ListaPontuacoes();
            if (!listaPontuacao.Any()) return false;
            var itemPontuacao = listaPontuacao.FirstOrDefault(x =>
                x.Temporada == Temporada && x.Posicao == Posicao && x.Pontos == Pontos);
            return itemPontuacao != null;
        }

        private bool VerificaRepetido(int pontuacaoSelecionadaNumero, string[] dados)
        {
            var listaPontuacao = PontuacaoVm.ListaPontuacoes();
            if (!listaPontuacao.Any()) return false;
            var itemPontuacao = listaPontuacao.FirstOrDefault(x => x.Numero != pontuacaoSelecionadaNumero &&
                                                                   x.Temporada.Numero == Convert.ToInt32(dados[3]) &&
                                                                   x.Posicao == Convert.ToInt32(dados[2]));
            return itemPontuacao != null;
        }

        #endregion Private Methods
    }
}