using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeekBurger.Production.Contract;
using GeekBurger.Production.Model;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace GeekBurger.Production.Service
{
    /// <summary>
    /// Serviços relacionados ao recebimento de novos pedidos.
    /// </summary>
    public class PaidOrderService : IPaidOrderService
    {
        private IConfiguration _configuration;
        private IServiceBusNamespace _namespace;
        private static ServiceBusConfiguration serviceBusConfiguration;
        private static IOrderFinishedService _orderFinishedService;
        private const string TopicName = "ProductionAreaChangedTopic";
        private const string SubscriptionName = "ProductionAreasSubscription";
        private static string _productionAreaId = "Grill 2 - No Gluten&Wheat";

        public PaidOrderService(IConfiguration configuration, IOrderFinishedService orderFinishedService)
        {
            _configuration = configuration;
            _namespace = _configuration.GetServiceBusNamespace();
            _orderFinishedService = orderFinishedService;
        }

        public void SubscribeToTopic(string topicName, List<ProductionArea> productionAreas)
        {
            var topic = _namespace.Topics.GetByName(topicName);

            topic.Subscriptions.DeleteByName(SubscriptionName);

            if (!topic.Subscriptions.List().Any(subscription => subscription.Name.Equals(SubscriptionName, StringComparison.InvariantCultureIgnoreCase)))
            {
                topic.Subscriptions.Define(SubscriptionName).Create();
            }

            ReceiveMessages(topicName, productionAreas);
        }

        private async void ReceiveMessages(string topicName, List<ProductionArea> productionAreas)
        {
            serviceBusConfiguration = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            var subscriptionClient = new SubscriptionClient(serviceBusConfiguration.ConnectionString, topicName, SubscriptionName);
            
            await subscriptionClient.RemoveRuleAsync("$Default");

            int cont = 0;
            foreach (ProductionArea pa in productionAreas)
            {
                await subscriptionClient.AddRuleAsync(new RuleDescription
                {
                    Filter = new CorrelationFilter { CorrelationId = pa.Id.ToString() },
                    Name = "filter-productionAreaId_" + cont
                });

                cont++;
            }

            var mo = new MessageHandlerOptions(ExceptionHandle) { AutoComplete = true };
            subscriptionClient.RegisterMessageHandler(Handle, mo);

            Console.ReadLine();
        }

        private static Task Handle(Message message, CancellationToken arg2)
        {
            OrderFinishedMessage orderFinishedMessage = new OrderFinishedMessage() { OrderFinishedId = new Guid(message.Label) };
            
            Random waitTime = new Random();
            int seconds = waitTime.Next(5 * 1000, 21 * 1000);
            System.Threading.Thread.Sleep(seconds);

            _orderFinishedService.AddToMessageList(orderFinishedMessage);
            _orderFinishedService.SendMessagesAsync();

            Thread.Sleep(40000);

            return Task.CompletedTask;
        }

        private static Task ExceptionHandle(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler encountered an exception {arg.Exception}.");
            var context = arg.ExceptionReceivedContext;
            Console.WriteLine($"- Endpoint: {context.Endpoint}, Path: {context.EntityPath}, Action: {context.Action}");
            
            return Task.CompletedTask;
        }
    }
}
