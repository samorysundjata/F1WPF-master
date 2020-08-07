using F1Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace F1Client.ViewModels
{
    internal class PontuacaoViewModel : ViewModelBase
    {
        #region Private Fields

        private static readonly Pontuacao PontuacaoTipo = new Pontuacao();
        private int _numero;
        private int _pontos;
        private Pontuacao _pontuacaoSelecionada;
        private ObservableCollection<Pontuacao> _pontuacoes;
        private int _posicao;
        private Temporada _temporada;

        #endregion Private Fields

        #region Public Properties

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
            }
        }

        public Pontuacao PontuacaoSelecionada
        {
            get { return _pontuacaoSelecionada; }

            set
            {
                _pontuacaoSelecionada = value;
                NotifyOfPropertyChange(() => PontuacaoSelecionada);
            }
        }

        public ObservableCollection<Pontuacao> Pontuacoes
        {
            get { return _pontuacoes; }

            set
            {
                _pontuacoes = value;
                NotifyOfPropertyChange(() => Pontuacoes);
            }
        }

        public int Posicao
        {
            get { return _posicao; }

            set
            {
                _posicao = value;
                NotifyOfPropertyChange(() => Posicao);
            }
        }

        public Temporada Temporada
        {
            get { return _temporada; }

            set
            {
                _temporada = value;
                NotifyOfPropertyChange(() => Temporada);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Editar()
        {
            if (PontuacaoSelecionada != null)
            {
                var viewModel = new PontuacaoCadViewModel(PontuacaoSelecionada);
                AbreJanela("Editar Pontuação", viewModel);
                ListaPontuacoes();
            }
            else
            {
                MessageBox.Show("Selecione uma pontuação para editar!", "Erro ao editar", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Excluir()
        {
            if (PontuacaoSelecionada != null)
            {
                var messageBoxResult = MessageBox.Show(
                    "Deseja mesmo excluir a pontuação " + PontuacaoSelecionada.Numero + "?",
                    "Confirme a exclusão", MessageBoxButton.YesNo,
                    MessageBoxImage.Question); /*Melhorar esta mensagem para incluir a temporada*/

                if (messageBoxResult != MessageBoxResult.Yes) return;
                IdadosF1.ExcluirDados(PontuacaoTipo.GetType(), PontuacaoSelecionada.Numero.ToString());
                ListaPontuacoes();
            }
            else
            {
                MessageBox.Show("Selecione uma pontuação para excluir!", "Erro ao excluir", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Pontuacao> ListaPontuacoes()
        {
            try
            {
                Pontuacoes = new ObservableCollection<Pontuacao>();

                var nos = IdadosF1.ListaDados(PontuacaoTipo.GetType());

                if (nos != null)
                    foreach (XmlNode node in nos)
                        Pontuacoes.Add(new Pontuacao
                        {
                            Numero = Convert.ToInt32(node.SelectSingleNode("Numero")?.InnerText),
                            Temporada = TemporadaVm.ListaTemporadas()
                                .FirstOrDefault(x =>
                                    x.Numero == Convert.ToInt32(node.SelectSingleNode("Temporada")?.InnerText)),
                            Pontos = Convert.ToInt32(node.SelectSingleNode("Pontos")?.InnerText),
                            Posicao = Convert.ToInt32(node.SelectSingleNode("Posicao")?.InnerText)
                        });

                return Pontuacoes =
                    new ObservableCollection<Pontuacao>(Pontuacoes.OrderBy(z => z.Temporada.Numero)
                        .ThenBy(t => t.Posicao));
            }
            catch (Exception ex)
            {
                TrataExcecao(ex, " ocorreu na listagem do registros!", "Erro na listagem");
                return null;
            }
        }

        public void Novo()
        {
            var viewModel = new PontuacaoCadViewModel(PontuacaoTipo);
            AbreJanela("Nova Pontuação", viewModel);
            ListaPontuacoes();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ListaPontuacoes();
        }

        #endregion Protected Methods
    }
}