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

    static void Saudacoes()
    {
        Console.WriteLine("Bem vindo ao cadastro de produtos");
        Console.WriteLine("Aqui você vai cadastrar os produtos da sua loja");
        Console.WriteLine("E no final vamos te dar um resumo do que foi cadastrado");
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
        Console.WriteLine($"O valor total do estoque é de R${valorTotalEstoque:F2}");
        return valorTotalEstoque;
    }

    static Produto? BuscarProduto(List<Produto> produtos, int codigo) {
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

        string resposta = LerRespostaCadastro();

        while (resposta == "S")
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

            decimal valorEstoque = ValorProduto(produto);

            Console.WriteLine("Dados do seu produto:");
            Console.WriteLine($"O código dele é {produto.Codigo}");
            Console.WriteLine($"Ele é um: {produto.Nome}");
            Console.WriteLine($"Preço: {produto.Preco:F2}");
            Console.WriteLine($"Quantidade em estoque: {produto.Estoque}");
            Console.WriteLine($"Em Reais o estoque é de {valorEstoque:F2}");

            if (produto.Estoque == 0)
            {
                Console.WriteLine("O estoque ta zerado");
            }
            else if (produto.Estoque < 5)
            {
                Console.WriteLine("O estoque ta baixo mano");
            }
            else
            {
                Console.WriteLine("Pode vender suave");
            }

            resposta = LerRespostaCadastro();
        }

        Console.WriteLine($"O total de produtos cadastrados foi {produtos.Count}");
        int produtosComEstoqueBaixo = produtos.Count(p => p.Estoque > 0 && p.Estoque < 5);
        Console.WriteLine($"Produtos com estoque baixo: {produtosComEstoqueBaixo}! ");

        decimal valorTotalEstoque = ValorTotalEstoque(produtos);

        Console.WriteLine($"O valor total do estoque é de R${valorTotalEstoque:F2}");

        foreach (Produto item in produtos)
        {
            MostrarProdutos(item);
        }

        if (produtos.Count > 0)
        {
            Console.WriteLine("-------Buscar Produtos--------");
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

                    Console.WriteLine("Produto encontrado:");
                    MostrarProdutos(encontrado);

                }
                else
                {
                    Console.WriteLine("Produto nào encontrado");
                }
            }
            else
            {
                Console.WriteLine("Código inválido. Digite um número válido.");
            }



        }

    }

        
}
