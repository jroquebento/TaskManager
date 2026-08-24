# TaskManager

## Sobre o projeto

O **TaskManager** é uma API REST para gerenciamento de tarefas, desenvolvida com **C# e .NET**.

O projeto está sendo desenvolvido com foco em aprendizado prático e construção de portfólio, aplicando conceitos e práticas comuns no desenvolvimento de aplicações backend.

## Objetivo

O objetivo do projeto é desenvolver uma aplicação completa de gerenciamento de tarefas, passando de forma incremental pelas principais etapas do desenvolvimento de uma API:

* Modelagem do domínio
* Desenvolvimento de uma API REST
* Persistência de dados
* Validação de informações
* Tratamento de exceções
* Testes automatizados
* Documentação da API
* Controle de versão
* Containerização
* Integração e entrega contínuas (CI/CD)
* Autenticação e autorização

Os recursos são implementados gradualmente, permitindo aplicar os conceitos estudados ao longo do desenvolvimento.

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

* [ ] Cadastro de usuário
* [ ] Autenticação
* [ ] Autorização

### Qualidade e infraestrutura

* [x] Validação de dados e regras de domínio
* [x] Tratamento global de exceções
* [x] Testes unitários
* [x] Testes de integração
* [x] Documentação com Swagger/OpenAPI
* [x] Entity Framework Core Migrations
* [ ] Docker
* [ ] CI/CD

## Regras de negócio

As regras atualmente implementadas incluem:

* O título da tarefa é obrigatório.
* Toda tarefa possui um status.
* Uma tarefa inicia no status `Pending`.
* Uma tarefa só pode ser iniciada quando estiver em `Pending`.
* Uma tarefa só pode ser concluída quando estiver em `InProgress`.
* O ciclo de vida da tarefa segue as transições:

```text
Pending → InProgress → Completed
```

* Transições de estado inválidas são rejeitadas pela aplicação.

Novas regras poderão ser adicionadas conforme o projeto evoluir.

## Tecnologias

As principais tecnologias e ferramentas utilizadas no projeto são:

* **C#**
* **.NET**
* **ASP.NET Core**
* **Entity Framework Core**
* **SQL Server**
* **Swagger/OpenAPI**
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

Responsável pela exposição da API REST, recebimento das requisições HTTP e retorno das respostas.

**TaskManager.Application**

Responsável pelos casos de uso, DTOs, mapeamentos e coordenação das operações da aplicação.

**TaskManager.Domain**

Contém as entidades, enums, exceções e regras relacionadas ao domínio do sistema.

**TaskManager.Infrastructure**

Responsável pelas implementações relacionadas à infraestrutura, incluindo persistência, Entity Framework Core, DbContext e acesso ao banco de dados.

**TaskManager.UnitTests**

Contém testes unitários das partes isoladas da aplicação.

**TaskManager.IntegrationTests**

Contém testes de integração que verificam o funcionamento conjunto da API, Application, Infrastructure, Entity Framework Core e banco de dados.

## Banco de dados

O projeto utiliza **SQL Server** com **Entity Framework Core** para persistência dos dados.

O schema do banco é versionado por meio de **migrations do Entity Framework Core**.

A migration inicial atualmente existente é:

```text
20260819123427_InitialCreate
```

### Configuração da conexão

A aplicação utiliza a connection string configurada em:

```text
src/TaskManager.Api/appsettings.json
```

A connection string deve apontar para uma instância do SQL Server disponível no ambiente de execução.

### Aplicando as migrations

Com o SQL Server configurado, execute na raiz da solução:

```bash
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.Api
```

Esse comando cria ou atualiza o banco de dados de acordo com as migrations existentes.

## Como executar o projeto

### Pré-requisitos

* .NET SDK 10
* SQL Server ou SQL Server LocalDB
* Git
* `dotnet-ef`

Caso a ferramenta `dotnet-ef` ainda não esteja instalada:

```bash
dotnet tool install --global dotnet-ef
```

### Restaurar dependências

Na raiz da solução:

```bash
dotnet restore
```

### Aplicar as migrations

```bash
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.Api
```

### Executar a API

Para executar utilizando o perfil HTTPS configurado no projeto:

```bash
dotnet run --launch-profile https --project src/TaskManager.Api
```

Após iniciar a aplicação, a URL HTTPS será exibida no terminal.

A documentação da API pode ser acessada adicionando `/swagger` à URL exibida.

Exemplo:

```text
https://localhost:7149/swagger
```

A porta pode variar conforme a configuração do ambiente.

## Testes

O projeto possui testes unitários e testes de integração.

Para executar toda a suíte:

```bash
dotnet test
```

Os testes de integração utilizam um banco SQL Server separado do banco de desenvolvimento para evitar que os dados dos testes interfiram no ambiente local.

## Status do projeto

🚧 **Em desenvolvimento**

O projeto está sendo construído incrementalmente, desde a estrutura inicial até a implementação de funcionalidades e recursos de infraestrutura.

As funcionalidades ainda planejadas incluem autenticação, autorização, Docker e CI/CD.

## Objetivo de aprendizado

Este projeto faz parte do processo de desenvolvimento de habilidades em **.NET e desenvolvimento backend**, buscando transformar conhecimentos teóricos em experiência prática por meio da construção de uma aplicação completa.

O objetivo não é apenas desenvolver uma API funcional, mas compreender as decisões de arquitetura, organização de código, persistência, testes, versionamento, documentação e práticas utilizadas no desenvolvimento de software.