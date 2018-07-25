using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Starbucks.Product.UpdateStatus.QueueService.Common.Commands;

namespace MSMQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //SendOrderConfirmationMessageToQueue();
            SendUpdateInternalProductApiWorker();
            //SendOverwriteInternalProductApiWorker();
        }

        private static void SendOrderConfirmationMessageToQueue()
        {
            TextWriter texsTextWriter = TextWriter.Null;

            MessageQueue messageQueue = null;
            SendOrderIsReadyNotification bodyMessage = new SendOrderIsReadyNotification { OrderToken = "12145663820298520060527830655218" };

            XmlSerializer abc = new XmlSerializer(bodyMessage.GetType());
            abc.Serialize(texsTextWriter, bodyMessage);

            var aa = MessageQueue.GetPrivateQueuesByMachine("mstmp00449.sbweb.prod");

            messageQueue = new MessageQueue(@"FormatName:Direct=OS:mstmp00450.sbweb.prod\private$\starbucks.orderconfirmation.queueservice");
            messageQueue.Send("test", MessageQueueTransactionType.Single);
        }

        private static void SendUpdateInternalProductApiWorker()
        {
            var ab = new List<AvailabilityItem>
            {
                new AvailabilityItem()
                {
                    Sku = "45",
                    Status = "Purchasable",
                }
            };

            var ps = new UpdateProductStatus
            {
                BusinessDate = DateTime.Now,
                Items = ab,
                RequestType = "update",
                StoreNumber = 15253,
                Timestamp = DateTime.Now
            };

            MessageQueue messageQueue = null;
            
            messageQueue = new MessageQueue(@"FormatName:Direct=OS:mstmp00450.sbweb.prod\private$\starbucks.product.updatestatus.queueservice");
            messageQueue.Send(ps, MessageQueueTransactionType.Single);
        }

        private static void SendOverwriteInternalProductApiWorker()
        {
            var ab = new List<AvailabilityItem>
            {
                new AvailabilityItem()
                {
                    Sku = "45",
                    Status = "Purchasable",
                },
                new AvailabilityItem()
                {
                    Sku = "91",
                    Status = "UnPurchasable"
                }
            };

            var ps = new UpdateProductStatus
            {
                BusinessDate = DateTime.Now,
                Items = ab,
                RequestType = "overwrite",
                StoreNumber = 15253,
                Timestamp = DateTime.Now
            };

            MessageQueue messageQueue = null;

            messageQueue = new MessageQueue(@"FormatName:Direct=OS:mstmp00449.sbweb.prod\private$\Starbucks.Product.UpdateProduct.QueueService");
            messageQueue.Send(ps, MessageQueueTransactionType.Single);
        }
    }
}