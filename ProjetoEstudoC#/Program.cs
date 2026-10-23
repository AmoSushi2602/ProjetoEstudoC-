using System;
using System.Collections.Generic;
using System.Linq;

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

    static void Saudacoes()
    {
        Console.WriteLine("Bem vindo ao sistema");
        Console.WriteLine("Aqui você vai cadastrar os produtos da sua loja");
        Console.WriteLine("E no final vamos te dar um resumo do que foi cadastrado");
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
            if (resposta < 0 || resposta > 4)
            {
                Console.WriteLine("Está não e uma opção");
                continue;
            }

                
            return(resposta);
        }
    }


    static string LerRespostaCadastro()
    {
        while (true)
        {
            Console.WriteLine("Deseja criar um novo produto? S / N ");
            string resposta = (Console.ReadLine() ?? "").Trim().ToUpper();

            if (resposta == "S" || resposta == "N")
            {
                return resposta;
            }
            Console.WriteLine("Escreve S ou N");
        }
    }


    static void MostrarProdutos(Produto produto)
    {
        Console.WriteLine($"Código: {produto.Codigo}");
        Console.WriteLine($"Nome: {produto.Nome}");
        Console.WriteLine($"Preço: {produto.Preco:F2}");
        Console.WriteLine($"Estoque: {produto.Estoque}");
    }

    static decimal ValorProduto(Produto produto)
    {
        return produto.Preco * produto.Estoque;
    }

    static decimal ValorTotalEstoque(List<Produto> produtos)
    {
        decimal valorTotalEstoque = 0;

        foreach (Produto item in produtos)
        {
            valorTotalEstoque += ValorProduto(item);
        }
        return valorTotalEstoque;
    }

    static Produto? BuscarProduto (List<Produto> produtos, int codigo)
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


    //Método principal ----------------------------------------
    static void Main(string[] args)
    {
        Saudacoes();

        List<Produto> produtos = new List<Produto>();

        int opcao = Opcoes();


    while(opcao != 0) {

            //Loop de cadastro
            if (opcao == 1)
            {
                int codigo; //Declara a variavel fora do loop para poder usar depois
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
                string nome; //Declara a variavel fora do loop para poder usar depois
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
                decimal preco; //Declara a variavel fora do loop para poder usar depois
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

                int estoque; //Declara a variavel fora do loop para poder usar depois
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

                produtos.Add(produto);

                Console.WriteLine("Criado com sucesso!");

            }

            // busca de produto
            else if (opcao == 2)
            {
                if (produtos.Count <= 0)
                {
                    Console.WriteLine("Não tem produtos para buscar");

                }
                else
                {

                    Console.WriteLine("Digite o código do produto que deseja buscar: ");

                    bool codigoValido = int.TryParse(
                        Console.ReadLine(),
                        out int codigoBusca
                    );

                    if (codigoValido)
                    {
                        Produto? encontrado = BuscarProduto(produtos, codigoBusca);

                        if (encontrado != null)
                        {
                            Console.WriteLine("Produto encontrado!");
                            MostrarProdutos(encontrado);
                        }
                        else
                        {
                            Console.WriteLine("Produto não encontrado");

                        }
                    }
                    else
                    {
                        Console.WriteLine("Código invalido");
                    }
                }
            }

            //listar produtos
            else if (opcao == 3)
            {
                if (produtos.Count <= 0)
                {
                    Console.WriteLine("Não tem produtos");
                }
                else
                {
                    Console.WriteLine("Dados dos seus produtos:");

                    foreach (Produto item in produtos)
                    {
                        MostrarProdutos(item);
                    }

                }
            }

            //Resumo do cadastro
            else if (opcao == 4)
            {
                if (produtos.Count <= 0)
                {
                    Console.WriteLine("Não tem produtos cadastrados");
                }
                else
                {
                    Console.WriteLine($"O total de produtos cadastrados foi {produtos.Count}");

                    int produtosComEstoqueBaixo = produtos.Count(p => p.Estoque > 0 && p.Estoque < 5);

                    Console.WriteLine($"Produtos com estoque baixo: {produtosComEstoqueBaixo}! ");

                    decimal valorTotalEstoque = ValorTotalEstoque(produtos);

                    Console.WriteLine//Retorna o valor total do estoque
                    (
                        $"O valor total do estoque é de R$ {valorTotalEstoque:F2}"
                    );

                    foreach (Produto item in produtos)
                    {
                        MostrarProdutos(item);
                    }
                }
            }
        
            //retorna menu 
            opcao = Opcoes();
        }
    }
}

