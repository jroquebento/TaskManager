# TaskManager

API REST para gerenciamento de tarefas, desenvolvida com **C# e .NET 10**.

O projeto foi desenvolvido com foco em aprendizado prático e construção de portfólio, aplicando conceitos e práticas comuns no desenvolvimento de aplicações backend, desde a modelagem do domínio até testes automatizados, containerização e CI/CD.

## Objetivo

O objetivo do projeto é construir uma aplicação completa de gerenciamento de tarefas, aplicando diferentes etapas do desenvolvimento de uma API:

* Modelagem do domínio
* Desenvolvimento de uma API REST
* Persistência de dados
* Validação e regras de negócio
* Tratamento de exceções
* Autenticação e autorização
* Testes automatizados
* Documentação da API
* Controle de versão
* Containerização
* Integração e entrega contínuas (CI/CD)

Os recursos foram implementados de forma incremental, permitindo aplicar na prática os conceitos estudados durante o desenvolvimento.

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
* [x] Docker Compose
* [x] CI/CD com GitHub Actions
* [x] Publicação da imagem Docker no GitHub Container Registry (GHCR)

## Regras de negócio

As principais regras implementadas incluem:

* O título da tarefa é obrigatório.
* Toda tarefa possui um status.
* Uma tarefa inicia no status `Pending`.
* Uma tarefa só pode ser iniciada quando estiver em `Pending`.
* Uma tarefa só pode ser concluída quando estiver em `InProgress`.
* Cada usuário pode acessar apenas suas próprias tarefas.
* A data de vencimento não pode ser anterior à data de criação.
* Transições de estado inválidas são rejeitadas pela aplicação.

O ciclo de vida da tarefa segue as seguintes transições:

```text
Pending → InProgress → Completed
```

## Tecnologias

As principais tecnologias e ferramentas utilizadas são:

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
* **GitHub Actions**
* **GitHub Container Registry (GHCR)**

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

### Banco de desenvolvimento com Docker

O projeto utiliza **Docker Compose** para executar a API juntamente com uma instância do SQL Server.

A configuração está definida no arquivo:

```text
docker-compose.yml
```

O Compose utiliza dois serviços:

* **api** — aplicação ASP.NET Core.
* **sqlserver** — banco de dados SQL Server.

A API se conecta ao SQL Server através do nome do serviço `sqlserver` dentro da rede do Docker.

As migrations existentes são aplicadas automaticamente pela aplicação durante a inicialização.

## Como executar o projeto

### Pré-requisitos

Para executar o projeto utilizando Docker, é necessário ter instalado:

* Docker Desktop

Não é necessário instalar o .NET SDK ou SQL Server separadamente para executar a aplicação através do Docker Compose.

### Executar com Docker Compose

Clone o repositório e acesse a pasta do projeto:

```bash
git clone https://github.com/jroquebento/TaskManager.git
cd TaskManager
```

Em seguida, execute:

```bash
docker compose up
```

O Docker Compose irá:

1. Baixar a imagem da API publicada no GitHub Container Registry.
2. Criar o container do SQL Server.
3. Aguardar o SQL Server ficar disponível.
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

Os dados do SQL Server são armazenados em um volume Docker e permanecem disponíveis após a remoção dos containers.

Para remover também o volume e apagar os dados do banco:

```bash
docker compose down -v
```

## Migrations

As migrations são gerenciadas pelo **Entity Framework Core**.

Ao executar a aplicação através do Docker Compose, as migrations existentes são aplicadas automaticamente durante a inicialização da API.

Durante o desenvolvimento, novas migrations podem ser criadas utilizando o .NET SDK e a ferramenta `dotnet-ef`.

Caso a ferramenta ainda não esteja instalada:

```bash
dotnet tool install --global dotnet-ef
```

Exemplo:

```bash
dotnet ef migrations add NomeDaMigration --project src/TaskManager.Infrastructure --startup-project src/TaskManager.Api
```

## Testes

O projeto possui testes unitários e testes de integração.

Para executar toda a suíte de testes:

```bash
dotnet test
```

Os testes de integração utilizam um banco SQL Server separado do banco de desenvolvimento para evitar que os dados dos testes interfiram no ambiente local.

## Autenticação

A API utiliza **JWT (JSON Web Token)** para autenticação.

Para acessar os endpoints protegidos:

1. Cadastre um usuário através da API.
2. Realize o login.
3. Obtenha o token JWT retornado pela autenticação.
4. Envie o token no header `Authorization`:

```text
Authorization: Bearer <token>
```

A autenticação pode ser testada através do Swagger:

```text
http://localhost:8080/swagger
```

## CI/CD

O projeto utiliza **GitHub Actions** para automatizar a validação e publicação da aplicação.

O pipeline executa:

```text
Restore
   ↓
Build
   ↓
Testes
   ↓
Build da imagem Docker
   ↓
Push da imagem para GHCR
```

O workflow é executado em **pushes para a `develop` e branches de feature**, além de **Pull Requests direcionados à `develop`**.

A imagem Docker é publicada no **GitHub Container Registry (GHCR)**:

```text
ghcr.io/jroquebento/taskmanager:latest
```

Dessa forma, o Docker Compose pode utilizar diretamente a imagem publicada no registry, sem precisar construir a imagem localmente.

## Status do projeto

**Concluído para fins de portfólio.**

O projeto possui uma implementação funcional de gerenciamento de tarefas, cadastro e autenticação de usuários, autorização, persistência de dados, regras de domínio, testes automatizados, tratamento global de exceções, documentação da API, containerização e pipeline de CI/CD.

## Objetivo de aprendizado

Este projeto faz parte do processo de desenvolvimento de habilidades em **.NET e desenvolvimento backend**, buscando transformar conhecimentos teóricos em experiência prática por meio da construção de uma aplicação completa.

O objetivo não foi apenas desenvolver uma API funcional, mas compreender na prática aspectos como:

* Organização e separação de responsabilidades
* Modelagem e regras de domínio
* Persistência de dados
* Autenticação e autorização
* Testes automatizados
* Versionamento com Git
* Containerização
* CI/CD
* Documentação de APIs

O projeto também serviu como oportunidade para praticar a resolução de problemas encontrados durante o desenvolvimento e compreender o ciclo de desenvolvimento de uma aplicação backend de ponta a ponta.
