using System.Buffers.Binary;
using System.Text;

namespace Client3
{
    public static class Protocol
    {
        public static void ReadAll(this Stream stream, Span<byte> buffer)
        {
            if (buffer.Length == 0)
            {
                return;
            }
            while (buffer.Length > 0)
            {
                
                var read = stream.Read(buffer);
                if (read == 0)
                {
                    throw new Exception("Connection Closed");
                }
                buffer = buffer.Slice(read);
            }
        }

        /// <summary>
        /// Read the size of the message.
        /// </summary>
        /// <param name="stream"></param>
        public static int ReadSize(this Stream stream)
        {
            var sizeMsg = new byte[4];

            stream.ReadAll(sizeMsg);

            return BitConverter.ToInt32(sizeMsg);
        }



        /// <summary>
        /// Read the bytes of the message.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadBytes(this Stream stream)
        {
            
            var size = ReadSize(stream);

            var bytesMsg = new byte[size];

            stream.ReadAll(bytesMsg);

            return bytesMsg;
        }

        /// <summary>
        /// Send the bytes trough the network.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        public static void WriteBytes(this Stream stream, Span<byte> buffer)
        {   
            int length = buffer.Length;
            var bytes = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(bytes, length);

            stream.Write(bytes);
            stream.Write(buffer);
        }


        /// <summary>
        /// Send the message.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="message"></param>
        public static void WriteString(this Stream stream, string message)
        {
            stream.WriteBytes(Encoding.UTF8.GetBytes(message));
        }


        /// <summary>
        /// Receive the message.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadString(this Stream stream)
        {
            return Encoding.UTF8.GetString(stream.ReadBytes());
        }
    }
}
    