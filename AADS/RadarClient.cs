using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AADS
{
    public class RadarClient
    {
        public static Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static byte[] buffer;
        public static void ConnectToServer(IPAddress ipAddr, int port)
        {
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Debug.WriteLine("Connection attempt " + attempts);
                    ClientSocket.Connect(ipAddr, port);
                    buffer = new byte[4];
                    ClientSocket.BeginReceive(buffer, 0, 4, SocketFlags.None, ReceiveCallback, ClientSocket);
                }
                catch (SocketException e)
                {
                    Debug.WriteLine(e);
                }
            }
            Debug.WriteLine("Connected");
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;
            MainForm form = MainForm.GetInstance();
            TrackUpdateHandler trackHandler = form.trackHandler;
            try
            {
                received = current.EndReceive(AR);
                int length = BitConverter.ToInt32(buffer, 0);
                buffer = new byte[length];
                current.Receive(buffer, 0, length, SocketFlags.None);
                byte[] dataSent = new byte[length];
                Array.Copy(buffer, dataSent, length);
                string text = Encoding.ASCII.GetString(dataSent);
                if (text.Equals("Exit"))
                {
                    Exit();
                }
                else
                {
                    var command = JsonSerializer.Deserialize<RadarCommand>(text);
                    if (command.Feature == RadarFeature.Track)
                    {
                        var args = JsonSerializer.Deserialize<TrackCommandArgs>(command.Args.ToString());
                        var track = args.Track;
                        if (command.Operation == "Add" || command.Operation == "Update")
                        {
                            trackHandler.AddTrack(track);
                        }
                        else if (command.Operation == "Remove")
                        {
                            trackHandler.RemoveTrack(track.Key);
                        }
                        else if (command.Operation == "Clear")
                        {
                            trackHandler.Clear();
                        }
                    }
                }
                buffer = new byte[4];
                ClientSocket.BeginReceive(buffer, 0, 4, SocketFlags.None, ReceiveCallback, ClientSocket);
            }
            catch (SocketException e)
            {

            }
            catch (ObjectDisposedException e)
            {

            }
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        public static void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
        }

        /// <summary>
        /// Sends a string to the server with ASCII encoding.
        /// </summary>
        public static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        public static void ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            Debug.WriteLine(text);
        }
    }
}
