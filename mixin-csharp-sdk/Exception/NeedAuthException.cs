using System;
namespace MixinSdk
{
    public class NeedAuthException : Exception
    {
        public override string Message => "This method need JWT authorization";
        public override string ToString()
        {
            return Message;
        }
    }
}
