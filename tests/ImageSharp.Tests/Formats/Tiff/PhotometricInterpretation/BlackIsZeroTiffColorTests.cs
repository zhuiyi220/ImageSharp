// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;

using SixLabors.ImageSharp.Formats.Experimental.Tiff;
using SixLabors.ImageSharp.PixelFormats;

using Xunit;

namespace SixLabors.ImageSharp.Tests.Formats.Tiff.PhotometricInterpretation
{
    public class BlackIsZeroTiffColorTests : PhotometricInterpretationTestBase
    {
        private static Rgba32 gray000 = new Rgba32(0, 0, 0, 255);
        private static Rgba32 gray128 = new Rgba32(128, 128, 128, 255);
        private static Rgba32 gray255 = new Rgba32(255, 255, 255, 255);
        private static Rgba32 gray0 = new Rgba32(0, 0, 0, 255);
        private static Rgba32 gray8 = new Rgba32(136, 136, 136, 255);
        private static Rgba32 grayF = new Rgba32(255, 255, 255, 255);
        private static Rgba32 bit0 = new Rgba32(0, 0, 0, 255);
        private static Rgba32 bit1 = new Rgba32(255, 255, 255, 255);

        private static readonly byte[] Bilevel_Bytes4x4 =
        {
            0b01010000,
            0b11110000,
            0b01110000,
            0b10010000
        };

        private static readonly Rgba32[][] Bilevel_Result4x4 = new[]
        {
            new[] { bit0, bit1, bit0, bit1 },
            new[] { bit1, bit1, bit1, bit1 },
            new[] { bit0, bit1, bit1, bit1 },
            new[] { bit1, bit0, bit0, bit1 }
        };

        private static readonly byte[] Bilevel_Bytes12x4 =
        {
            0b01010101, 0b01010000,
            0b11111111, 0b11111111,
            0b01101001, 0b10100000,
            0b10010000, 0b01100000
        };

        private static readonly Rgba32[][] Bilevel_Result12x4 =
        {
            new[] { bit0, bit1, bit0, bit1, bit0, bit1, bit0, bit1, bit0, bit1, bit0, bit1 },
            new[] { bit1, bit1, bit1, bit1, bit1, bit1, bit1, bit1, bit1, bit1, bit1, bit1 },
            new[] { bit0, bit1, bit1, bit0, bit1, bit0, bit0, bit1, bit1, bit0, bit1, bit0 },
            new[] { bit1, bit0, bit0, bit1, bit0, bit0, bit0, bit0, bit0, bit1, bit1, bit0 }
        };

        private static readonly byte[] Grayscale4_Bytes4x4 =
        {
            0x8F, 0x0F,
            0xFF, 0xFF,
            0x08, 0x8F,
            0xF0, 0xF8
        };

        private static readonly Rgba32[][] Grayscale4_Result4x4 =
        {
            new[] { gray8, grayF, gray0, grayF },
            new[] { grayF, grayF, grayF, grayF },
            new[] { gray0, gray8, gray8, grayF },
            new[] { grayF, gray0, grayF, gray8 }
        };

        private static readonly byte[] Grayscale4_Bytes3x4 =
        {
            0x8F, 0x00,
            0xFF, 0xF0,
            0x08, 0x80,
            0xF0, 0xF0
        };

        private static readonly Rgba32[][] Grayscale4_Result3x4 =
        {
            new[] { gray8, grayF, gray0 },
            new[] { grayF, grayF, grayF },
            new[] { gray0, gray8, gray8 },
            new[] { grayF, gray0, grayF }
        };

        private static readonly byte[] Grayscale8_Bytes4x4 =
        {
            128, 255, 000, 255,
            255, 255, 255, 255,
            000, 128, 128, 255,
            255, 000, 255, 128
        };

        private static readonly Rgba32[][] Grayscale8_Result4x4 =
        {
            new[] { gray128, gray255, gray000, gray255 },
            new[] { gray255, gray255, gray255, gray255 },
            new[] { gray000, gray128, gray128, gray255 },
            new[] { gray255, gray000, gray255, gray128 }
        };

        public static IEnumerable<object[]> Bilevel_Data
        {
            get
            {
                yield return new object[] { Bilevel_Bytes4x4, 1, 0, 0, 4, 4, Bilevel_Result4x4 };
                yield return new object[] { Bilevel_Bytes4x4, 1, 0, 0, 4, 4, Offset(Bilevel_Result4x4, 0, 0, 6, 6) };
                yield return new object[] { Bilevel_Bytes4x4, 1, 1, 0, 4, 4, Offset(Bilevel_Result4x4, 1, 0, 6, 6) };
                yield return new object[] { Bilevel_Bytes4x4, 1, 0, 1, 4, 4, Offset(Bilevel_Result4x4, 0, 1, 6, 6) };
                yield return new object[] { Bilevel_Bytes4x4, 1, 1, 1, 4, 4, Offset(Bilevel_Result4x4, 1, 1, 6, 6) };

                yield return new object[] { Bilevel_Bytes12x4, 1, 0, 0, 12, 4, Bilevel_Result12x4 };
                yield return new object[] { Bilevel_Bytes12x4, 1, 0, 0, 12, 4, Offset(Bilevel_Result12x4, 0, 0, 18, 6) };
                yield return new object[] { Bilevel_Bytes12x4, 1, 1, 0, 12, 4, Offset(Bilevel_Result12x4, 1, 0, 18, 6) };
                yield return new object[] { Bilevel_Bytes12x4, 1, 0, 1, 12, 4, Offset(Bilevel_Result12x4, 0, 1, 18, 6) };
                yield return new object[] { Bilevel_Bytes12x4, 1, 1, 1, 12, 4, Offset(Bilevel_Result12x4, 1, 1, 18, 6) };
            }
        }

        public static IEnumerable<object[]> Grayscale4_Data
        {
            get
            {
                yield return new object[] { Grayscale4_Bytes4x4, 4, 0, 0, 4, 4, Grayscale4_Result4x4 };
                yield return new object[] { Grayscale4_Bytes4x4, 4, 0, 0, 4, 4, Offset(Grayscale4_Result4x4, 0, 0, 6, 6) };
                yield return new object[] { Grayscale4_Bytes4x4, 4, 1, 0, 4, 4, Offset(Grayscale4_Result4x4, 1, 0, 6, 6) };
                yield return new object[] { Grayscale4_Bytes4x4, 4, 0, 1, 4, 4, Offset(Grayscale4_Result4x4, 0, 1, 6, 6) };
                yield return new object[] { Grayscale4_Bytes4x4, 4, 1, 1, 4, 4, Offset(Grayscale4_Result4x4, 1, 1, 6, 6) };

                yield return new object[] { Grayscale4_Bytes3x4, 4, 0, 0, 3, 4, Grayscale4_Result3x4 };
                yield return new object[] { Grayscale4_Bytes3x4, 4, 0, 0, 3, 4, Offset(Grayscale4_Result3x4, 0, 0, 6, 6) };
                yield return new object[] { Grayscale4_Bytes3x4, 4, 1, 0, 3, 4, Offset(Grayscale4_Result3x4, 1, 0, 6, 6) };
                yield return new object[] { Grayscale4_Bytes3x4, 4, 0, 1, 3, 4, Offset(Grayscale4_Result3x4, 0, 1, 6, 6) };
                yield return new object[] { Grayscale4_Bytes3x4, 4, 1, 1, 3, 4, Offset(Grayscale4_Result3x4, 1, 1, 6, 6) };
            }
        }

        public static IEnumerable<object[]> Grayscale8_Data
        {
            get
            {
                yield return new object[] { Grayscale8_Bytes4x4, 8, 0, 0, 4, 4, Grayscale8_Result4x4 };
                yield return new object[] { Grayscale8_Bytes4x4, 8, 0, 0, 4, 4, Offset(Grayscale8_Result4x4, 0, 0, 6, 6) };
                yield return new object[] { Grayscale8_Bytes4x4, 8, 1, 0, 4, 4, Offset(Grayscale8_Result4x4, 1, 0, 6, 6) };
                yield return new object[] { Grayscale8_Bytes4x4, 8, 0, 1, 4, 4, Offset(Grayscale8_Result4x4, 0, 1, 6, 6) };
                yield return new object[] { Grayscale8_Bytes4x4, 8, 1, 1, 4, 4, Offset(Grayscale8_Result4x4, 1, 1, 6, 6) };
            }
        }

        [Theory]
        [MemberData(nameof(Bilevel_Data))]
        [MemberData(nameof(Grayscale4_Data))]
        [MemberData(nameof(Grayscale8_Data))]
        public void Decode_WritesPixelData(byte[] inputData, int bitsPerSample, int left, int top, int width, int height, Rgba32[][] expectedResult)
        {
            AssertDecode(expectedResult, pixels =>
                {
                    new BlackIsZeroTiffColor<Rgba32>(new[] { (ushort)bitsPerSample }).Decode(inputData, pixels, left, top, width, height);
                });
        }

        [Theory]
        [MemberData(nameof(Bilevel_Data))]
        public void Decode_WritesPixelData_Bilevel(byte[] inputData, int bitsPerSample, int left, int top, int width, int height, Rgba32[][] expectedResult)
        {
            AssertDecode(expectedResult, pixels =>
                {
                    new BlackIsZero1TiffColor<Rgba32>().Decode(inputData, pixels, left, top, width, height);
                });
        }

        [Theory]
        [MemberData(nameof(Grayscale4_Data))]
        public void Decode_WritesPixelData_4Bit(byte[] inputData, int bitsPerSample, int left, int top, int width, int height, Rgba32[][] expectedResult)
        {
            AssertDecode(expectedResult, pixels =>
                {
                    new BlackIsZero4TiffColor<Rgba32>().Decode(inputData, pixels, left, top, width, height);
                });
        }

        [Theory]
        [MemberData(nameof(Grayscale8_Data))]
        public void Decode_WritesPixelData_8Bit(byte[] inputData, int bitsPerSample, int left, int top, int width, int height, Rgba32[][] expectedResult)
        {
            AssertDecode(expectedResult, pixels =>
                {
                    new BlackIsZero8TiffColor<Rgba32>().Decode(inputData, pixels, left, top, width, height);
                });
        }
    }
}