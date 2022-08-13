using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GeekBurger.Production.Contract;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GeekBurger.Production.Service
{
    /// <summary>
    /// Serviços relacionados com a finalização de produção de ordem.
    /// </summary>
    public class OrderFinishedService : IOrderFinishedService
    {
        private const string Topic = "OrderFinishedTopic";
        private IConfiguration _configuration;
        private IMapper _mapper;
        private List<Message> _messages;
        private Task _lastTask;
        private IServiceBusNamespace _namespace;
        
        public OrderFinishedService(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
            _messages = new List<Message>();
            _namespace = _configuration.GetServiceBusNamespace();
            EnsureTopicIsCreated();
        }

        /// <summary>
        /// Método utilizado para que seja validado se o item foi realmente incluído no tópico
        /// </summary>
        public void EnsureTopicIsCreated()
        {
            if (!_namespace.Topics.List()
                    .Any(topic => topic.Name
                    .Equals(Topic, StringComparison.InvariantCultureIgnoreCase))) _namespace.Topics.Define(Topic)
                    .WithSizeInMB(1024).Create();
        }

        /// <summary>
        /// Método utilizado para adicionar novo item na lista de mensagens
        /// </summary>
        /// <param name="orderFinished"></param>
        public void AddToMessageList(OrderFinishedMessage orderFinished)
        {
            _messages.Add(this.GetMessage(orderFinished));
        }

        /// <summary>
        /// Método utilizado para criar uma nova mensagem utilizando como base dados da ordem finalizada
        /// </summary>
        /// <param name="orderFinished"></param>
        /// <returns></returns>
        public Message GetMessage(OrderFinishedMessage orderFinished)
        {
            var orderFinishedSerialized = JsonConvert.SerializeObject(orderFinished);
            var orderFinishedByteArray = Encoding.UTF8.GetBytes(orderFinishedSerialized);

            return new Message
            {
                Body = orderFinishedByteArray,
                MessageId = Guid.NewGuid().ToString(),
                Label = orderFinished.OrderFinishedId.ToString()
            };
        }

        /// <summary>
        /// Inclui nova mensagem na lista do serviceBus
        /// </summary>
        public async void SendMessagesAsync()
        {
            if (_lastTask != null && !_lastTask.IsCompleted) return;

            var config = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            var topicClient = new TopicClient(config.ConnectionString, Topic);

            _lastTask = SendAsync(topicClient);

            await _lastTask;

            var closeTask = topicClient.CloseAsync();
            await closeTask;
            HandleException(closeTask);
        }

        /// <summary>
        /// Inclui registro no tópico de itens produzidos e aguarda um tempo X para retorno da mensagem
        /// </summary>
        /// <param name="topicClient"></param>
        /// <returns></returns>
        public async Task SendAsync(TopicClient topicClient)
        {
            int tries = 0;
            Message message;
            while (true)
            {
                if (_messages.Count <= 0)
                    break;

                lock (_messages)
                {
                    message = _messages.FirstOrDefault();
                }

                var sendTask = topicClient.SendAsync(message);
                await sendTask;
                var success = HandleException(sendTask);

                if (!success)
                    Thread.Sleep(10000 * (tries < 60 ? tries++ : tries));
                else
                    _messages.Remove(message);
            }
        }

        /// <summary>
        /// Método para tratamento de retorno ao incluir na lista de produtos finalizados
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool HandleException(Task task)
        {
            if (task.Exception == null || task.Exception.InnerExceptions.Count == 0) return true;

            task.Exception.InnerExceptions.ToList().ForEach(innerException =>
            {
                Console.WriteLine($"Error in SendAsync task: {innerException.Message}. Details:{innerException.StackTrace} ");

                if (innerException is ServiceBusCommunicationException) 
                    Console.WriteLine("Problema de conexão com o host. A conexão com a Internet pode estar inativa");
            });

            return false;
        }
    }
}
