
using System.Collections.Generic;
using System.Windows.Forms;
using Testes.Dominio.ModuloQuestao;
using Testes.Infra.Arquivos.ModuloQuestao;
using Testes.WinApp.Compartilhado;

namespace Testes.WinApp.ModuloQuestões
{
    public class ControladorQuestao : ControladorBase
    {
        private RepositorioQestaoEmArquivo repositorioQestao;
        private TabelaQuestaoControl tabelaQuestaos;

        public ControladorQuestao(RepositorioQestaoEmArquivo repositorioQestao)
        {
            this.repositorioQestao = repositorioQestao;
        }

        public override void Inserir()
        {
            TelaCadastroQuestaoForm tela = new TelaCadastroQuestaoForm();
            tela.Questao = new Questao();

            tela.GravarRegistro = repositorioQestao.Inserir;

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarQuestao();
            }
        }
        public override void Editar()
        {
            Questao QuestaoSelecionada = ObtemQuestaoSelecionada();

            if (QuestaoSelecionada == null)
            {
                MessageBox.Show("Selecione uma questão primeiro",
                "Edição de Questões", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TelaCadastroQuestaoForm tela = new TelaCadastroQuestaoForm();

            tela.Questao = QuestaoSelecionada;

            tela.GravarRegistro = repositorioQestao.Editar;

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarQuestao();
            }
        }

        public override void Excluir()
        {
            Questao tarefaSelecionada = ObtemQuestaoSelecionada();

            if (tarefaSelecionada == null)
            {
                MessageBox.Show("Selecione uma Questao primeiro",
                "Exclusão de Questões", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult resultado = MessageBox.Show("Deseja excluir essa questão?",
                "Exclusão de Questões", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (resultado == DialogResult.OK)
            {
                repositorioQestao.Excluir(tarefaSelecionada);
                CarregarQuestao();
            }
        }

        private void CarregarQuestao()
        {
            List<Questao> Questaos = repositorioQestao.SelecionarTodos();

            tabelaQuestaos.AtualizarRegistros(Questaos);
        }

        public override UserControl ObtemListagem()
        {
            if (tabelaQuestaos == null)
                tabelaQuestaos = new TabelaQuestaoControl();

            CarregarQuestao();

            return tabelaQuestaos;
        }

        private Questao ObtemQuestaoSelecionada()
        {
            var numero = tabelaQuestaos.ObtemNumeroQuestaoSelecionado();

            return repositorioQestao.SelecionarPorNumero(numero);
        }
    }
}
