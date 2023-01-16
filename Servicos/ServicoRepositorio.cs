using NuGet.Protocol.Plugins;
using PrimeiroDesafio.Modelos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PrimeiroDesafio.Servicos
{
    public class ServicoRepositorio
    {
        private readonly Fluxos.Fluxo _path;
        public ServicoRepositorio()
        {
            _path = new Fluxos.Fluxo();
        }
        public void EscolhaDoUsuario()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Digite o Numero reeferente a sua eescolha:");
                Console.WriteLine("1 - Cadastrar Pessoa");
                Console.WriteLine("2 - Listar Pessoas");
                Console.WriteLine("3 - Atualizar Pessoa");
                Console.WriteLine("4 - Remover Pessoa");
                var resposta = Console.ReadLine();
                Console.Clear();

                switch (resposta)
                {
                    case "1":
                        CadastrarPessoa();
                        break;
                    case "2":
                        ListarPessoas();
                        break;
                    case "3":
                        AtualizarPessoa();
                        break;
                    case "4":
                        RemoverPessoa();
                        break;
                    default:
                        Console.WriteLine("Selecione uma opção válida");
                        break;
                }
            }

        }
        private void CadastrarPessoa()
        {
            var pessoa = ReceberDados();
            _path.InserirPessoa(pessoa);

            Console.WriteLine($"{pessoa.nome} cadastrado com sucesso!");
            Console.WriteLine($"Aperte uma tecla para prosseguir!");
            Console.ReadKey();
        }
        private void ListarPessoas()
        {
            Console.Clear();

            var pessoas = _path.ListarPssoas();

            foreach (var pessoa in pessoas)
            {
                Console.WriteLine($"Nome: {pessoa.nome}; Data de Nascimeento: {pessoa.idade}; Cpf: {pessoa.Cpf}");
            }
            Console.WriteLine("Repositorio lido com Sucesso.");
            Console.WriteLine("Aperte enteer para finalizar!");
            Console.ReadKey();
        }
        private void AtualizarPessoa()
        {
            Console.WriteLine("Informe o Cpf da pssoa que deseja atualzar:");
            string cpfEscolhido = Console.ReadLine();
            if (!_path.CpfExist(cpfEscolhido))
            {
                Console.WriteLine("Cpf invaldo \nTente novamnte.");
            }

            var pessoa = ReceberDados();
            pessoa.Cpf = cpfEscolhido;

            _path.Atualzar(pessoa);
        }
        private void RemoverPessoa()
        {
            Console.WriteLine("Informe o Cpf da pssoa que deseja remover:");
            string cpfEscolhido = Console.ReadLine();
            if (!_path.CpfExist(cpfEscolhido))
            {
                Console.WriteLine("Cpf invaldo \nTente novamnte.");
            }

            _path.RemoverPessoa(cpfEscolhido);

        }
        private Pessoa ReceberDados()
        {
            Console.WriteLine("Informe seu nome: ");
            var nome = Console.ReadLine();
            var nomeValido = Fluxos.Fluxo.ValidacaoNome(nome.ToUpper());
            if (nomeValido != true) ReceberDados();


            Console.WriteLine("Insire sau data de nascimento: dd/MM/AAAA");
            var dataNascimento = Convert.ToDateTime(Console.ReadLine());

            DateTime dataHoje = DateTime.Today;
            var idade = dataHoje - dataNascimento;
            Console.WriteLine(idade);
            if (idade.Days < 5840 || idade.Days > 43800)
            {
                Console.WriteLine("Idade invaliida.");
                Console.WriteLine("Você precisar ter entre 16  120 anos.");
                ReceberDados();
            }




            Console.WriteLine("Insira seu CPF: (Apenas Numeros)");
            string cpf = (Console.ReadLine());
            var cpfValido = Fluxos.Fluxo.ValidarCpf(cpf.Trim());
            if (cpfValido != true)
            {
                Console.WriteLine("Cpf invalido.");
                ReceberDados();
            }




            cpf = Regex.Replace(cpf, @"(\d{3})(\d{3})(\d{3})(\d{2})",
                                    "$1.$2.$3-$4");
            var cpfFinal = cpf;

            return new Pessoa()
            {
                nome = nome,
                idade = dataNascimento,
                Cpf = cpfFinal
            };

        }
    }
}
