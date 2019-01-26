using System;
using MixinSdk.Bean;

namespace MixinSdk
{
    public class MixinException : Exception
    {
        public MixinError mixinError { get; private set; }

        public MixinException(MixinError error)
        {
            mixinError = error;
        }

        public override string Message => mixinError.ToString();
    }
}
