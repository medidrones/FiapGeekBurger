# FIAP - 21NET - MBA em Arquitetura e Desenvolvimento na Plataforma .NET
# GeekBurger - Production

### Alunos
* 343223 - Jorge Antonio Gonçalves Medina
* 342827 - Henrique Lopes Mendonça
* 343115 - Henry Turner Lizidatti Rosolini

Trabalho em grupo da disciplina de Arquitetura de Integração e Microservices.
Microserviço GeekBurger-Production, responsável por:

  - Criação, edição, remoção e recuperação de Production Areas.
  - Listagem de Production Areas disponíveis.
  - Listagem de Production Areas que atendam a determinada restrição.
  - Disponibilização de mensagens com dados das Production Areas ativas no Tópico ProductionAreaChangedTopic.
  - Leitura de mensagens com dados de Orders disponibilizadas no Tópico OrderNew (obs.: tópico não localizado no ServiceBus).
  - Leitura de mensagens com dados de Orders disponibilizadas no Tópico OrderPaid.
  - Disponibilização de mensagens com id das Orders produzidas (o tempo de produção é definido randomicamente).


Segue descrição dos atributos de uma Production Area:

  - id: Guid que representa o campo identificador de uma Production Area.
  - name: Nome da Production Area.
  - status: identifica a disponibilidade de uma Production Area. True, significa Production Area disponível. False, o contrário.
  - restrictions: lista de restrições vinculadas à Production Area.
  

Segue listagem dos métodos disponibilizados na API:

  - /api/productionarea/{productionAreaId} (GET)
    Permite a obtenção dos dados de determinada Production Area, cujo Id equivale ao valor do parâmetro productionAreaId.
  
  - /api/productionarea/{productionAreaId} (PUT)
    Permite a alteração dos dados de determinada Production Area, cujo Id equivale ao valor do parâmetro productionAreaId.
  
  - /api/productionarea/{productionAreaId} (DELETE)
    Permite a remoção de determinada Production Area, cujo Id equivale ao valor do parâmetro productionAreaId.
    
  - /api/productionarea (POST)
    Permite a inclusão de uma nova Production Area.
    
  - /api/production/areas (GET)
    Permite a obtenção de listagem com as Production Areas ativas (atributo status igual a true).
  
  - /api/production/areas/{restrictionName} (GET)
    Permite a obtenção de listagem com as Production Areas que não tenham a restrição especificada no parâmetro restrictionName.


Segue breve descrição das principais classes envolvidas no projeto.

- GeekBurger.Production

  - /Controllers/ProductionAreaController
    Controller responsável pelas operações CRUD de Production Areas.
    
  - /Controllers/ProductionAreasController
    Controller responsável pelas operações de listagem de Production Areas ativas e listagem de Production Areas que não tenham determinada restrição.
    
  - /Extension/ProductionContextExtension
    Classe onde é especificado o método Seed, utilizado para efetuar uma carga inicial de Production Areas.
    
  - /Model/ProductionArea
    Classe que define o Modelo de ProductionArea, a ser utilizado para persistência em base de dados.
    
  - /Model/Restriction
    Classe que define o Modelo de Restriction, a ser utilizado para persistência em base de dados.

  - /Repository/ProductionAreaRepository
    Classe responsável por concentrar as operações relacionadas à base de dados.
    
  - /Service/ProductionAreaChangedService.cs
    Classe responsável por disponibilizar, para cada Production Area alterada, uma mensagem no Tópico ProductionAreaChangedTopic.

  - /Service/NewOrderService
    Classe responsável por: 
      - Efetuar a subscrição no Tópico neworder (obs.: tópico não localizado no ServiceBus).
      - A cada mensagem recebida, simular o tempo de produção (randômico) e, decorrido este tempo, disparar o processo de disponibilização de uma nova mensagem no Tópico OrderFinishedTopic.

  - /Service/PaidOrderService.cs
    Classe responsável por: 
      - Efetuar a subscrição no Tópico orderpaid.
      - A cada mensagem recebida, simular o tempo de produção (randômico) e, decorrido este tempo, disparar o processo de disponibilização de uma nova mensagem no Tópico OrderFinishedTopic.

  - /Service/OrderFinishedService
    Classe responsável por disponibilizar mensagens no Tópico OrderFinishedTopic.
            

- GeekBurger.Production.Contract

  - ProductionAreaChangedMessage
    Classe responsável por abrigar os dados da Mensagem a ser utilizada no Tópico ProductionAreaChangedTopic.

  - OrderFinishedMessage
    Classe responsável por abrigar os dados da Mensagem a ser utilizada no Tópico OrderFinishedTopic.
  
  - ProductionAreaCRUD
    Classe responsável por abrigar os dados a serem utilizados para as operações CRUD de Production Areas.
    
  - ProductionAreaDTO
    Classe responsável por abrigar os dados a serem utilizados para consultas externas (via API) dos dados de Production Areas.
    
    
        