using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace Atividade_CSharp
{
    internal class Pessoa
    {
        public string Nome { get; set; }
        public string Cpf { get; private set; }
        public string Sus { get; private set; }
        public int Idade { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }




        public void Cadastrar()
        {
            Console.Clear();
            Console.WriteLine("Informe seu Nome:");
            this.Nome = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Informe seu CPF:");
            this.Cpf = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Informe o Numero do seu Cartao do SUS:");
            this.Sus = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Informe sua Idade:");
            this.Idade = int.Parse(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("Informe seu Endereco:");
            this.Endereco = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Informe seu Telefone:");
            this.Telefone = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Informe seu Email:");
            this.Email = Console.ReadLine();
        }

       
                    }
                }