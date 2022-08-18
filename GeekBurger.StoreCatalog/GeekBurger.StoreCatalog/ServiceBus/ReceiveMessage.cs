using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeekBurger.Production.Contract;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Infra.Interfaces;
using GeekBurger.StoreCatalog.Infra.Repositories;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace GeekBurger.StoreCatalog.ServiceBus
{
    /// <summary>
    /// Class Receive Message
    /// </summary>
    public class ReceiveMessage
    {
        private readonly IRepository<ProductionAreas> _repository;

        /// <summary>
        /// Constructor for register task
        /// </summary>
        public ReceiveMessage()
        {
            ReceberMensagemAsync();
            _repository = new Repository<ProductionAreas>(new StoreCatalogDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<StoreCatalogDbContext>()));
        }

        const string ServiceBusConnectionString = "Endpoint=sb://geekburgernet.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Z8l0qsaaDXT7gv5z0lUlmB1dH/ISKypGf3/OVFOIGsU=";
        //const string TopicName = "storecatalog";
        //const string SubscriptionName = "catalog";
        const string TopicName = "productionareachangedtopic";
        const string SubscriptionName = "productionarea";
        static ISubscriptionClient subscriptionClient;

        /// <summary>
        /// Método responsável por se subscrever em um tópico
        /// </summary>
        /// <returns></returns>
        public async Task ReceberMensagemAsync()
        {
            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName, ReceiveMode.PeekLock);
            RegisterOnMessageHandlerAndReceiveMessages();
        }
        /// <summary>
        /// Método responsável por definir ações de sucesso e err na captura da mensagem
        /// </summary>
        private void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 3,
                AutoComplete = false
            };

            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions: messageHandlerOptions);
        }

        /// <summary>
        /// Método responsável por processar a mensagem
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            try
            {
                string teste = Encoding.UTF8.GetString(message.Body);

                ProductionAreaChangedMessage areaChangedMessage = JsonConvert.DeserializeObject<ProductionAreaChangedMessage>(teste);

                if (areaChangedMessage.State == ProductionAreaChangedMessage.ProductionAreaState.Added)
                {
                    _repository.Insert(RetornaAreasDeProducao(areaChangedMessage.ProductionArea));
                }
                else if (areaChangedMessage.State == ProductionAreaChangedMessage.ProductionAreaState.Modified)
                {
                    _repository.Update(RetornaAreasDeProducao(areaChangedMessage.ProductionArea));
                }
                else
                {
                    _repository.Delete(RetornaAreasDeProducao(areaChangedMessage.ProductionArea));
                }

                await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Métofo de exceção
        /// </summary>
        /// <param name="exceptionReceivedEventArgs"></param>
        /// <returns></returns>
        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            return Task.CompletedTask;
        }

        private static ProductionAreas RetornaAreasDeProducao(ProductionAreaTO productionAreaTO)
        {
            return new ProductionAreas()
            {
                Name = productionAreaTO.Name,
                ProductionAreaId = productionAreaTO.Id,
                Status = productionAreaTO.Status,
                Restrictions = productionAreaTO.Restrictions.ToArray()
            };
        }
    }
}
