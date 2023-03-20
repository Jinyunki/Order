using Order.ViewModel;
using System.Net.Sockets;

namespace Order {
    class ClientData : MainModel {
        public TcpClient client { get; set; }
        public byte[] readByteData { get; set; }
        public int clientNumber;

        public ClientData(TcpClient client) {
            this.client = client;
            this.readByteData = new byte[1024];

            string clientEndPoint = client.Client.LocalEndPoint.ToString();
            char[] point = { '.', ':' };
            string[] splitedData = clientEndPoint.Split(point);
            this.clientNumber = int.Parse(splitedData[3]);            
        }
    }
}
