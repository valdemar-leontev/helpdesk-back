namespace Helpdesk.WebApi.Helpers;

public class StrongPasswordGenerator
{
    private readonly int _length;

    public StrongPasswordGenerator(int length)
    {
        _length = length;
    }

    private class CharRange
    {
        private readonly Random _random = new(Environment.TickCount);
        public char Begin { get; set; }
        public char End { get; set; }
        public bool IsPresent { get; set; }

        public char GetRandomChar()
        {
            IsPresent = true;
            return Convert.ToChar(
                _random.Next(Convert.ToInt32(Begin), Convert.ToInt32(End)));
        }

        public bool IsFrom(char c)
        {
            return c >= Begin && c <= End;
        }
    }

    private class CharEqualityComparer : IEqualityComparer<char>
    {
        public bool Equals(char x, char y)
        {
            return char.ToLower(x) == char.ToLower(y);
        }

        public int GetHashCode(char obj)
        {
            return obj.GetHashCode();
        }
    }

    public string Next(bool onlySymbols = false)
    {
        var random = new Random(Environment.TickCount);
        var newPassword = string.Empty;

        var passChar = '\0';
        var previousPassChar = '\0';

        CharRange[] ranges =
        {
            new() { Begin = '0', End = '9' },
            new() { Begin = 'A', End = 'Z' },
            new() { Begin = 'a', End = 'z' }
        };

        if (!onlySymbols)
        {
            var list = ranges.ToList();
            list.Add(new CharRange { Begin = '#', End = '&' });
            ranges = list.ToArray();
        }

        var charEqualityComparer = new CharEqualityComparer();
        do
        {
            ranges.ToList().ForEach(p => p.IsPresent = false);

            newPassword = string.Empty;
            while (newPassword.Length < _length)
            {
                var indexRange = random.Next(0, ranges.Length);
                var range = ranges[indexRange];
                passChar = range.GetRandomChar();

                if (newPassword.Contains(passChar, charEqualityComparer) || range.IsFrom(previousPassChar))
                {
                    continue;
                }

                newPassword += passChar;
                previousPassChar = passChar;
            }
        } while (!ranges.All(r => r.IsPresent));

        return newPassword;
    }
}