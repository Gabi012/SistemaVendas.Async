# Sistema Vendas - Estudo de Processamento Assíncrono com .NET 8

## 📌 Objetivo

Este projeto foi desenvolvido com o objetivo de estudar conceitos de processamento assíncrono e arquiteturas orientadas a eventos utilizando **.NET 8**, **RabbitMQ**, **Background Services** e **SignalR**, simulando cenários encontrados em aplicações corporativas.

O foco não está na complexidade das regras de negócio, mas na arquitetura responsável por executar tarefas pesadas de forma desacoplada e escalável.

---

# Tecnologias utilizadas

* ASP.NET Core MVC (.NET 8)
* Entity Framework Core
* SQL Server
* RabbitMQ
* BackgroundService
* SignalR
* Injeção de Dependência
* Programação Assíncrona (async/await)

---

# Objetivos de aprendizado

Durante o desenvolvimento do projeto foram estudados os seguintes conceitos:

* Programação assíncrona
* async / await
* Tasks
* Thread Pool
* Background Services
* Mensageria
* RabbitMQ
* Comunicação desacoplada
* SignalR
* Persistência de notificações
* Processamento em segundo plano
* Arquitetura em camadas
* Dependency Injection

---

# Cenário implementado

O usuário solicita a geração de um relatório pelo sistema.

Em vez de gerar o relatório durante a requisição HTTP, a aplicação publica uma mensagem no RabbitMQ.

Um Worker em Background consome essa mensagem, realiza todo o processamento pesado e atualiza o banco de dados.

Ao finalizar o processamento:

* atualiza o status do relatório;
* salva o caminho do arquivo gerado;
* cria uma notificação persistida no banco;
* envia uma notificação em tempo real utilizando SignalR.

Tudo isso acontece sem bloquear a interface do usuário.

---

# Fluxo do sistema

```text
Usuário

↓

MVC

↓

RabbitMQ Producer

↓

Fila de Relatórios

↓

Background Service

↓

Processamento do Relatório

↓

Atualiza Banco

↓

Cria Notificação

↓

SignalR

↓

Usuário recebe aviso em tempo real

↓

Download do relatório
```

---

# Funcionalidades implementadas

## Cadastro de Vendas

* CRUD simples de vendas
* Persistência utilizando Entity Framework Core

---

## Solicitação de Relatórios

* Solicitação via MVC
* Persistência da solicitação
* Status do processamento

---

## Processamento Assíncrono

* RabbitMQ Producer
* RabbitMQ Consumer
* Background Service
* Simulação de processamento pesado

---

## Geração de Arquivos

* Geração de arquivos CSV
* Armazenamento local
* Download pela aplicação

---

## Notificações

* Persistência em banco de dados
* Contador de notificações
* Marcar como lida
* Atualização em tempo real utilizando SignalR

---

## Cancelamento

O usuário pode cancelar um relatório enquanto ele ainda estiver com status **Pendente**.

O Worker verifica o status antes de iniciar o processamento, evitando executar tarefas canceladas.

---

# Estrutura do projeto

```text
SistemaVendas

├── SistemaVendas.Domain
│
├── SistemaVendas.Infrastructure
│
├── SistemaVendas.Web
│
└── SistemaVendas.Worker
```

---

# Conceitos estudados

## Programação Assíncrona

Utilização de async/await para evitar bloqueio de requisições HTTP.

---

## RabbitMQ

Estudo de:

* Producers
* Consumers
* Filas
* Acknowledgement
* Publicação de mensagens

---

## BackgroundService

Execução de tarefas demoradas em segundo plano sem bloquear a aplicação MVC.

---

## SignalR

Comunicação em tempo real entre servidor e navegador para atualização automática de notificações.

---

## Entity Framework Core

Persistência dos dados utilizando DbContext, Migrations e SQL Server.

---

## Arquitetura em Camadas

Separação entre:

* Web
* Domain
* Infrastructure
* Worker

---

# O que foi aprendido

Ao longo do projeto foram explorados conceitos importantes utilizados em aplicações de grande porte, como:

* desacoplamento entre interface e processamento;
* filas para execução assíncrona;
* comunicação entre aplicações utilizando RabbitMQ;
* persistência de estados de processamento;
* notificações em tempo real;
* execução de tarefas em Background Services;
* boas práticas de organização em projetos ASP.NET Core.

---

# Próximas evoluções

As próximas etapas planejadas para o projeto incluem:

## Infraestrutura

* RabbitMQ Connection Manager
* Reutilização de Connections
* Gerenciamento de Channels

---

## RabbitMQ Avançado

* Dead Letter Queue (DLQ)
* Retry automático
* Exchanges
* Routing Keys
* Prefetch Count
* Quality of Service (QoS)

---

## Processamento

* Pipeline de processamento
* Separação de responsabilidades
* Processamento paralelo
* Controle de concorrência
* CancellationToken

---

## Monitoramento

* Dashboard em tempo real
* Estatísticas de processamento
* Tempo médio de execução
* Quantidade de itens em fila

---

## Observabilidade

* ILogger
* Logs estruturados
* Correlação de requisições

---

## Armazenamento

* Abstração para armazenamento de arquivos
* Possibilidade de integração futura com Amazon S3

---

# Objetivo final

Construir uma aplicação simples do ponto de vista de negócio, porém rica em conceitos arquiteturais modernos utilizados em sistemas distribuídos e aplicações de alta escalabilidade.

O projeto servirá como laboratório para estudo de:

* Programação Assíncrona
* RabbitMQ
* Background Services
* SignalR
* Escalabilidade
* Processamento distribuído
* Arquitetura de Software
* Boas práticas em .NET
