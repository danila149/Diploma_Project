using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class KeyboardInputHelper
{
    static readonly KeyCode[] _keyCodes =
        System.Enum.GetValues(typeof(KeyCode))
            .Cast<KeyCode>()
            .Where(k => k < KeyCode.Mouse0)
            .ToArray();

    public static bool IsAnyKeyDown(params KeyCode[] interestingCodes)
    {
        return Enumerable.Intersect(GetCurrentKeys(), interestingCodes).Any();
    }

    public static bool IsAllKeyDown(params KeyCode[] interestingCodes)
    {
        return Enumerable.SequenceEqual(GetCurrentKeys(), interestingCodes);
    }

    public static IEnumerable<KeyCode> GetCurrentKeys()
    {
        if (Input.anyKeyDown)
        {
            for (int i = 0; i < _keyCodes.Length; i++)
                if (Input.GetKey(_keyCodes[i]))
                    yield return _keyCodes[i];
        }
    }

    public static IEnumerable<KeyCode> GetCurrentKeysUp()
    {
        for (int i = 0; i < _keyCodes.Length; i++)
            if (Input.GetKeyUp(_keyCodes[i]))
                yield return _keyCodes[i];
    }
}