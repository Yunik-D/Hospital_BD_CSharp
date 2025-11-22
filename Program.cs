using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Atividade_CSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu inicio = new Menu();
            Pessoa[] array = new Pessoa[15];
            const int limite = 3;
            int contador;
            bool continuar = true;

            while (continuar)
            {
                string cs = "server=127.0.0.1;uid=root;database=hospital;Port=3306";

                using (var conexao = new MySqlConnection(cs))
                {
                    try
                    {
                        conexao.Open();
                    }
                    catch (MySqlException erro)
                    {
                        Console.WriteLine("Erro ao conectar ao banco de dados. \n" + erro.Message);
                        // Não tentar executar comandos com a conexão fechada
                        Console.WriteLine("Pressione ENTER para tentar novamente ou Q para sair...");
                        var k = Console.ReadLine();
                        if (!string.IsNullOrEmpty(k) && k.ToUpper() == "Q")
                            return;

                        Console.Clear();
                        continue; // volta ao início do while para tentar reconectar
                    }

                    // Garantir que a conexão está aberta antes de executar comandos
                    if (conexao.State != ConnectionState.Open)
                    {
                        Console.WriteLine("Conexão não está aberta. Voltando ao menu.");
                        continue;
                    }

                    string bd = "SELECT contador FROM paciente ORDER BY contador DESC LIMIT 1";
                    using (var cmdBd = new MySqlCommand(bd, conexao))
                    {
                        object resultado = cmdBd.ExecuteScalar();
                        if (resultado == null || resultado == DBNull.Value)
                        {
                            contador = 0;
                        }
                        else
                        {
                            int tmp;
                            if (int.TryParse(resultado.ToString(), out tmp))
                                contador = tmp;
                            else
                                contador = 0;
                        }
                    }

                    switch (inicio.Hud())
                    {
                        case "C":
                            string res = "S";

                            while (res == "S" && contador < limite && contador < array.Length)
                            {
                                Pessoa paciente = new Pessoa();
                                paciente.Cadastrar();

                                array[contador] = paciente;

                                contador++;

                                Console.Clear();
                                string sqlInsert = "INSERT INTO paciente (Nome, Cpf, Sus, Idade, Endereco, Telefone, Email, Contador) VALUES (@nome, @cpf, @sus, @idade, @endereco, @telefone, @email, @contador)";
                                using (var cmdCa = new MySqlCommand(sqlInsert, conexao))
                                {
                                    cmdCa.Parameters.AddWithValue("@nome", paciente.Nome);
                                    cmdCa.Parameters.AddWithValue("@cpf", paciente.Cpf);
                                    cmdCa.Parameters.AddWithValue("@sus", paciente.Sus);
                                    cmdCa.Parameters.AddWithValue("@idade", paciente.Idade);
                                    cmdCa.Parameters.AddWithValue("@endereco", paciente.Endereco);
                                    cmdCa.Parameters.AddWithValue("@telefone", paciente.Telefone);
                                    cmdCa.Parameters.AddWithValue("@email", paciente.Email);
                                    cmdCa.Parameters.AddWithValue("@contador", contador);

                                    int linhas = cmdCa.ExecuteNonQuery();
                                }

                                Console.WriteLine("Paciente Cadastrado!! \n\nPressione ENTER para continuar...");
                                Console.ReadLine();
                                Console.Clear();


                                if (contador == limite)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Paciente Cadastrado!! \n\nLimite de Pacientes Excedido\n\n");
                                    Console.WriteLine("Pressione ENTER para voltar ao Menu...");
                                    Console.ReadLine();
                                    Console.Clear();

                                }
                                else
                                {
                                    Console.WriteLine("Deseja Cadastrar Outro Paciente? (S/N)");
                                    res = Console.ReadLine();
                                    res = res.ToUpper();
                                    Console.Clear();
                                    Console.WriteLine("Pressione ENTER para continuar...");
                                    Console.ReadLine();
                                    Console.Clear();
                                }
                                if (res != "S" && res != "N")
                                {
                                    Console.Clear();
                                    Console.WriteLine("Opção inválida. Voltando ao Menu.");
                                    res = "N";
                                    Console.Clear();
                                }
                            }
                            break;

                        case "L":
                            Console.Clear();
                            Console.WriteLine($"PACIENTES NA FILA ({contador + "/" + limite}):");
                            if (contador == 0)
                            {
                                Console.Clear();
                                Console.WriteLine("Não existem pacientes Cadastrados!\n\nPressione Enter para voltar ao Menu...");
                                Console.ReadLine();
                                Console.Clear();
                            }
                            else
                            {
                                string sqlSelect = "SELECT Nome FROM paciente ORDER BY Idade DESC";
                                using (var cmdLI = new MySqlCommand(sqlSelect, conexao))
                                using (var reader = cmdLI.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string nome = reader.GetString("Nome");
                                        Console.WriteLine($"Nome: {nome}");
                                    }
                                }

                                Console.WriteLine("\n\n\nPressione Enter para voltar ao Menu...");
                                Console.ReadLine();
                                Console.Clear();
                            }
                            break;

                        case "A":
                            Console.Clear();
                            if (contador == 0)
                            {
                                Console.WriteLine("A fila de atendimento está vazia!\n\n\nPressione ENTER para retornar ao Menu.");
                                Console.ReadLine();
                                Console.Clear();
                            }
                            else
                            {
                                string sqlSelect = "SELECT Id, Nome, Idade FROM paciente ORDER BY Idade DESC";
                                using (var cmdAt = new MySqlCommand(sqlSelect, conexao))
                                using (var reader = cmdAt.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string nome = reader.GetString("Nome");
                                        int idade = reader.GetInt32("Idade");
                                        int id = reader.GetInt32("Id");
                                        Console.WriteLine($"O Paciente {nome} de {idade} anos, finalizou o atendimento!!\n\n");
                                    }
                                }

                                string sqlBusca = "SELECT Id FROM paciente ORDER BY Idade DESC";
                                using (var cmdId = new MySqlCommand(sqlBusca, conexao))
                                {
                                    object buscaId = cmdId.ExecuteScalar();
                                    if (buscaId != null && buscaId != DBNull.Value)
                                    {
                                        int valorId = Convert.ToInt32(buscaId);

                                        string sqlDelete = "DELETE FROM paciente WHERE id = @id";
                                        using (var cmdDel = new MySqlCommand(sqlDelete, conexao))
                                        {
                                            cmdDel.Parameters.AddWithValue("@id", valorId);
                                            cmdDel.ExecuteNonQuery();
                                        }

                                        Console.WriteLine($"|*LISTA DE PACIENTES NA FILA ATUALIZADA ({contador - 1}/{limite})*|");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nenhum ID encontrado para deletar.");
                                    }
                                }

                                Console.WriteLine("\n\nPressione ENTER para retornar ao Menu");
                                Console.ReadLine();
                                Console.Clear();
                            }
                            break;

                        case "D":

                            Console.Clear();
                            if (contador == 0)
                            {
                                Console.WriteLine("Não existe nenhum paciente cadastrado no sistema. Voltando ao Menu. \n\n\n");
                            }

                            Console.WriteLine("Digite o CPF do paciente que voce deseja alterar os dados: ");
                            string cpfDigt = Console.ReadLine();

                            string sql = "SELECT COUNT(*) FROM paciente WHERE Cpf = @cpf";

                            using (var cmd = new MySqlCommand(sql, conexao))
                            {
                                cmd.Parameters.AddWithValue("@cpf", cpfDigt);

                                object countObj = cmd.ExecuteScalar();
                                int qtd = 0;
                                if (countObj != null && countObj != DBNull.Value)
                                    qtd = Convert.ToInt32(countObj);

                                if (qtd > 0)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Digite seu nome novamente: \n");
                                    string novoNome = Console.ReadLine();
                                    Console.Clear();
                                    Console.WriteLine("Digite seu cartao do sus novamente: \n");
                                    string novoSus = Console.ReadLine();
                                    Console.Clear();
                                    Console.WriteLine("Digite sua idade novamente: \n");
                                    string novaIdade = Console.ReadLine();
                                    Console.Clear();
                                    Console.WriteLine("Digite seu endereco novamente: \n");
                                    string novoEndereco = Console.ReadLine();
                                    Console.Clear();
                                    Console.WriteLine("Digite seu telefone novamente: \n");
                                    string novoTel = Console.ReadLine();
                                    Console.Clear();
                                    Console.WriteLine("Digite seu email novamente: \n");
                                    string novoEmail = Console.ReadLine();
                                    Console.Clear();
                                    Console.WriteLine("Todas suas informações foram alteradas.\n\nPressione ENTER para continuar...");
                                    Console.ReadLine();
                                    Console.Clear();

                                    string sqlupd = "UPDATE paciente SET nome=@Nome, sus=@Sus, idade=@Idade, endereco=@Endereco, telefone=@Telefone, email=@Email WHERE cpf = @cpf";

                                    using (var cmdUpd = new MySqlCommand(sqlupd, conexao))
                                    {
                                        cmdUpd.Parameters.AddWithValue("@Nome", novoNome);
                                        cmdUpd.Parameters.AddWithValue("@cpf", cpfDigt);
                                        cmdUpd.Parameters.AddWithValue("@Sus", novoSus);
                                        cmdUpd.Parameters.AddWithValue("@Idade", novaIdade);
                                        cmdUpd.Parameters.AddWithValue("@Endereco", novoEndereco);
                                        cmdUpd.Parameters.AddWithValue("@Telefone", novoTel);
                                        cmdUpd.Parameters.AddWithValue("@Email", novoEmail);
                                        cmdUpd.ExecuteNonQuery();
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("CPF NAO CADASTRADO!!");
                                    Console.Clear();
                                }
                            }
                            break;

                        case "Q":
                            Console.Clear();
                            continuar = false;
                            Console.WriteLine("Saindo do programa. Aperte ESC para fechar.");
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Opção Invalida! Tente Novamente:");
                            Console.WriteLine("Pressione ENTER para voltar ao Menu...");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                    }
                } // using conexao
            }
        }
    }
}