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
            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal("PRINCIPAL_NAME2", "PRINCIPAL_PASSWORD2", "11dbbfe2-89b8-4549-be10-cec364e59551", AzureEnvironment.AzureGlobalCloud);
            var serviceBusManager = ServiceBusManager.Authenticate(credentials, "8f738168-bc53-45e3-a83d-45ebfe947df2");
            var serviceBusNamespace = serviceBusManager.Namespaces.GetByResourceGroup("FIAP-NET", "GeekBurgerNET");

            var topics = await serviceBusNamespace.Topics.ListAsync();
            var topic = topics.FirstOrDefault(t => t.Name == "storecatalog");

            SubscriptionInner subscriptionInner = new SubscriptionInner()
            {
            };

            /*Criação da subscrição no tópico de areas de produção*/
            await topic.Manager.Inner.Subscriptions.CreateOrUpdateAsync("FIAP-NET", "GeekBurgerNET", "productionareachangedtopic", "productionarea", subscriptionInner);

            if (topic == null)
            {
                topic = await serviceBusNamespace.Topics.Define("storecatalog")
                    .WithSizeInMB(1024)
                    .CreateAsync();

                await topic.Manager.Inner.Subscriptions.CreateOrUpdateAsync("FIAP-NET", "GeekBurgerNET", "storecatalog", "catalog", subscriptionInner);
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
                var topicClient = new TopicClient("Endpoint=sb://geekburgernet.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Z8l0qsaaDXT7gv5z0lUlmB1dH/ISKypGf3/OVFOIGsU=", "storecatalog");
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
