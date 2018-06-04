using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.IO.Pipes;


namespace IPC
{
    public class NamedPipesServer
    {
        // Named Pipe client and server
        private NamedPipeServerStream serverStream;

        // Named Pipe configuration variables
        public enum PipeDirection
        {
            DirectionIn,
            DirectionOut,
            DirectionInOut
        }
        public enum SendMode
        {
            ByteMode,
            MessageMode,
        }
        public bool Created { set; get; }

        public void CreateNewServerPipe(string PipeName)
        {
            try
            {
                serverStream = new NamedPipeServerStream(PipeName);
                Created = true;
            }
            catch (Exception err)
            {

                throw;
            }
        }
        public void CreateNewServerPipe(string PipeName, PipeDirection direction)
        {
            try
            {
                if (direction == PipeDirection.DirectionIn)
                {
                    serverStream = new NamedPipeServerStream(PipeName, System.IO.Pipes.PipeDirection.In);
                    Created = true;
                }
                else if (direction == PipeDirection.DirectionOut)
                {
                    serverStream = new NamedPipeServerStream(PipeName, System.IO.Pipes.PipeDirection.Out);
                    Created = true;
                }
                else if (direction == PipeDirection.DirectionInOut)
                {
                    serverStream = new NamedPipeServerStream(PipeName, System.IO.Pipes.PipeDirection.InOut);
                    Created = true;
                }
                else
                {
                    Created = false;
                }
            }
            catch (Exception err)
            {

                throw;
            }
        }
        public void CreateNewServerPipe(string PipeName, PipeDirection direction, SendMode mode)
        {
            switch (direction) 
            {
                case PipeDirection.DirectionIn:
                    if(mode == SendMode.ByteMode) serverStream = new NamedPipeServerStream(PipeName, System.IO.Pipes.PipeDirection.In, 1, PipeTransmissionMode.Byte);
                    else serverStream = new NamedPipeServerStream(PipeName, System.IO.Pipes.PipeDirection.In, 1, PipeTransmissionMode.Message);
                    break;
                case PipeDirection.DirectionOut:
                    if (mode == SendMode.ByteMode) serverStream = new NamedPipeServerStream(PipeName, System.IO.Pipes.PipeDirection.Out, 1, PipeTransmissionMode.Byte);
                    else serverStream = new NamedPipeServerStream(PipeName, System.IO.Pipes.PipeDirection.Out, 1, PipeTransmissionMode.Message);
                    break;
                case PipeDirection.DirectionInOut:
                    if (mode == SendMode.ByteMode) serverStream = new NamedPipeServerStream(PipeName, System.IO.Pipes.PipeDirection.InOut, 1, PipeTransmissionMode.Byte);
                    else serverStream = new NamedPipeServerStream(PipeName, System.IO.Pipes.PipeDirection.InOut, 1, PipeTransmissionMode.Message);
                    break;
                default:
                    break;
            }
        }
        public void WaitForConnection()
        {
            serverStream.WaitForConnection();
        }
        public void WaitForPipeDrain()
        {
            serverStream.WaitForPipeDrain();
        }
        public void WriteData(byte Content)
        {
            serverStream.WriteByte(Content);
        }
        public void WriteMessage(byte[] Content, int Offset, int Count)
        {
            serverStream.Write(Content, Offset, Count);
        }
        public byte ReadByte()
        {
            return (byte)serverStream.ReadByte();
        }
        public string ReadMessage()
        {
            string tempMessage = string.Empty;
            do
            {
                tempMessage += (char)serverStream.ReadByte();
            } while (!serverStream.IsMessageComplete);
            return tempMessage;
        }

        public bool CheckConnection()
        {
            return serverStream.IsConnected;
        }
        
        public void Disconnect()
        {
            if (serverStream.IsConnected)
            {
                serverStream.Disconnect();
            }
        }

        // How this works
        /*
         * 1. Check if serverStream is null. If yes, don't bother to coninue.
         * 2. If not null, check if serverStream is still connected. If yes,
         *    disconnect first (to prevent named pipe "pipe is broken" problem)
         * 3. After closing, set serverStream to null and let the Garbage Collector
         *    clean it later (to prevent "pipe is busy" problem)
         */
        public void ClosePipe()
        {
            if (serverStream != null)
            {
                if (serverStream.IsConnected)
                {
                    serverStream.Close();
                }
                serverStream = null;
            }
        }
    }

#region NamedPipe Client
    public class NamedPipeClient
    {
        public enum ReceiveMode
        {
            ByteMode,
            MessageMode,
            MessageInStringMode
        }
        NamedPipeClientStream clientStream;
        public bool isConnected { private set; get; }

        public NamedPipeClient()
        {

        }
        public NamedPipeClient(string PipeName)
        {
            CreateNewClientStream(PipeName);
        }
        public void CreateNewClientStream(string PipeName)
        {
            clientStream = new NamedPipeClientStream(PipeName);
        }
        public void ConnectToServer()
        {
            clientStream.Connect();
            if (clientStream.IsConnected)
            {
                isConnected = true;
            }
        }
        public void DisconnectToServer()
        {
            if (clientStream != null)
            {
                if (clientStream.IsConnected)
                {
                    clientStream.Dispose();
                }
                clientStream.Close();
                clientStream = null;
            }
        }
        public void WriteToServer(byte Content)
        {
            if (CheckConnection())
            {
                clientStream.WriteByte(Content);
            }
        }
        public void WriteToServer(byte[] Content, int offset, int length)
        {
            if (CheckConnection())
            {
                clientStream.Write(Content, offset, length);
            }
        }
        public dynamic ReadFromServer(ReceiveMode Mode = ReceiveMode.ByteMode)
        {
            switch (Mode)
            {
                case ReceiveMode.ByteMode:
                    return clientStream.ReadByte();
                case ReceiveMode.MessageMode:
                    byte[] tempMessage = null;
                    int messageLength = 0;
                    do
                    {
                        tempMessage[messageLength] = (byte)clientStream.ReadByte();
                        messageLength++;
                    } while (!clientStream.IsMessageComplete);
                    return tempMessage;
                case ReceiveMode.MessageInStringMode:
                    string tempStringMessage = string.Empty;
                    do
                    {
                        tempStringMessage += (char)clientStream.ReadByte();
                    } while (!clientStream.IsMessageComplete);
                    return tempStringMessage;
                default:
                    return 0;
            }
        }
        
        public bool CheckConnection()
        {
            return clientStream.IsConnected;
        }
        public void WaitForPipeDrain()
        {
            if (CheckConnection())
            {
                clientStream.WaitForPipeDrain();
            }
        }
    }
#endregion NamedPipe Client
}
