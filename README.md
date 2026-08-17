# TaskManager

## Sobre o projeto

O **TaskManager** é uma API REST para gerenciamento de tarefas, desenvolvida com **C# e .NET**.

O projeto está sendo desenvolvido com foco em aprendizado prático e construção de portfólio, aplicando conceitos e práticas comuns no desenvolvimento de aplicações backend profissionais.

## Objetivo

O objetivo do projeto é desenvolver uma aplicação completa de gerenciamento de tarefas, passando pelas principais etapas do desenvolvimento de uma API:

* Modelagem do domínio;
* Desenvolvimento de uma API REST;
* Persistência de dados;
* Validação de informações;
* Autenticação e autorização;
* Testes automatizados;
* Containerização;
* Integração e entrega contínuas (CI/CD).

O projeto será desenvolvido de forma incremental, permitindo aplicar os conceitos estudados ao longo do desenvolvimento.

## Funcionalidades

As funcionalidades serão implementadas gradualmente.

### Usuários

* [ ] Cadastro de usuário
* [ ] Autenticação
* [ ] Autorização

### Tarefas

* [ ] Criar tarefa
* [ ] Listar tarefas
* [ ] Consultar tarefa
* [ ] Atualizar tarefa
* [ ] Concluir tarefa
* [ ] Excluir tarefa

### Qualidade e infraestrutura

* [ ] Validações
* [ ] Tratamento global de exceções
* [ ] Testes unitários
* [ ] Testes de integração
* [ ] Docker
* [ ] CI/CD

## Regras de negócio

As regras iniciais do sistema são:

* O e-mail do usuário deve ser único.
* Nome, e-mail e senha são obrigatórios no cadastro.
* Toda tarefa pertence a um usuário.
* O título da tarefa é obrigatório.
* Toda tarefa possui um status.
* Toda tarefa possui uma prioridade.
* Um usuário só pode acessar suas próprias tarefas.
* Um usuário só pode alterar ou excluir suas próprias tarefas.

Novas regras poderão ser adicionadas conforme o projeto evoluir.

## Tecnologias

As principais tecnologias e ferramentas utilizadas no projeto são:

* **C#**
* **.NET**
* **ASP.NET Core**
* **Entity Framework Core**
* **SQL Server**
* **Git**
* **GitHub**

Outras tecnologias serão adicionadas conforme novas funcionalidades forem implementadas.

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

Responsável pelos casos de uso e pela coordenação das operações da aplicação.

**TaskManager.Domain**

Contém as entidades e regras relacionadas ao domínio do sistema.

**TaskManager.Infrastructure**

Responsável pelas implementações relacionadas à infraestrutura, como persistência de dados e acesso ao banco de dados.

**TaskManager.UnitTests**

Contém os testes unitários das partes isoladas da aplicação.

**TaskManager.IntegrationTests**

Contém os testes de integração, verificando o funcionamento conjunto de diferentes componentes da aplicação.

## Status do projeto

🚧 **Em desenvolvimento**

O projeto está sendo construído incrementalmente, desde a estrutura inicial até a implementação das funcionalidades e recursos de infraestrutura.

## Objetivo de aprendizado

Este projeto faz parte do meu processo de desenvolvimento de habilidades em **.NET e desenvolvimento backend**, buscando transformar conhecimentos teóricos em experiência prática por meio da construção de uma aplicação completa.

O objetivo não é apenas desenvolver uma API funcional, mas compreender as decisões de arquitetura, organização de código, testes, versionamento e práticas utilizadas no desenvolvimento de software.
