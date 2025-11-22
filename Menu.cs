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
            Console.WriteLine("\n C. Cadastrar Paciente. \n L. Listar Pacientes. \n A. Atender Paciente. \n D. Alterar Dados Cadastrais. \n Q. Sair");
            Console.WriteLine("=================================================================");
            string valor = Console.ReadLine();
            valor = valor.ToUpper();
            return valor;
        }
    }
}
