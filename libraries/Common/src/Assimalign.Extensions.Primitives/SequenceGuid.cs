
using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Assimalign.Extensions.Primitives;

[Serializable]
public readonly struct SequenceGuid : 
    ISpanFormattable, 
    IFormattable, 
    IComparable, 
    IComparable<SequenceGuid>, 
    IEquatable<SequenceGuid>
{
    [DllImport("rpcrt4.dll", SetLastError = true)]
    internal static extern int UuidCreateSequential(out Guid guid);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string FastAllocateString(int length);

    private const uint length = 16;

    private readonly int    a;
    private readonly short  b;
    private readonly short  c;
    private readonly byte   d;
    private readonly byte   e;
    private readonly byte   f;
    private readonly byte   g;
    private readonly byte   h;
    private readonly byte   i;
    private readonly byte   j;
    private readonly byte   k;

    public SequenceGuid(byte[] bytes) : this(new ReadOnlySpan<byte>(bytes)) { }

    public SequenceGuid(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length != length)
        {
            throw new ArgumentException("");
        }
        _ = BitConverter.IsLittleEndian;
        this = MemoryMarshal.Read<SequenceGuid>(bytes);
    }

    public unsafe bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
    {
		if (format.Length == 0)
		{
			format = "D";
		}
		if (format.Length != 1)
		{
			throw new FormatException(SR.Format_InvalidGuidFormatSpecification);
		}
		bool flag = true;
		bool flag2 = false;
		int num = 0;
		int num2;
		switch (format[0])
		{
			case 'D':
			case 'd':
				num2 = 36;
				break;
			case 'N':
			case 'n':
				flag = false;
				num2 = 32;
				break;
			case 'B':
			case 'b':
				num = 8192123;
				num2 = 38;
				break;
			case 'P':
			case 'p':
				num = 2687016;
				num2 = 38;
				break;
			case 'X':
			case 'x':
				num = 8192123;
				flag = false;
				flag2 = true;
				num2 = 68;
				break;
			default:
				throw new FormatException(SR.Format_InvalidGuidFormatSpecification);
		}
		if (destination.Length < num2)
		{
			charsWritten = 0;
			return false;
		}
		fixed (char* ptr = &MemoryMarshal.GetReference(destination))
		{
			char* ptr2 = ptr;
			if (num != 0)
			{
				char* num3 = ptr2;
				ptr2 = num3 + 1;
				*num3 = (char)num;
			}
			if (flag2)
			{
				char* num4 = ptr2;
				ptr2 = num4 + 1;
				*num4 = '0';
				char* num5 = ptr2;
				ptr2 = num5 + 1;
				*num5 = 'x';
				ptr2 += HexsToChars(ptr2, _a >> 24, _a >> 16);
				ptr2 += HexsToChars(ptr2, _a >> 8, _a);
				char* num6 = ptr2;
				ptr2 = num6 + 1;
				*num6 = ',';
				char* num7 = ptr2;
				ptr2 = num7 + 1;
				*num7 = '0';
				char* num8 = ptr2;
				ptr2 = num8 + 1;
				*num8 = 'x';
				ptr2 += HexsToChars(ptr2, _b >> 8, _b);
				char* num9 = ptr2;
				ptr2 = num9 + 1;
				*num9 = ',';
				char* num10 = ptr2;
				ptr2 = num10 + 1;
				*num10 = '0';
				char* num11 = ptr2;
				ptr2 = num11 + 1;
				*num11 = 'x';
				ptr2 += HexsToChars(ptr2, _c >> 8, _c);
				char* num12 = ptr2;
				ptr2 = num12 + 1;
				*num12 = ',';
				char* num13 = ptr2;
				ptr2 = num13 + 1;
				*num13 = '{';
				ptr2 += HexsToCharsHexOutput(ptr2, _d, _e);
				char* num14 = ptr2;
				ptr2 = num14 + 1;
				*num14 = ',';
				ptr2 += HexsToCharsHexOutput(ptr2, _f, _g);
				char* num15 = ptr2;
				ptr2 = num15 + 1;
				*num15 = ',';
				ptr2 += HexsToCharsHexOutput(ptr2, _h, _i);
				char* num16 = ptr2;
				ptr2 = num16 + 1;
				*num16 = ',';
				ptr2 += HexsToCharsHexOutput(ptr2, _j, _k);
				char* num17 = ptr2;
				ptr2 = num17 + 1;
				*num17 = '}';
			}
			else
			{
				ptr2 += HexsToChars(ptr2, _a >> 24, _a >> 16);
				ptr2 += HexsToChars(ptr2, _a >> 8, _a);
				if (flag)
				{
					char* num18 = ptr2;
					ptr2 = num18 + 1;
					*num18 = '-';
				}
				ptr2 += HexsToChars(ptr2, _b >> 8, _b);
				if (flag)
				{
					char* num19 = ptr2;
					ptr2 = num19 + 1;
					*num19 = '-';
				}
				ptr2 += HexsToChars(ptr2, _c >> 8, _c);
				if (flag)
				{
					char* num20 = ptr2;
					ptr2 = num20 + 1;
					*num20 = '-';
				}
				ptr2 += HexsToChars(ptr2, _d, _e);
				if (flag)
				{
					char* num21 = ptr2;
					ptr2 = num21 + 1;
					*num21 = '-';
				}
				ptr2 += HexsToChars(ptr2, _f, _g);
				ptr2 += HexsToChars(ptr2, _h, _i);
				ptr2 += HexsToChars(ptr2, _j, _k);
			}
			if (num != 0)
			{
				char* num22 = ptr2;
				ptr2 = num22 + 1;
				*num22 = (char)(num >> 16);
			}
		}
		charsWritten = num2;
		return true;
	}


    public override string ToString()
    {
        return ToString("D", null);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format))
        {
            format = "D";
        }
        if (format!.Length != 1)
        {
            throw new FormatException("");
        }
        int length;
        switch (format![0])
        {
            case 'D':
            case 'd':
                length = 36;
                break;
            case 'N':
            case 'n':
                length = 32;
                break;
            case 'B':
            case 'P':
            case 'b':
            case 'p':
                length = 38;
                break;
            case 'X':
            case 'x':
                length = 68;
                break;
            default:
                throw new FormatException("Invalid Guid Format");
        }
        string text = FastAllocateString(length);
        int charsWritten;

        bool flag = TryFormat(new Span<char>(text.ToCharArray()), out charsWritten, format);
        return text;
    }

    public int CompareTo(SequenceGuid other)
    {
        throw new NotImplementedException();
    }

    public bool Equals(SequenceGuid other)
    {
        throw new NotImplementedException();
    }

    public int CompareTo(object obj)
    {
        throw new NotImplementedException();
    }

	private unsafe static int HexsToChars(char* guidChars, int a, int b)
	{
		*guidChars = HexConverter.ToCharLower(a >> 4);
		guidChars[1] = HexConverter.ToCharLower(a);
		guidChars[2] = HexConverter.ToCharLower(b >> 4);
		guidChars[3] = HexConverter.ToCharLower(b);
		return 4;
	}

	private unsafe static int HexsToCharsHexOutput(char* guidChars, int a, int b)
	{
		*guidChars = '0';
		guidChars[1] = 'x';
		guidChars[2] = HexConverter.ToCharLower(a >> 4);
		guidChars[3] = HexConverter.ToCharLower(a);
		guidChars[4] = ',';
		guidChars[5] = '0';
		guidChars[6] = 'x';
		guidChars[7] = HexConverter.ToCharLower(b >> 4);
		guidChars[8] = HexConverter.ToCharLower(b);
		return 9;
	}


	public static SequenceGuid New()
    {
        Guid guid;
        UuidCreateSequential(out guid);
        var s = guid.ToByteArray();
        var t = new byte[16];
        t[3] = s[0];
        t[2] = s[1];
        t[1] = s[2];
        t[0] = s[3];
        t[5] = s[4];
        t[4] = s[5];
        t[7] = s[6];
        t[6] = s[7];
        t[8] = s[8];
        t[9] = s[9];
        t[10] = s[10];
        t[11] = s[11];
        t[12] = s[12];
        t[13] = s[13];
        t[14] = s[14];
        t[15] = s[15];
        return new SequenceGuid(t);
    }
}