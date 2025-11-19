using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atividade_CSharp
{
    internal class Menu
    {
        public string Hud()
        {
            Console.WriteLine("=================================================================");
            Console.WriteLine("Bem vindo ao programa do Hospital JK. Selecione a opção desejada:");
            Console.WriteLine("\n 1. Cadastrar Paciente. \n 2. Listar Pacientes. \n 3. Atender Paciente. \n 4. Alterar Dados Cadastrais. \n Q. Sair");
            Console.WriteLine("=================================================================");
            string valor = Console.ReadLine();
            valor = valor.ToUpper();
            return valor;
        }
    }
}
