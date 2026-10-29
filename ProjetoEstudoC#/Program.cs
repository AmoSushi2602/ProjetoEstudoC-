using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;

class Produto
{
    public int Codigo { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }

    public int Estoque { get; set; }

    public Produto(int codigo, string nome, decimal preco, int estoque)
    {
        Codigo = codigo;
        Nome = nome;
        Preco = preco;
        Estoque = estoque;
    }
}

class Program
{
    //Métodos

    //criar dados da conexão
    static readonly string connectionString =
        "Host = localhost;Port = 5432;Database = sistema_estoque;Username = postgres;Password = SOEUSEI;";

    static NpgsqlConnection CriarConexao()
    {
        return new NpgsqlConnection(connectionString);
    }

    static void ConexaoSQL()
    {
        try
        {
            using (NpgsqlConnection conexao = CriarConexao())
            {
                conexao.Open();

                Console.WriteLine("Conexão com o banco realizada");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro na conexão: {ex.Message}");
        }
    }

    static void Saudacoes()
    {
        Console.WriteLine("Bem vindo ao sistema");
        Console.WriteLine("Aqui você vai ter o controle de saida e entrada dos produtos da sua loja");
        Console.WriteLine("E tirar relatorios");
    }

    static int Opcoes()
    {
        int resposta;
        while (true)
        {
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1 - Cadastrar produto");
            Console.WriteLine("2 - Buscar Produto");
            Console.WriteLine("3 - Listar produtos");
            Console.WriteLine("4 - Consultar resumo do estoque");
            Console.WriteLine("5 - Entrada de estoque");
            Console.WriteLine("6 - Saida de estoque");
            Console.WriteLine("0 - Sair");

            bool respostaValida = int.TryParse(
                Console.ReadLine() ?? "",


                out resposta
                );

            if (!respostaValida)
            {
                Console.WriteLine("Escreva uma resposta Válida!");
                continue;

            }
            if (resposta < 0 || resposta > 6)
            {
                Console.WriteLine("Está não e uma opção");
                continue;
            }


            return (resposta);
        }
    }

    static void MostrarProdutos(Produto produto)
    {
        Console.WriteLine($"Código: {produto.Codigo}");
        Console.WriteLine($"Nome: {produto.Nome}");
        Console.WriteLine($"Preço: {produto.Preco:F2}");
        Console.WriteLine($"Estoque: {produto.Estoque}");
    }

    static void ListarProdutosBanco()
    {
        using (NpgsqlConnection conexao = CriarConexao())
        {
            conexao.Open();
            string comando = "SELECT codigo, nome, preco, estoque FROM PRODUTOS";

            using (NpgsqlCommand comandoExecutado = new NpgsqlCommand(comando, conexao))
            {
                using (NpgsqlDataReader leitor = comandoExecutado.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        int codigo = leitor.GetInt32(0);
                        string nome = leitor.GetString(1);
                        decimal preco = leitor.GetDecimal(2);
                        int estoque = leitor.GetInt32(3);
                        Produto produto = new Produto(codigo, nome, preco, estoque);
                        MostrarProdutos(produto);
                    }
                }
            }

        }
    }

    static decimal ValorEstoqueProduto(Produto produto)
    {
        return produto.Preco * produto.Estoque;
    }

    static decimal ValorTotalEstoque(List<Produto> produtos)
    {
        decimal valorTotalEstoque = 0;

        foreach (Produto item in produtos)
        {
            valorTotalEstoque += ValorEstoqueProduto(item);
        }
        return valorTotalEstoque;
    }

    static void CadastrarProduto(List<Produto> produtos)
    {
        //Declara a variavel fora do loop para poder usar depois
        int codigo;
        string nome;
        decimal preco;
        int estoque;

        while (true)
        {
            Console.WriteLine("Qual o codigo do Produto");
            bool codigoValido = int.TryParse(
                Console.ReadLine(),
                out codigo //retira o int para nao tentar criar uma var 2 vezes
             );

            if (!codigoValido)
            {
                Console.WriteLine("Escreve um numero zé");
                continue;
            }

            if (codigo <= 0)
            {
                Console.WriteLine("O nuemero deve ser positivo");
                continue;
            }

            if (produtos.Any(p => p.Codigo == codigo))
            {
                Console.WriteLine("Já exite este código de produto zé");
                continue;
            }
            break;
        }

        while (true)
        {
            Console.WriteLine("Qual o nome:");
            nome = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("Escreve um nome né");
                continue;
            }
            break;

        }

        while (true)
        {
            Console.WriteLine("Qual o preço:");
            bool conversaoValida = decimal.TryParse(
                Console.ReadLine(),
                out preco);
            if (conversaoValida == false)
            {
                Console.WriteLine("Escreve um numero né");
                continue;
            }


            if (preco <= 0)
            {
                Console.WriteLine("Vai entregar de graça?");
                continue;
            }
            break;
        }

        while (true)
        {
            Console.WriteLine("Quanto tem em estoque? ");
            bool estoqueValido = int.TryParse(Console.ReadLine(),
                out estoque
                );
            if (!estoqueValido)
            {
                Console.WriteLine("Escreve um numero zé");
                continue;
            }

            if (estoque < 0)
            {
                Console.WriteLine("Negativo nao doido");
                continue;
            }
            break;
        }
        Produto produto = new Produto(codigo, nome, preco, estoque);
        CadastrarProdutoBanco(produto);
        produtos.Add(produto);
    }

    static void CadastrarProdutoBanco(Produto produto)
    {
        using (NpgsqlConnection conexao = CriarConexao())
        {
            conexao.Open();

            string comando = "INSERT INTO PRODUTOS(codigo, nome, preco, estoque) VALUES (@codigo, @nome, @preco, @estoque)";

            using (NpgsqlCommand comandoExecutado =
                new NpgsqlCommand(comando, conexao))
            {
                comandoExecutado.Parameters.AddWithValue("@codigo", produto.Codigo);
                comandoExecutado.Parameters.AddWithValue("@nome", produto.Nome);
                comandoExecutado.Parameters.AddWithValue("@preco", produto.Preco);
                comandoExecutado.Parameters.AddWithValue("@estoque", produto.Estoque);

                comandoExecutado.ExecuteNonQuery();
            }
        }
    }

    static Produto? BuscarProduto(List<Produto> produtos, int codigo)
    {
        foreach (Produto item in produtos)
        {
            if (item.Codigo == codigo)
            {
                return item;
            }
        }
        return null;
    }

    static Produto? BuscarProdutoBanco(int codigo)
    {
        using (NpgsqlConnection conexao = CriarConexao())
        {
            conexao.Open();

            string comando = "SELECT codigo, nome, preco, estoque FROM PRODUTOS WHERE codigo = @codigo";

            using (NpgsqlCommand comandoExecutado = new NpgsqlCommand(comando, conexao))
            {
                comandoExecutado.Parameters.AddWithValue("@codigo", codigo);
                using (NpgsqlDataReader leitor = comandoExecutado.ExecuteReader())
                {
                    if (leitor.Read())
                    {
                        int codigoProduto = leitor.GetInt32(0);
                        string nome = leitor.GetString(1);
                        decimal preco = leitor.GetDecimal(2);
                        int estoque = leitor.GetInt32(3);

                        return new Produto(codigoProduto, nome, preco, estoque);
                    }
                }

            }
        }
        else
        {
            return null;
        }

    }

    static void ResumoCadastro(List<Produto> produtos)
    {
        if (produtos.Count <= 0)
        {
            Console.WriteLine("Não tem produtos cadastrados");
        }
        else
        {
            Console.WriteLine($"O total de produtos cadastrados foi {produtos.Count}");

            int produtosComEstoqueBaixo = produtos.Count(p => p.Estoque < 5);

            Console.WriteLine($"Produtos com estoque baixo: {produtosComEstoqueBaixo}! ");

            decimal valorTotalEstoque = ValorTotalEstoque(produtos);

            Console.WriteLine//Retorna o valor total do estoque
            (
                $"O valor total do estoque é de R$ {valorTotalEstoque:F2}"
            );

            foreach (Produto item in produtos)
            {

                MostrarProdutos(item);

                decimal valorEmEstoque = ValorEstoqueProduto(item);


                Console.WriteLine($"Seu produto: {valorEmEstoque:f2}");
            }
        }
    }

    static int AdicionarEntradaEstoque(Produto produto, int quantidade)
    {
        produto.Estoque += quantidade;
        return produto.Estoque;
    }

    static void EntradaEstoque(List<Produto> produtos)
    {

        if (produtos.Count <= 0)
        {
            Console.WriteLine("Não tem produtos para ser alterado!");
            return;
        }


        while (true)
        {

            Console.WriteLine("Digite o código do produto que deseja alterar:");
            bool codigoValido = int.TryParse(
                Console.ReadLine(),
                out int codigoBusca
            );

            if (!codigoValido)
            {
                Console.WriteLine("Escreva um valor valido");
                continue;
            }

            Produto? encontrado = BuscarProduto(produtos, codigoBusca);
            if (encontrado == null)
            {
                Console.WriteLine("Produto não encontrado");
                continue;
            }

            Console.WriteLine("Produto encontrado:");
            MostrarProdutos(encontrado);

            while (true)
            {
                Console.WriteLine("Quanto deseja adicionar ao estoque?");
                bool quantidadeValida = int.TryParse(
                    Console.ReadLine(),
                    out int quantidadeAlterada
                );

                if (!quantidadeValida || quantidadeAlterada <= 0)
                {
                    Console.WriteLine("Digite um valor inteiro maior que zero");
                    continue;
                }

                int estoqueNovo = AdicionarEntradaEstoque(encontrado, quantidadeAlterada);
                Console.WriteLine($"O Estoque novo é de :{estoqueNovo} ");
                break;
            }
            break;
        }
    }

    static int SubtrairSaidaEstoque(Produto produto, int quantidade)
    {
        if (quantidade <= 0)
        {
            throw new ArgumentException("A quantidade da saida deve ser maior que zero");
        }
        if (produto.Estoque < quantidade)
        {
            throw new InvalidOperationException("Não pode ser feito esta operação, o estoque ficará negativo");
        }
        produto.Estoque -= quantidade;
        return produto.Estoque;
    }

    static void SaidaEstoque(List<Produto> produtos)
    {
        if (produtos.Count <= 0)
        {
            Console.WriteLine("Não tem produtos para dar saida");
            return;
        }

        while (true)
        {
            Console.WriteLine("Digite o código do produto que deseja dar saida");
            bool codigoValido = int.TryParse(
                Console.ReadLine(),
                out int codigoBusca
                );
            if (!codigoValido)
            {
                Console.WriteLine("Escreva um código válido");
                continue;
            }
            Produto? encontrado = BuscarProduto(produtos, codigoBusca);
            if (encontrado == null)
            {
                Console.WriteLine("Produto não encontrado");
                continue;
            }
            Console.WriteLine("Produto encontrado");
            MostrarProdutos(encontrado);

            while (true)
            {
                try
                {
                    Console.WriteLine("Digite a quantidade que deseja dar saida:");
                    bool quantidadeValida = int.TryParse(
                        Console.ReadLine(),
                        out int quantidadeAlterada
                        );
                    if (!quantidadeValida)
                    {
                        Console.WriteLine("Digite uma quantidade validade");
                        continue;
                    }
                    int estoqueNovo = SubtrairSaidaEstoque(encontrado, quantidadeAlterada);

                    Console.WriteLine($"O estoque novo é de {estoqueNovo}");
                    break;
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("A quantidade da saida deve ser maior que zero");
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Não pode ser feito esta operação, o estoque ficará negativo");
                }
            }
            break;
        }

    }




    //static bool PesquisaCodProd

    //Método principal ----------------------------------------
    static void Main(string[] args)
    {
        //Tenta conectar no banco
        ConexaoSQL();

        //Inicia o programa com as saudações
        Saudacoes();

        List<Produto> produtos = new List<Produto>();

        int opcao = Opcoes();


        while (opcao != 0)
        {

            //Loop de cadastro
            if (opcao == 1)
            {
                CadastrarProduto(produtos);

                Console.WriteLine("Criado com sucesso!");

            }

            // busca de produto
            else if (opcao == 2)
            {
                if (BuscarProdutoBanco(codigoBusca) == null)
                {
                    Console.WriteLine("Não tem produtos para buscar");

                }
                else
                {

                    while (true)
                    {

                        Console.WriteLine("Digite o código do produto que deseja buscar: ");

                        bool codigoValido = int.TryParse(
                            Console.ReadLine(),
                            out int codigoBusca
                        );

                        if (codigoValido)
                        {
                            Produto? encontrado = BuscarProdutoBanco(codigoBusca);

                            if (encontrado != null)
                            {
                                Console.WriteLine("Produto encontrado!");
                                MostrarProdutos(encontrado);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Produto não encontrado");
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Código invalido");
                            continue;
                        }
                    }
                }
            }

            //listar produtos
            else if (opcao == 3)
            {

                ListarProdutosBanco();
            }

            //Resumo do cadastro
            else if (opcao == 4)
            {
                ResumoCadastro(produtos);
            }

            //Entrada de estoque
            else if (opcao == 5)
            {
                EntradaEstoque(produtos);
            }

            //Saida de estoque
            else if (opcao == 6)
            {
                SaidaEstoque(produtos);
            }

            //retorna menu 
            opcao = Opcoes();
        }
    }
}
