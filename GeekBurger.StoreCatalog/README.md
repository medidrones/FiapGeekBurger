# FIAP - 21NET - MBA em Arquitetura e Desenvolvimento na Plataforma .NET
# GeekBurger.StoreCatalog

### Alunos
* 343223 - Jorge Antonio Gonçalves Medina
* 342827 - Henrique Lopes Mendonça
* 343115 - Henry Turner Lizidatti Rosolini

Trabalho em grupo da disciplina de Arquitetura de Integração e Microservices.

## Escopo
Este projeto trata-se de um micro serviço de classificação de produtos baseados nas áreas de preparo disponíveis e, principalmente, nas restrições alimentares do cliente.

### Arquitetura
* Contract - Armazena todos os objetos que envolvem o contrato da aplicação e seus funcionamentos internos. A camada contrato é compartilhada por todoas as outras.
* Endpoints - É o projeto que possui os endpoits e todo resto das configurações como Swagger e configurações do projeto. 
* Core - Camada responsável pelo processamento e calculo dos dados. A regra de negócio do domínio fica centralizada na camada Core.
* Infra - A camada Infra é dividida em duas outras camadas, são elas a Services e Repositories
* **Services** - Responsável por fazer todas as requisições dos outros serviços - Testas os métodos de forma integrada
* **Repositories** - Armazena os dados recolhidos no banco de dados
* Tests - Armazena os testes de Integração e de Unidade
* **IntegrationsTests** - Testas os métodos de forma integrada
* **UnitTests** - Testas os métodos de forma isolada

### Estrutura dos responses
Todos os responses estão padronizados dentro do objeto OperationResult. Este objeto foi padronizado para facilitar a serialização e deserialização dos serviços que consomem o Store Catalog. A estrutura do objeto é composta de:
* Date - Data e hora que o objeto foi criado no servidor
* Success - Booleano que identifica se a requisição foi feita com sucesso ou não
* Message - Mensagem da exception caso a requisição tenha falhado
* Data - Objeto genérico retornado com a requisição
