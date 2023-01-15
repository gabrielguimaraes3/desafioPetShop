using PrimeiroDesafio.Modelos;
using PrimeiroDesafio.Servicos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeiroDesafio.Fluxos
{
    internal class Fluxo
    {

        private readonly string _path = "C:\\ProejtosCurso\\AtividadesCSharp\\ClientesPetShop.csv";
        private List<Pessoa> ListaPessoas = new List<Pessoa>();
        public Fluxo()
        {
            if (!File.Exists(_path))
            {
                var file = File.Create(_path);
                file.Close();
            }
        }
        public void InserirPessoa(Pessoa pessoa)
        {
            var sw = new StreamWriter(_path, true);
            sw.WriteLine(GerarStriingPessoa(pessoa));
            sw.Close();
        }

        public List<Pessoa> ListarPssoas()
        {
            CarregarPessoasDaLista();
            return ListaPessoas;
        }
        public bool CpfExist(string cpfEscolhido)
        {
            CarregarPessoasDaLista();



            foreach (var cpf in ListaPessoas)
            {
                if (cpf.Cpf == cpfEscolhido) return true;
            }
            return false;
        }
        public void Atualzar(Pessoa pessoa)
        {
            CarregarPessoasDaLista();
            var posicao = ListaPessoas.FindIndex(x => x.Cpf == pessoa.Cpf);
            ListaPessoas[posicao] = pessoa;
            Regravar(ListaPessoas);
        }
        public void RemoverPessoa(string cpfEscolhido)
        {
            CarregarPessoasDaLista();
            var posicao = ListaPessoas.FindIndex(x => x.Cpf == cpfEscolhido);
            ListaPessoas.RemoveAt(posicao);
            Regravar(ListaPessoas);
        }
        private void Regravar(List<Pessoa> Pessoa)
        {
            var sw = new StreamWriter(_path);

            foreach(var pessoa in ListaPessoas.OrderBy(x => x.Cpf))
            {
                sw.WriteLine(GerarStriingPessoa(pessoa));
            }
            sw.Close();
        }

        public static bool ValidacaoNome(string nome)
        {
            nome = nome.Trim();

            if (nome.Length < 3 || nome.Length > 80)
            {
                Console.WriteLine("Nome invalido. Tente novamente.");
                return false;
            }

            return true;
        }
        public static bool ValidaIdade(DateTime dataNascimento)
        {
            var birthdate = dataNascimento;
            var today = DateTime.Now;
            var age = today.Year - birthdate.Year;
            if (birthdate > today.AddYears(-age)) age--;

            if (age < 16 || age > 120) return false;

            Console.WriteLine(age);
            
            return true;
        }
        public static bool ValidarCpf(string cpf)
        {
            

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
        private Pessoa LinhaTextoParaPessoa(string linha)
        {
            var colunas = linha.Split(';');

            var pessoa = new Pessoa();
            pessoa.nome = colunas[0];
            pessoa.idade = Convert.ToDateTime(colunas[1]);
            pessoa.Cpf = (colunas[2]);

            return pessoa;
        }
        private string GerarStriingPessoa(Pessoa pessoa)
        {
            return $"{pessoa.nome}; {pessoa.idade}; {pessoa.Cpf};";
        }
        private void CarregarPessoasDaLista()
        {
            ListaPessoas.Clear();
            var sr = new StreamReader(_path);
            while (true)
            {
                var linha = sr.ReadLine();

                if (linha == null)
                    break;

                ListaPessoas.Add(LinhaTextoParaPessoa(linha));
            }

            sr.Close();
        }
    }
}
