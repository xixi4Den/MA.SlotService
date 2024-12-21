using MA.SlotService.Application.Abstractions;

namespace MA.SlotService.Infrastructure.Randomization;

public class SpinResultGenerator: ISpinResultGenerator
{
    private readonly Random _random = new();

    public int[] Generate()
    {
        var result = new int[3];

        for (var i = 0; i < result.Length; i++)
        {
            result[i] = _random.Next(0, 10);
        }

        return result;
    }
}