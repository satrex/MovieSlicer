using System;
using Xunit;

namespace Satrex.FFMpeg.Test
{
    public class UnitTest1
    {
        [Fact]
        public void ExecutablePathIsValid()
        {
            FFMpeg.ExecutablePath = "/";
            Assert.Equal("/", FFMpeg.ExecutablePath);
        }

        [Fact]
        public void ExecutablePathIsValidAfterNew()
        {
            FFMpeg.ExecutablePath = "/";
            var instance = new FFMpeg();
            Assert.Equal("/", FFMpeg.ExecutablePath);
        }

        [Fact]
        public void ExecutablePathIsValidBeforeNew()
        {
            var instance = new FFMpeg();
            System.Threading.Thread.Sleep(1000);
            Assert.Equal("/usr/local/bin/FFMPeg", FFMpeg.ExecutablePath);

            FFMpeg.ExecutablePath = "/";
            Assert.Equal("/", FFMpeg.ExecutablePath);
        }
    }
}
