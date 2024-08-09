using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Bogus;
using System.Runtime.InteropServices;
using System.Text;

namespace Optimization7;

[MemoryDiagnoser]
//[SimpleJob(RunStrategy.ColdStart, invocationCount: 2)]
public class EmailMaskingBenchmark
{
    private string[] emails;

    [Params(1_000/*, 10_000*/)]
    public int EmailCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var faker = new Faker();

        // Sahte veriler için array
        emails = new string[EmailCount];

        for (int i = 0; i < emails.Length; i++)
        {
            // 70% valid, 30% invalid email address
            if (i < emails.Length * 0.7)
            {
                emails[i] = faker.Internet.Email();
            }
            else
            {
                // Invalid email address and big size string
                if (faker.Random.Bool())
                {
                    // Büyük boyutlu string oluşturma (örneğin 500 karakter)
                    emails[i] = faker.Lorem.Letter(500);
                }
                else
                {
                    // InValid email address
                    emails[i] = faker.Random.AlphaNumeric(10) + faker.Random.String2(10, "-_.") + "@" + faker.Random.AlphaNumeric(5);
                }
            }
        }

        emails = [.. emails.OrderBy(x => Guid.NewGuid())]; // randomize the order
    }

    [Benchmark]
    public void MaskEmail_V1()
    {
        foreach (var email in emails)
        {
            MaskEmailV1(email, 2, '*');
        }
    }

    [Benchmark]
    public void MaskEmail_V2()
    {
        foreach (var email in emails)
        {
            MaskEmailV2(email, 2, '*');
        }
    }

    [Benchmark]
    public void MaskEmail_V2_2()
    {
        foreach (var email in emails)
        {
            MaskEmailV2_2(email, 2, '*');
        }
    }

    [Benchmark]
    public void MaskEmail_V3()
    {
        foreach (var email in emails)
        {
            MaskEmailV3(email, 2, '*');
        }
    }


    [Benchmark]
    public void MaskEmail_V4()
    {
        foreach (var email in emails)
        {
            MaskEmailV4(email, 2, '*');
        }
    }

    [Benchmark]
    public void MaskEmail_V4_2()
    {
        foreach (var email in emails)
        {
            MaskEmailV4_2(email, 2, '*');
        }
    }

    [Benchmark]
    public void MaskEmail_V5()
    {
        foreach (var email in emails)
        {
            MaskEmailV5(email, 2, '*');
        }
    }

    [Benchmark]
    public void MaskEmail_V6()
    {
        foreach (var email in emails)
        {
            MaskEmailV6(email, 2, '*');
        }
    }

    [Benchmark]
    public void MaskEmail_V7()
    {
        foreach (var email in emails)
        {
            MaskEmailV7(email, 2, '*');
        }
    }





    public string MaskEmailV0(string email, int visibleCharacters, char maskChar)
    {
        var arr = email.Split('@');
        var firstPart = arr[0];
        var secondPart = arr[1];

        return firstPart.Substring(0, visibleCharacters) + new string(maskChar, firstPart.Length - visibleCharacters) + "@" + secondPart;
    }

    public string MaskEmailV1(string email, int visibleCharacters, char maskChar)
    {
        if (email == null || !email.Contains("@"))
        {
            return null;
        }

        string[] parts = email.Split('@');
        string localPart = parts[0];
        string domainPart = parts[1];

        // s@gmail.com
        if (localPart.Length <= visibleCharacters)
        {
            return email;
        }

        string maskedLocalPart = "";
        for (int i = 0; i < visibleCharacters; i++)
        {
            maskedLocalPart += localPart[i];
        }

        for (int i = visibleCharacters; i < localPart.Length; i++)
        {
            maskedLocalPart += maskChar;
        }

        return maskedLocalPart + "@" + domainPart;
    }

    public string MaskEmailV2(string email, int visibleCharacters, char maskChar)
    {
        if (email == null || !email.Contains("@"))
        {
            return null;
        }

        string[] parts = email.Split('@');
        string localPart = parts[0];
        string domainPart = parts[1];

        if (localPart.Length <= visibleCharacters)
        {
            return email;
        }

        StringBuilder maskedLocalPart = new StringBuilder();
        maskedLocalPart.Append(localPart.Substring(0, visibleCharacters));
        maskedLocalPart.Append(new string(maskChar, localPart.Length - visibleCharacters));

        return maskedLocalPart.ToString() + "@" + domainPart;
    }

    public string MaskEmailV2_2(string email, int visibleCharacters, char maskChar)
    {
        if (email == null || !email.Contains("@"))
        {
            return null;
        }

        string[] parts = email.Split('@');
        string localPart = parts[0];
        string domainPart = parts[1];

        if (localPart.Length <= visibleCharacters)
        {
            return email;
        }

        StringBuilder maskedLocalPart = new StringBuilder(capacity: email.Length);
        maskedLocalPart.Append(localPart.Substring(0, visibleCharacters));
        maskedLocalPart.Append(new string(maskChar, localPart.Length - visibleCharacters));

        return maskedLocalPart.ToString() + "@" + domainPart;
    }

    public string MaskEmailV3(string email, int visibleCharacters, char maskChar)
    {
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        int atIndex = email.IndexOf('@');
        if (atIndex == -1 || atIndex < visibleCharacters)
        {
            return email;
        }

        string localPart = email.Substring(0, atIndex);
        string domainPart = email.Substring(atIndex);

        StringBuilder maskedEmail = new StringBuilder(visibleCharacters + domainPart.Length);
        maskedEmail.Append(localPart.Substring(0, visibleCharacters));
        maskedEmail.Append(new string(maskChar, localPart.Length - visibleCharacters));
        maskedEmail.Append('@');
        maskedEmail.Append(domainPart);

        return maskedEmail.ToString();
    }

    public string MaskEmailV4(string email, int visibleCharacters, char maskChar)
    {
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        int atIndex = email.IndexOf('@');
        if (atIndex == -1 || atIndex <= visibleCharacters)
        {
            return email;
        }

        return string.Create(email.Length, email, (span, email) =>
        {
            email.AsSpan(0, visibleCharacters).CopyTo(span);
            new string(maskChar, atIndex - visibleCharacters).AsSpan().CopyTo(span.Slice(visibleCharacters));

            email.AsSpan(atIndex).CopyTo(span.Slice(visibleCharacters + atIndex - visibleCharacters));
        });
    }

    public string MaskEmailV4_2(string email, int visibleCharacters, char maskChar)
    {
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        int atIndex = email.IndexOf('@');
        if (atIndex == -1 || atIndex <= visibleCharacters)
        {
            return email;
        }

        return string.Create(email.Length, email, (span, email) =>
        {
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = i < visibleCharacters || i >= atIndex ? email[i] : maskChar;
            }
        });
    }

    public string MaskEmailV5(string email, int visibleCharacters, char maskChar)
    {
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        int atIndex = email.IndexOf('@');
        if (atIndex == -1 || atIndex <= visibleCharacters)
        {
            return email;
        }

        return string.Concat(
            email.AsSpan(0, visibleCharacters),
            new string(maskChar, atIndex - visibleCharacters),
            email.AsSpan(atIndex)
        );
    }

    public unsafe string MaskEmailV6(string email, int visibleCharacters, char maskChar)
    {
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        int atIndex = email.IndexOf('@');
        if (atIndex == -1 || atIndex <= visibleCharacters)
        {
            return email;
        }

        int totalLength = email.Length;
        string result = new string('\0', totalLength);  // create a string with null characters

        fixed (char* pEmail = email)
        fixed (char* pResult = result)
        {
            // first, copy the first visibleCharacters characters
            for (int i = 0; i < visibleCharacters; i++)
            {
                pResult[i] = pEmail[i];
            }

            // then, replace the rest with maskChar
            for (int i = visibleCharacters; i < atIndex; i++)
            {
                pResult[i] = maskChar;
            }

            // finally, copy the rest of the email
            for (int i = atIndex; i < totalLength; i++)
            {
                pResult[i] = pEmail[i];
            }
        }

        return result;
    }

    public string MaskEmailV7(string email, int visibleCharacters, char maskChar)
    {
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        int atIndex = email.IndexOf('@');
        if (atIndex == -1 || atIndex <= visibleCharacters)
        {
            return email;
        }

        int totalLength = email.Length;
        string result = new string('\0', totalLength);

        IntPtr pEmail = Marshal.StringToHGlobalUni(email);
        IntPtr pResult = Marshal.StringToHGlobalUni(result);

        try
        {
            // first, copy the first visibleCharacters characters
            for (int i = 0; i < visibleCharacters; i++)
            {
                Marshal.WriteInt16(pResult, i * 2, Marshal.ReadInt16(pEmail, i * 2));
            }

            // then, replace the rest with maskChar
            for (int i = visibleCharacters; i < atIndex; i++)
            {
                Marshal.WriteInt16(pResult, i * 2, maskChar);
            }

            // finally, copy the rest of the email
            for (int i = atIndex; i < totalLength; i++)
            {
                Marshal.WriteInt16(pResult, i * 2, Marshal.ReadInt16(pEmail, i * 2));
            }

            return Marshal.PtrToStringUni(pResult);
        }
        finally
        {
            Marshal.FreeHGlobal(pEmail);
            Marshal.FreeHGlobal(pResult);
        }
    }
}
