# TaskManager

## Sobre o projeto

O **TaskManager** é uma API REST para gerenciamento de tarefas, desenvolvida com **C# e .NET**.

O projeto foi desenvolvido com foco em aprendizado prático e construção de portfólio, aplicando conceitos e práticas comuns no desenvolvimento de aplicações backend.

## Objetivo

O objetivo do projeto é desenvolver uma aplicação completa de gerenciamento de tarefas, passando pelas principais etapas do desenvolvimento de uma API:

* Modelagem do domínio
* Desenvolvimento de uma API REST
* Persistência de dados
* Validação de informações
* Tratamento de exceções
* Testes automatizados
* Documentação da API
* Controle de versão
* Containerização
* Autenticação e autorização
* Integração e entrega contínuas (CI/CD)

Os recursos foram implementados de forma incremental, permitindo aplicar os conceitos estudados ao longo do desenvolvimento.

## Funcionalidades

### Tarefas

* [x] Criar tarefa
* [x] Listar tarefas
* [x] Consultar tarefa por ID
* [x] Atualizar tarefa
* [x] Excluir tarefa
* [x] Iniciar tarefa
* [x] Concluir tarefa

### Usuários

* [x] Cadastro de usuário
* [x] Autenticação
* [x] Autorização

### Qualidade e infraestrutura

* [x] Validação de dados e regras de domínio
* [x] Tratamento global de exceções
* [x] Testes unitários
* [x] Testes de integração
* [x] Documentação com Swagger/OpenAPI
* [x] Entity Framework Core Migrations
* [x] Docker
* [ ] CI/CD

## Regras de negócio

As regras atualmente implementadas incluem:

* O título da tarefa é obrigatório.
* Toda tarefa possui um status.
* Uma tarefa inicia no status `Pending`.
* Uma tarefa só pode ser iniciada quando estiver em `Pending`.
* Uma tarefa só pode ser concluída quando estiver em `InProgress`.
* Cada usuário pode acessar apenas suas próprias tarefas.
* O ciclo de vida da tarefa segue as transições:

```text
Pending → InProgress → Completed
```

* Transições de estado inválidas são rejeitadas pela aplicação.

## Tecnologias

As principais tecnologias e ferramentas utilizadas no projeto são:

* **C#**
* **.NET 10**
* **ASP.NET Core**
* **Entity Framework Core**
* **SQL Server**
* **Docker**
* **Docker Compose**
* **Swagger/OpenAPI**
* **JWT**
* **Argon2**
* **xUnit**
* **Moq**
* **Git**
* **GitHub**

## Arquitetura

O projeto utiliza uma organização baseada em separação de responsabilidades, inspirada em conceitos de **DDD (Domain-Driven Design)** e **Clean Architecture**.

### Estrutura

```text
TaskManager
│
├── src
│   ├── TaskManager.Api
│   ├── TaskManager.Application
│   ├── TaskManager.Domain
│   └── TaskManager.Infrastructure
│
└── tests
    ├── TaskManager.UnitTests
    └── TaskManager.IntegrationTests
```

### Camadas

**TaskManager.Api**

Responsável pela exposição da API REST, recebimento das requisições HTTP, autenticação e retorno das respostas.

**TaskManager.Application**

Responsável pelos casos de uso, DTOs, interfaces e coordenação das operações da aplicação.

**TaskManager.Domain**

Contém as entidades, enums, exceções e regras relacionadas ao domínio do sistema.

**TaskManager.Infrastructure**

Responsável pelas implementações relacionadas à infraestrutura, incluindo persistência, Entity Framework Core, DbContext, repositórios e recursos de segurança.

**TaskManager.UnitTests**

Contém testes unitários das partes isoladas da aplicação.

**TaskManager.IntegrationTests**

Contém testes de integração que verificam o funcionamento conjunto da API, Application, Infrastructure, Entity Framework Core e banco de dados.

## Banco de dados

O projeto utiliza **SQL Server** com **Entity Framework Core** para persistência dos dados.

O schema do banco é versionado por meio de **migrations do Entity Framework Core**.

As migrations atualmente existentes no projeto são:

```text
20260819123427_InitialCreate
20260825151519_AddUser
```
Elas representam a evolução do schema do banco de dados ao longo do desenvolvimento da aplicação.

### Banco de desenvolvimento com Docker

A aplicação utiliza **Docker Compose** para executar a API juntamente com uma instância do SQL Server.

A configuração está definida no arquivo:

```text
docker-compose.yml
```

O Compose cria dois serviços:

* **api** — aplicação ASP.NET Core.
* **sqlserver** — banco de dados SQL Server.

A API se conecta ao SQL Server através do nome do serviço `sqlserver` dentro da rede do Docker.

As migrations são aplicadas automaticamente pela aplicação durante a inicialização.

## Como executar o projeto

### Pré-requisitos

Para executar o projeto utilizando Docker, é necessário ter instalado:

* Docker
* Docker Compose

Não é necessário instalar o .NET SDK ou SQL Server separadamente para executar a aplicação através do Docker.

### Executar com Docker Compose

Na raiz do projeto, execute:

```bash
docker compose up --build
```

O Docker Compose irá:

1. Criar o container do SQL Server.
2. Aguardar o SQL Server ficar disponível.
3. Construir a imagem da API.
4. Iniciar a API.
5. Aplicar as migrations do Entity Framework Core automaticamente.

A API ficará disponível em:

```text
http://localhost:8080
```

A documentação da API pode ser acessada em:

```text
http://localhost:8080/swagger
```

### Parar a aplicação

Para parar os containers:

```bash
docker compose down
```

Os dados do SQL Server são armazenados no volume Docker `sqlserver_data` e permanecem disponíveis após a remoção dos containers.

Para remover também o volume e apagar os dados do banco:

```bash
docker compose down -v
```

## Migrations

As migrations são gerenciadas pelo **Entity Framework Core**.

Ao executar a aplicação através do Docker Compose, as migrations existentes são aplicadas automaticamente durante a inicialização da API.

Para criar novas migrations durante o desenvolvimento, é necessário ter o .NET SDK e a ferramenta `dotnet-ef` instalados.

Caso a ferramenta ainda não esteja instalada:

```bash
dotnet tool install --global dotnet-ef
```

Exemplo para criar uma nova migration:

```bash
dotnet ef migrations add NomeDaMigration --project src/TaskManager.Infrastructure --startup-project src/TaskManager.Api
```

## Testes

O projeto possui testes unitários e testes de integração.

Para executar toda a suíte:

```bash
dotnet test
```

Os testes de integração utilizam um banco SQL Server separado do banco de desenvolvimento para evitar que os dados dos testes interfiram no ambiente local.

## Autenticação

A API utiliza **JWT (JSON Web Token)** para autenticação.

Para acessar os endpoints protegidos:

1. Cadastre um usuário através da API.
2. Realize o login.
3. Utilize o token JWT retornado na autenticação.
4. Envie o token no header `Authorization`:

```text
Authorization: Bearer <token>
```

A autenticação pode ser testada através do Swagger disponível em:

```text
http://localhost:8080/swagger
```

## Status do projeto

🚧 **Concluído para fins de portfólio**

A implementação principal da API foi concluída, incluindo gerenciamento de tarefas, cadastro e autenticação de usuários, autorização, persistência de dados, testes automatizados, tratamento de exceções e execução com Docker.

**CI/CD** permanece como uma possível evolução futura.

## Objetivo de aprendizado

Este projeto faz parte do processo de desenvolvimento de habilidades em **.NET e desenvolvimento backend**, buscando transformar conhecimentos teóricos em experiência prática por meio da construção de uma aplicação completa.

O objetivo não é apenas desenvolver uma API funcional, mas compreender as decisões de arquitetura, organização de código, persistência, autenticação, testes, versionamento, documentação e práticas utilizadas no desenvolvimento de software.
