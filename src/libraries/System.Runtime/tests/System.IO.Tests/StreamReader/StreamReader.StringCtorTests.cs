// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Text;
using Xunit;

namespace System.IO.Tests
{
    public class StreamReader_StringCtorTests
    {
        [Fact]
        public static void NullArgs_ThrowsArgumentNullException()
        {
            AssertExtensions.Throws<ArgumentNullException>("path", () => new StreamReader((string)null));
            AssertExtensions.Throws<ArgumentNullException>("path", () => new StreamReader((string)null, (FileStreamOptions)null));
            AssertExtensions.Throws<ArgumentNullException>("path", () => new StreamReader((string)null, (Encoding)null));
            AssertExtensions.Throws<ArgumentNullException>("path", () => new StreamReader((string)null, null, true));
            AssertExtensions.Throws<ArgumentNullException>("path", () => new StreamReader((string)null, null, true, null));
            AssertExtensions.Throws<ArgumentNullException>("path", () => new StreamReader((string)null, null, true, -1));
            AssertExtensions.Throws<ArgumentNullException>("options", () => new StreamReader("path", (FileStreamOptions)null));
            AssertExtensions.Throws<ArgumentNullException>("options", () => new StreamReader("path", Encoding.UTF8, true, null));

        }

        [Fact]
        public static void EmptyPath_ThrowsArgumentException()
        {
            // No argument name for the empty path exception
            AssertExtensions.Throws<ArgumentException>("path", () => new StreamReader(""));
            AssertExtensions.Throws<ArgumentException>("path", () => new StreamReader("", new FileStreamOptions()));
            AssertExtensions.Throws<ArgumentException>("path", () => new StreamReader("", Encoding.UTF8));
            AssertExtensions.Throws<ArgumentException>("path", () => new StreamReader("", Encoding.UTF8, true));
            AssertExtensions.Throws<ArgumentException>("path", () => new StreamReader("", Encoding.UTF8, true, new FileStreamOptions()));
            AssertExtensions.Throws<ArgumentException>("path", () => new StreamReader("", Encoding.UTF8, true, -1));
        }

        [Fact]
        public static void NullEncodingParamInCtor_ShouldNotThrowException()
        {
            // Call the constructor with overloads that has Stream and null encoding parameters.
            // It should not throw exception, to test passing the nullable encoding parameter..
            StreamReader streamReaderTest = new StreamReader(new MemoryStream(), null);
            streamReaderTest = new StreamReader(new MemoryStream(), null, false);
            streamReaderTest = new StreamReader(new MemoryStream(), null, false, 100);
            streamReaderTest = new StreamReader(new MemoryStream(), null, false, 100, false);
        }

        [Fact]
        public static void NegativeBufferSize_ThrowsArgumentOutOfRangeException()
        {
            AssertExtensions.Throws<ArgumentOutOfRangeException>("bufferSize", () => new StreamReader("path", Encoding.UTF8, true, -1));
            AssertExtensions.Throws<ArgumentOutOfRangeException>("bufferSize", () => new StreamReader("path", Encoding.UTF8, true, 0));
        }

        [Fact]
        public static void LackOfReadAccess_ThrowsArgumentException()
        {
            var noReadAccess = new FileStreamOptions { Access = FileAccess.Write };

            AssertExtensions.Throws<ArgumentException>("options", () => new StreamReader("path", noReadAccess));
            AssertExtensions.Throws<ArgumentException>("options", () => new StreamReader("path", Encoding.UTF8, false, noReadAccess));
            AssertExtensions.Throws<ArgumentException>("options", () => new StreamReader("path", Encoding.UTF8, true, noReadAccess));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public static void ReadToEnd_detectEncodingFromByteOrderMarks(bool detectEncodingFromByteOrderMarks)
        {
            string testfile = Path.GetTempFileName();
            try
            {
                File.WriteAllBytes(testfile, new byte[] { 65, 66, 67, 68 });
                using (var sr2 = new StreamReader(testfile, detectEncodingFromByteOrderMarks))
                {
                    Assert.Equal("ABCD", sr2.ReadToEnd());
                }
            }
            finally
            {
                File.Delete(testfile);
            }
        }
    }
}
