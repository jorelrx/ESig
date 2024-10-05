# SalarioWeb

SalarioWeb é um sistema web desenvolvido em ASP.NET Core para a gestão de pessoas, cargos e salários. O sistema permite a importação de dados via planilhas Excel, cálculo de salários, geração de relatórios de salários, além de uma interface web para cadastro, consulta e exclusão de pessoas e cargos.

## Funcionalidades

### 1. Importação de Dados via Planilha Excel
O sistema possui um serviço de importação de dados (pessoas e cargos) a partir de um arquivo Excel. A importação é realizada no momento da inicialização do sistema, sem a necessidade de intervenção do usuário. Caso um registro já exista no banco de dados, o sistema o ignora, caso contrário, o novo registro é adicionado.

### 2. Cálculo de Salários
O sistema conta com uma *Stored Procedure* no banco de dados que realiza o cálculo de salários. Para cada cálculo, o valor do salário é armazenado junto com a data do cálculo. Caso o cálculo já tenha sido realizado no mesmo dia, ele atualiza o valor existente.

### 3. Relatório Geral de Salários
O sistema permite visualizar um *Relatório Geral de Salários*, que apresenta informações como nome do funcionário, salário inicial, salário atual e a data do último cálculo. O relatório é gerado com base nos dados armazenados no banco de dados e permite acessar os detalhes do funcionário e seu histórico de salários.

### 4. Cadastro e Gestão de Pessoas e Cargos
A aplicação oferece uma interface web para realizar as seguintes ações:
- *Cadastro de Pessoas*: Formulário de cadastro com validação de dados como nome, e-mail, telefone e cargo. O nome e o usuário devem conter no mínimo 4 caracteres.
- *Edição e Exclusão de Pessoas*: Opções para editar e excluir registros de pessoas.
- *Cadastro de Cargos*: Inserção de novos cargos com definição de nome e salário base.
- *Edição e Exclusão de Cargos*: Opções para editar e excluir registros de cargos.

## Tecnologias Utilizadas
- *ASP.NET Core 8.0*: Framework principal para desenvolvimento da aplicação web.
- *Entity Framework Core*: ORM para interação com o banco de dados SQL Server.
- *SQL Server*: Banco de dados utilizado para armazenar as informações da aplicação.
- *FluentValidation*: Biblioteca para validação dos dados das entidades.
- *AutoMapper*: Biblioteca para mapeamento de objetos (DTOs e entidades do banco).
- *OfficeOpenXml*: Biblioteca para leitura e manipulação de arquivos Excel.
- *Bootstrap*: Framework para estilização e design responsivo.

## Estrutura do Projeto

- Controllers: Controladores para lidar com as requisições HTTP.
  - PessoaController: Gerencia as operações relacionadas a Pessoas (CRUD e Relatórios).
  - CargoController: Gerencia as operações relacionadas a Cargos.
  
- Services: Contém a lógica de negócio.
  - PessoaService: Implementa as funcionalidades relacionadas a Pessoas.
  - CargoService: Implementa as funcionalidades relacionadas a Cargos.
  - ExcelImportService: Implementa a lógica de importação dos dados das planilhas Excel.
  
- Repositories: Interface de acesso ao banco de dados.
  - IPessoaRepository: Define as operações de dados para a entidade Pessoa.
  - ICargoRepository: Define as operações de dados para a entidade Cargo.

- Data: Configuração do contexto do banco de dados (EF Core).
  - SalarioContext: Contexto principal da aplicação que gerencia as entidades e realiza as migrações.

- DTOs: Data Transfer Objects, utilizados para transportar dados entre as camadas do sistema.
  - CreatePessoaDTO, UpdatePessoaDTO, PessoaRelatorioDTO: DTOs específicos para diferentes operações de Pessoa.

- Validators: Contém a validação de dados das entidades.
  - CreatePessoaDTOValidator: Regras de validação para o cadastro de uma nova Pessoa.

- Views: Arquivos Razor para renderização da interface do usuário.
  - *Pessoa*: Exibe as páginas de CRUD de Pessoas.
  - *Cargo*: Exibe as páginas de CRUD de Cargos.
  - *Relatórios*: Exibe os relatórios de salários e detalhes.

## Configuração do Banco de Dados

A aplicação utiliza o SQL Server como banco de dados. É necessário configurar a conexão no arquivo appsettings.json:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=SalarioWeb;Trusted_Connection=True;"
}

# Passos para Executar o Projeto

1. Clonar o Repositório

Clone o projeto em sua máquina local usando o comando Git:

git clone https://github.com/seuusuario/salario-web.git

2. Configurar a Conexão com o Banco de Dados

No arquivo appsettings.json, configure a connection string para se conectar à sua instância local do SQL Server:

"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=SalarioWeb;Trusted_Connection=True;"
}

Substitua SEU_SERVIDOR pelo nome ou IP do seu servidor SQL.

Se estiver usando SQL Server Express localmente, a connection string pode ser algo como:


"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SalarioWeb;Trusted_Connection=True;"

3. Instalar Dependências

Antes de executar o projeto, restaure todas as dependências. Se estiver usando Visual Studio ou Visual Studio Code, abra o terminal no diretório do projeto e execute o seguinte comando:

dotnet restore

4. Executar Migrações para o Banco de Dados

Execute as migrações para garantir que o banco de dados esteja atualizado. Você pode rodar este comando no terminal:

dotnet ef database update

Este comando cria todas as tabelas e estruturas necessárias no banco de dados.

5. Configurar o Arquivo Excel para Importação de Dados

Coloque o arquivo dados.xlsx dentro da pasta Excels no diretório raiz do projeto. Certifique-se de que o arquivo tenha as abas Pessoa e Cargo com a estrutura descrita no README anterior.

6. Executar o Projeto

Após a configuração do banco de dados e restauração das dependências, você pode executar o projeto com o seguinte comando:

dotnet run

Ou, se estiver utilizando o Visual Studio, apenas pressione F5 ou clique em "Run" para iniciar o projeto.