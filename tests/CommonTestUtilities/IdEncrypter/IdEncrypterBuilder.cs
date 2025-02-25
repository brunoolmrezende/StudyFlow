using Sqids;

namespace CommonTestUtilities.IdEncrypter
{
    public class IdEncrypterBuilder
    {
        public static SqidsEncoder<long> Build()
        {
            return new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = "2NwuxtKSqvZ3OeBFWX4f1C8igGVLAskzlb7Mjdm09HYRPpIocEUhnDJr5TaQ6y",
            });
        }
    }
}
