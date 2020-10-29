﻿using System.IO;
using System.Text;

namespace Xamarin.Text
{
    public class CountingWriter : TextWriter
    {
        private readonly TextWriter _writer;
        private int _count;

        public CountingWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public override Encoding Encoding => _writer.Encoding;
        public int Count => _count;

        public override void Write(char value)
        {
            _count++;
            _writer.Write(value);
        }

        public override string ToString()
        {
            return _writer.ToString();
        }
    }
}