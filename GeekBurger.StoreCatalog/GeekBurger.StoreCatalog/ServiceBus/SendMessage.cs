using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeekBurger.StoreCatalog.Contract;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent.Models;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace GeekBurger.StoreCatalog.ServiceBus
{
    public class SendMessage
    {
        public SendMessage(bool criarTopico)
        {
            if (criarTopico) CriarTopicAsync();
        }

        public SendMessage()
        {
        }

        /// <summary>
        /// Método responsável por criar um tópico e criar a subscrição para que outros microserviços posssam se subscrever
        /// </summary>
        /// <returns></returns>
        public async Task CriarTopicAsync()
        {
            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal("bfd544b9-eb3d-4409-aa76-e2b83c39a446", "lovetoteach", "11dbbfe2-89b8-4549-be10-cec364e59551", AzureEnvironment.AzureGlobalCloud);
            var serviceBusManager = ServiceBusManager.Authenticate(credentials, "dbc49a7f-caee-46b5-a6a6-7eac85bf97f1");
            var serviceBusNamespace = serviceBusManager.Namespaces.GetByResourceGroup("fiap", "GeekBurger");

            var topics = await serviceBusNamespace.Topics.ListAsync();
            var topic = topics.FirstOrDefault(t => t.Name == "storecatalog");

            SubscriptionInner subscriptionInner = new SubscriptionInner()
            {
            };

            /*Criação da subscrição no tópico de areas de produção*/
            await topic.Manager.Inner.Subscriptions.CreateOrUpdateAsync("fiap", "GeekBurger", "productionareachangedtopic", "productionarea", subscriptionInner);

            if (topic == null)
            {
                topic = await serviceBusNamespace.Topics.Define("storecatalog")
                    .WithSizeInMB(1024)
                    .CreateAsync();

                await topic.Manager.Inner.Subscriptions.CreateOrUpdateAsync("fiap", "GeekBurger", "storecatalog", "catalog", subscriptionInner);
            }
        }

        /// <summary>
        /// Método resposável por enviar uma mensagem
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task SendMessagesAsync(ProductsByUserWrapper product)
        {
            try
            {
                var topicClient = new TopicClient("Endpoint=sb://geekburger.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=VrwaCn+4NbZkDFguQNGDCu2cMQ7IXyjOPLMto0HuE8Q=", "storecatalog");
                var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(product)));
                await topicClient.SendAsync(message);
                await topicClient.CloseAsync();

            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}
