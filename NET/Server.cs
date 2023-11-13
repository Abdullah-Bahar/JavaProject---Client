using JavaProject___Server.NET.IO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace JavaProject___Client.NET
{

    
    public class Server
    {
        //Serverdan gelen paketleri dinlemek için eventler
        public event Action UserConnectedEvent;
        public event Action UserDisconnectedEvent;
        public event Action MessageReceivedEvent;
        public event Action GroupCreatedEvent;

        public event Action LoginCorrectEvent;
        public event Action LoginFailEvent;

        public event Action RegisterSuccessEvent;
        public event Action RegisterFailEvent;

        public string Username;
        public string UID;

        private static TcpClient _client = new TcpClient();
        public PacketReader PacketReader;
        public Server()
        {
            _client = new TcpClient();
        }

        //servere register yapmak için kullanılan fonksiyon
        public void Register(string username, string email, string password)
        {
            if (!_client.Connected)
            {
                try
                {
                    _client.Connect("46.31.77.173", 9001);
                }
                catch
                {
                    MessageBox.Show("Sunucu bulunumadı, uygulama kapanıyor...");
                    Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
                    return;
                }
                PacketReader = new PacketReader(_client.GetStream());
                ReadPackets();
            }
            var RegisterPacket = new PacketBuilder();
            RegisterPacket.WriteOpCode(0);
            RegisterPacket.WriteMessage(username);
            RegisterPacket.WriteMessage(email);
            RegisterPacket.WriteMessage(password);
            _client.Client.Send(RegisterPacket.GetPacketBytes());
        }

        //Servere giriş yapmak için kullanılan fonksiyon
        public void Login(string email, string password)
        {
            if (!_client.Connected)
            {
                try
                {
                    _client.Connect("46.31.77.173", 9001);
                }
                catch
                {
                    MessageBox.Show("Sunucu ile iletişim kurulamıyor, daha sonra tekrar deneyin");
                }

                PacketReader = new PacketReader(_client.GetStream());
                ReadPackets();
            }
            var loginPacket = new PacketBuilder();
            loginPacket.WriteOpCode(1);
            loginPacket.WriteMessage(email);
            loginPacket.WriteMessage(password);
            _client.Client.Send(loginPacket.GetPacketBytes());
        }
        // OpCodes
        //0 - Register
        //1 - Login
        //2 - Info
        //3 - New Connection
        //4 - Disconnect
        //5 - Message - Group Message
        //6 - Group Created


        //Sunucudan gelen paketleri okumak için kullanılan fonksiyon
        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var opcode = PacketReader.ReadByte();
                        switch (opcode)
                        {
                            case 0:
                                if (bool.Parse(PacketReader.ReadMessage()) == true)
                                {
                                    RegisterSuccessEvent?.Invoke();
                                }
                                else
                                {
                                    RegisterFailEvent?.Invoke();
                                }
                                break;
                            case 1:
                                if (bool.Parse(PacketReader.ReadMessage()) == true)
                                {
                                    LoginCorrectEvent?.Invoke();
                                }
                                else
                                {
                                    LoginFailEvent?.Invoke();
                                }
                                break;
                            case 2:
                                Username = PacketReader.ReadMessage();
                                UID = PacketReader.ReadMessage();
                                break;
                            case 3:
                                UserConnectedEvent?.Invoke();
                                break;
                            case 4:
                                UserDisconnectedEvent?.Invoke();
                                break;
                            case 5:
                                MessageReceivedEvent?.Invoke();
                                break;
                            case 6:
                                GroupCreatedEvent?.Invoke();
                                break;
                            default:
                                Console.WriteLine("Unknown opcode: " + opcode);
                                break;
                        }
                    }
                    catch
                    {
                        //Eğer sunucu çökerse client kapanıyor
                        MessageBox.Show("Sunucu çöktü, uygulama kapanıyor... ");
                        Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
                        return;
                    }
                }
            });
        }
        public void createGroup(string groupName, string clientIDS)
        {
            var packet = new PacketBuilder();
            packet.WriteOpCode(6);
            packet.WriteMessage(groupName);
            packet.WriteMessage(clientIDS);
            _client.Client.Send(packet.GetPacketBytes());
        }
        public void SendMessageToGroup(string message, string contactUID)
        {
            var packet = new PacketBuilder();
            packet.WriteOpCode(10);
            packet.WriteMessage(message);
            packet.WriteMessage(contactUID);
            _client.Client.Send(packet.GetPacketBytes());
        }
        public void SendMessageToUser(string message, string contactUID)
        {
            var packet = new PacketBuilder();
            packet.WriteOpCode(5);
            packet.WriteMessage(message);
            packet.WriteMessage(contactUID);
            _client.Client.Send(packet.GetPacketBytes());
        }
    }
}
