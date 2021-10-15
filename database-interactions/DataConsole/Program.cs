using DataConsole.Repositories;
using System;
using System.IO;

namespace DataConsole
{
    public class Program
    {
        private readonly MessageRepository _messageRepository;
        private readonly TextReader _input;
        private readonly TextWriter _output;

        static void Main(string[] args)
        {
            var program = new Program(new MessageRepository(),Console.In,Console.Out);

            program.Run(args);
        }

        public Program(MessageRepository messageRepository,TextReader input, TextWriter output)
        {
            _messageRepository = messageRepository;
            _input = input;
            _output = output;
        }

        public void Run(string[] args)
        {
            _messageRepository.Configure();

            string messageInput = null;
            do
            {
                _output.WriteLine("----Enter your message");
                messageInput = _input.ReadLine();

                if (!string.IsNullOrWhiteSpace(messageInput))
                {
                    _messageRepository.Save(messageInput);

                    var messages = _messageRepository.Get();

                    _output.WriteLine("----Saved Messages");
                    foreach (var item in messages)
                    {
                        _output.WriteLine(item);
                    }
                }


            } while (!string.IsNullOrWhiteSpace(messageInput));
            

        }
    }
}
