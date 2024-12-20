using MA.SlotService.Application.Abstractions;

namespace MA.SlotService.Infrastructure.Randomization;

public class SpinResultGenerator: ISpinResultGenerator
{
    private readonly Random _random = new();

    public byte[] Generate()
    {
        var result = new byte[3];

        for (var i = 0; i < result.Length; i++)
        {
            result[i] = (byte) _random.Next(0, 10);
        }

        return result;
    }
}