namespace Bobo
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ SuppressMessage( "ReSharper", "ClassNeverInstantiated.Global" ) ]
    public class CharacterMapper
    {
        public static int GetCharacterId(char character)
        {
            return character switch
            {
                '1' or 'l' or 'I' or 'i' => 1,
                '2' => 2,
                '3' => 3,
                '4' => 4,
                '5' => 5,
                '6' => 6,
                '7' => 7,
                '8' or 'B' => 8,
                '9' => 9,
                '0' or 'O' or 'o' or 'D' => 10,
                'A' => 11,
                'a' => 12,
                'b' => 13,
                'C' or 'c' => 14,
                'd' => 15,
                'e' => 16,
                'E' => 17,
                'f' => 18,
                'F' => 19,
                'g' => 20,
                'G' => 21,
                'H' => 22,
                'h' => 23,
                'j' or 'J' => 24,
                'k' or 'K' => 25,
                'L' => 26,
                'm' => 27,
                'M' => 28,
                'n' => 29,
                'N' => 30,
                'P' or 'p' => 31,
                'q' => 32,
                'Q' => 33,
                'R' => 34,
                'r' => 35,
                'S' or 's' => 36,
                't' => 37,
                'T' => 38,
                'U' or 'u' => 39,
                'V' or 'v' => 40,
                'W' or 'w' => 41,
                'X' or 'x' => 42,
                'Y' or 'y' => 43,
                'Z' or 'z' => 44,
                '\\' or '⟍' or '⧵' or '╲' => 45,
                '/' or '⟋' or '⁄' or '╱' => 46,
                '-' or '_' => 47,
                '<' or '‹' => 48,
                '>' or '›' => 49,
                '.' or '⬥' => 50,
                _ => 0
            };
        }

        public static char GetCharacter(int characterId)
        {
            return characterId switch
            {
                1 => 'I',
                2 => '2',
                3 => '3',
                4 => '4',
                5 => '5',
                6 => '6',
                7 => '7',
                8 => 'B',
                9 => '9',
                10 => 'O',
                11 => 'A',
                12 => 'a',
                13 => 'b',
                14 => 'C',
                15 => 'd',
                16 => 'e',
                17 => 'E',
                18 => 'f',
                19 => 'F',
                20 => 'g',
                21 => 'G',
                22 => 'H',
                23 => 'h',
                24 => 'J',
                25 => 'K',
                26 => 'L',
                27 => 'm',
                28 => 'M',
                29 => 'n',
                30 => 'N',
                31 => 'P',
                32 => 'q',
                33 => 'Q',
                34 => 'R',
                35 => 'r',
                36 => 'S',
                37 => 't',
                38 => 'T',
                39 => 'U',
                40 => 'V',
                41 => 'W',
                42 => 'X',
                43 => 'Y',
                44 => 'Z',
                45 => '\\',
                46 => '/',
                47 => '-',
                48 => '<',
                49 => '>',
                50 => '.',
                _ => '*'
            };
        }

        public static bool IsGrouped( char character )
        {
            return character is '0' or 'O' or 'o' or 'D' or '1' or 'l' or 'I' or 'i' or '8' or 'B'
                or 'C' or 'c' or 'j' or 'J' or 'k' or 'K' or 'P' or 'p' or 'S' or 's' or 'U' or 'u'
                or 'V' or 'v' or 'W' or 'w' or 'X' or 'x' or 'Y' or 'y' or 'Z' or 'z' or '\\' or '⟍'
                or '⧵' or '╲' or '/' or '⟋' or '⁄' or '╱' or '-' or '_' or '<' or '‹' or '.' or '⬥';
        }

        public static char[ ] GetGroup( char character )
        {
            return character switch
            {
                '0' or 'O' or 'o' => new[ ]
                {
                    '0',
                    'O',
                    'o'
                },
                '1' or 'l' or 'I' or 'i' => new[ ]
                {
                    '1',
                    'l',
                    'I',
                    'i'
                },
                '2' => new[ ]
                {
                    '2'
                },
                '3' => new[ ]
                {
                    '3'
                },
                '4' => new[ ]
                {
                    '4'
                },
                '5' => new[ ]
                {
                    '5'
                },
                '6' => new[ ]
                {
                    '6'
                },
                '7' => new[ ]
                {
                    '7'
                },
                '8' or 'B' => new[ ]
                {
                    '8',
                    'B'
                },
                '9' => new[ ]
                {
                    '9'
                },
                'A' => new[ ]
                {
                    'A'
                },
                'a' => new[ ]
                {
                    'a'
                },
                'b' => new[ ]
                {
                    'b'
                },
                'C' or 'c' => new[ ]
                {
                    'C',
                    'c'
                },
                'd' => new[ ]
                {
                    'd'
                },
                'e' => new[ ]
                {
                    'e'
                },
                'E' => new[ ]
                {
                    'E'
                },
                'f' => new[ ]
                {
                    'f'
                },
                'F' => new[ ]
                {
                    'F'
                },
                'g' => new[ ]
                {
                    'g'
                },
                'G' => new[ ]
                {
                    'G'
                },
                'H' => new[ ]
                {
                    'H'
                },
                'h' => new[ ]
                {
                    'h'
                },
                'j' or 'J' => new[ ]
                {
                    'j',
                    'J'
                },
                'k' or 'K' => new[ ]
                {
                    'k',
                    'K'
                },
                'L' => new[ ]
                {
                    'L'
                },
                'm' => new[ ]
                {
                    'm'
                },
                'M' => new[ ]
                {
                    'M'
                },
                'n' => new[ ]
                {
                    'n'
                },
                'N' => new[ ]
                {
                    'N'
                },
                'P' or 'p' => new[ ]
                {
                    'P',
                    'p'
                },
                'q' => new[ ]
                {
                    'q'
                },
                'Q' => new[ ]
                {
                    'Q'
                },
                'R' => new[ ]
                {
                    'R'
                },
                'r' => new[ ]
                {
                    'r'
                },
                'S' or 's' => new[ ]
                {
                    'S',
                    's'
                },
                't' => new[ ]
                {
                    't'
                },
                'T' => new[ ]
                {
                    'T'
                },
                'U' or 'u' => new[ ]
                {
                    'U',
                    'u'
                },
                'V' or 'v' => new[ ]
                {
                    'V',
                    'v'
                },
                'W' or 'w' => new[ ]
                {
                    'W',
                    'w'
                },
                'X' or 'x' => new[ ]
                {
                    'X',
                    'x'
                },
                'Y' or 'y' => new[ ]
                {
                    'Y',
                    'y'
                },
                'Z' or 'z' => new[ ]
                {
                    'z',
                    'Z'
                },
                '\\' or '⟍' or '⧵' or '╲' => new[ ]
                {
                    '\\',
                    '⟍',
                    '⧵',
                    '╲'
                },
                '/' or '⟋' or '⁄' or '╱' => new[ ]
                {
                    '/',
                    '⟋',
                    '⁄',
                    '╱'
                },
                '-' or '_' => new[ ]
                {
                    '-',
                    '_'
                },
                '<' or '‹' => new[ ]
                {
                    '<',
                    '‹'
                },
                '>' or '›' => new[ ]
                {
                    '<',
                    '‹'
                },
                '.' or '⬥' => new[ ]
                {
                    '.',
                    '⬥'
                },
                _ => Array.Empty<char>( )
            };
        }

        public static bool IsAmbiguous(char character1, char character2)
        {
            return GetCharacterId( character1 ) == GetCharacterId( character2 );
        }

        public static bool ValidateCharacterByPixelCount(char character, int size)
        {
            return character switch
            {
                'A' => size > 68 && size < 218,
                'B' => size > 94 && size < 252,
                'C' => size > 55 && size < 207,
                'D' => size > 84 && size < 233,
                'E' => size > 58 && size < 226,
                'F' => size > 54 && size < 202,
                'G' => size > 68 && size < 232,
                'H' => size > 72 && size < 281,
                'I' => size > 20 && size < 157,
                'J' => size > 31 && size < 158,
                'K' => size > 70 && size < 234,
                'L' => size > 43 && size < 179,
                'M' => size > 68 && size < 291,
                'N' => size > 74 && size < 251,
                'O' => size > 69 && size < 232,
                'P' => size > 73 && size < 212,
                'Q' => size > 70 && size < 225,
                'R' => size > 84 && size < 257,
                'S' => size > 67 && size < 204,
                'T' => size > 44 && size < 191,
                'U' => size > 62 && size < 220,
                'V' => size > 62 && size < 199,
                'W' => size > 77 && size < 228,
                'X' => size > 70 && size < 222,
                'Y' => size > 60 && size < 187,
                'Z' => size > 57 && size < 213,
                'a' => size > 105 && size < 286,
                'b' => size > 61 && size < 188,
                'c' => size > 75 && size < 246,
                'd' => size > 61 && size < 208,
                'e' => size > 103 && size < 279,
                'f' => size > 27 && size < 136,
                'g' => size > 81 && size < 217,
                'h' => size > 59 && size < 187,
                'i' => size > 49 && size < 210,
                'j' => size > 29 && size < 114,
                'k' => size > 59 && size < 199,
                'l' => size > 20 && size < 116,
                'm' => size > 52 && size < 286,
                'n' => size > 90 && size < 300,
                'o' => size > 106 && size < 263,
                'p' => size > 63 && size < 201,
                'q' => size > 66 && size < 211,
                'r' => size > 51 && size < 211,
                's' => size > 80 && size < 254,
                't' => size > 40 && size < 153,
                'u' => size > 92 && size < 285,
                'v' => size > 76 && size < 228,
                'w' => size > 82 && size < 258,
                'x' => size > 88 && size < 270,
                'y' => size > 51 && size < 180,
                'z' => size > 67 && size < 246,
                '0' => size > 55 && size < 234,
                '1' => size > 35 && size < 182,
                '2' => size > 58 && size < 240,
                '3' => size > 52 && size < 184,
                '4' => size > 57 && size < 196,
                '5' => size > 57 && size < 197,
                '6' => size > 68 && size < 207,
                '7' => size > 47 && size < 153,
                '8' => size > 70 && size < 235,
                '9' => size > 67 && size < 203,
                '\\' => size > 18 && size < 205,
                '/' => size > 20 && size < 116,
                '-' => size > 16 && size < 215,
                '_' => size > 16 && size < 155,
                '⟋' => size > 30 && size < 75,
                '⟍' => size > 40 && size < 89,
                '⧵' => size > 24 && size < 83,
                '⁄' => size > 27 && size < 138,
                '╱' => size > 19 && size < 90,
                '╲' => size > 17 && size < 107,
                '⬥' => size > 163 && size < 218,
                '.' => size > 71 && size < 300,
                '<' => size > 34 && size < 218,
                '>' => size > 31 && size < 224,
                '‹' => size > 51 && size < 187,
                '›' => size > 52 && size < 188,
                ' ' => size > -1 && size < 1,
                _ => false,
            };
        }

        public static bool ValidateCharacterIdByPixelCount(int characterId, int size)
        {
            return characterId switch
            {
                1 => size > 20 && size < 210,
                2 => size > 58 && size < 240,
                3 => size > 52 && size < 184,
                4 => size > 57 && size < 196,
                5 => size > 57 && size < 197,
                6 => size > 68 && size < 207,
                7 => size > 47 && size < 153,
                8 => size > 70 && size < 252,
                9 => size > 67 && size < 203,
                10 => size > 55 && size < 263,
                11 => size > 68 && size < 218,
                12 => size > 105 && size < 286,
                13 => size > 61 && size < 188,
                14 => size > 55 && size < 246,
                15 => size > 61 && size < 208,
                16 => size > 103 && size < 279,
                17 => size > 58 && size < 226,
                18 => size > 27 && size < 136,
                19 => size > 54 && size < 202,
                20 => size > 81 && size < 217,
                21 => size > 68 && size < 232,
                22 => size > 72 && size < 281,
                23 => size > 59 && size < 187,
                24 => size > 29 && size < 158,
                25 => size > 59 && size < 234,
                26 => size > 43 && size < 179,
                27 => size > 52 && size < 286,
                28 => size > 68 && size < 291,
                29 => size > 90 && size < 300,
                30 => size > 74 && size < 251,
                31 => size > 63 && size < 212,
                32 => size > 66 && size < 211,
                33 => size > 70 && size < 225,
                34 => size > 84 && size < 257,
                35 => size > 51 && size < 211,
                36 => size > 67 && size < 254,
                37 => size > 40 && size < 153,
                38 => size > 44 && size < 191,
                39 => size > 62 && size < 285,
                40 => size > 62 && size < 228,
                41 => size > 77 && size < 258,
                42 => size > 70 && size < 270,
                43 => size > 51 && size < 187,
                44 => size > 57 && size < 246,
                45 => size > 17 && size < 205,
                46 => size > 19 && size < 138,
                47 => size > 16 && size < 215,
                48 => size > 34 && size < 218,
                49 => size > 34 && size < 218,
                50 => size > 71 && size < 300,
                _ => false,
            };
        }
    }
}
