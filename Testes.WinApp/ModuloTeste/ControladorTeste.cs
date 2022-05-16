
using System.Collections.Generic;
using System.Windows.Forms;
using Testes.Dominio.ModuloTeste;
using Testes.Infra.Arquivos.ModuloTeste;
using Testes.WinApp.Compartilhado;

namespace Testes.WinApp.ModuloTeste
{
    internal class ControladorTeste : ControladorBase
    {
        private RepositorioTesteEmArquivo repositorioTeste;
        private TabelaTesteControl tabelaTestes;
        public ControladorTeste(RepositorioTesteEmArquivo repositorioTeste)
        {
            this.repositorioTeste = repositorioTeste;
        }
        public override void Inserir()
        {
            TelaCadastroTesteForm tela = new TelaCadastroTesteForm();
            tela.Teste = new Teste();

            tela.GravarRegistro = repositorioTeste.Inserir;

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarTeste();
            }
        }
        public override void Editar()
        {
            Teste TesteSelecionada = ObtemTesteSelecionada();

            if (TesteSelecionada == null)
            {
                MessageBox.Show("Selecione um teste primeiro",
                "Edição de Testes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TelaCadastroTesteForm tela = new TelaCadastroTesteForm();

            tela.Teste = TesteSelecionada;

            tela.GravarRegistro = repositorioTeste.Editar;

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarTeste();
            }
        }
        public override void Excluir()
        {
            Teste tarefaSelecionada = ObtemTesteSelecionada();

            if (tarefaSelecionada == null)
            {
                MessageBox.Show("Selecione um teste primeiro",
                "Exclusão de Testes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult resultado = MessageBox.Show("Deseja excluir esse teste?",
                "Exclusão de Testes", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (resultado == DialogResult.OK)
            {
                repositorioTeste.Excluir(tarefaSelecionada);
                CarregarTeste();
            }
        }
        public override void Duplicar()
        {
            Teste TesteSelecionada = ObtemTesteSelecionada();

            if (TesteSelecionada == null)
            {
                MessageBox.Show("Selecione um teste primeiro",
                "Edição de Testes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            repositorioTeste.Inserir(TesteSelecionada);

            CarregarTeste();
        }

        private void CarregarTeste()
        {
            List<Teste> Testes = repositorioTeste.SelecionarTodos();

            tabelaTestes.AtualizarRegistros(Testes);
        }
        public override UserControl ObtemListagem()
        {
            if (tabelaTestes == null)
                tabelaTestes = new TabelaTesteControl();

            CarregarTeste();

            return tabelaTestes;
        }
        private Teste ObtemTesteSelecionada()
        {
            int numero = (int)tabelaTestes.ObtemNumeroTesteSelecionado();

            return repositorioTeste.SelecionarPorNumero(numero);
        }
    }
}
