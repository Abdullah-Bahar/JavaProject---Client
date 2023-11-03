using JavaProject___Server.NET.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JavaProject___Client.NET
{

    
    internal class Server
    {
        //Serverdan gelen paketleri dinlemek için eventler
        public event Action UserConnectedEvent;
        public event Action UserDisconnectedEvent;
        public event Action MessageReceivedEvent;
        public event Action GroupCreatedEvent;

        private static TcpClient _client = new TcpClient();
        public PacketReader PacketReader;
        public Server()
        {
            _client = new TcpClient();
        }

        //Servera bağlanmak için kullanılan fonksiyon
        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                try
                {
                    _client.Connect("127.0.0.1", 9001);
                }
                catch
                {
                    //Eğer sunucuya bağlanamazsa uygulama kapanıcak
                    MessageBox.Show("Sunucu bulunumadı, uygulama kapanıyor...");
                    Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
                    return;
                }

                PacketReader = new PacketReader(_client.GetStream());

                //Sunucuya bağlandığında kullanıcı adını yolluyor
                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteMessage(username);
                _client.Client.Send(connectPacket.GetPacketBytes());
                //Sunucudan gelen paketleri okumak için kullanılan fonksiyon
                ReadPackets();
            }
        }

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
                            //Sunucu 1 numaralı opcode gönderince UserConnectedEvent çalışıcak
                            case 1:
                                UserConnectedEvent?.Invoke();
                                break;
                            //Sunucu 2 numaralı opcode gönderince GroupCreatedEvent çalışıcak
                            case 2:
                                GroupCreatedEvent?.Invoke();
                                break;
                            //Sunucu 5 numaralı opcode gönderince MessageReceivedEvent çalışıcak
                            case 5:
                                MessageReceivedEvent?.Invoke();
                                break;
                            //Sunucu 10 numaralı opcode gönderince UserDisconnectedEvent çalışıcak
                            case 10:
                                UserDisconnectedEvent?.Invoke();
                                break;
                            //opcode yanlışsa Konsola hata yolluyor
                            default:
                                Console.WriteLine("Unknown opcode: " + opcode);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Eğer sunucu çökerse client kapanıyor
                        MessageBox.Show("Sunucu çöktü, uygulama kapanıyor... ");
                        Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
                        return;
                    }
                }
            });
        }

    }
}
