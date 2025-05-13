using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2
{
    public class CrossPlatformRandom
    {
        // 128-bit state
        private ulong _s0, _s1;

        // 생성자: 하나의 64비트 시드로부터 상태 초기화 (splitmix64 사용)
        public CrossPlatformRandom(ulong seed)
        {
            // splitmix64로 시드를 확장하여 두 개의 상태 변수 생성
            _s0 = SplitMix64(ref seed);
            _s1 = SplitMix64(ref seed);
        }
        public CrossPlatformRandom()
        {
        }

        public void setSeed(ulong seed)
        {
            // splitmix64로 시드를 확장하여 두 개의 상태 변수 생성
            _s0 = SplitMix64(ref seed);
            _s1 = SplitMix64(ref seed);
        }

        // splitmix64: 시드 인자로 받은 값을 섞어줍니다.
        private static ulong SplitMix64(ref ulong x)
        {
            ulong z = (x += 0x9E3779B97F4A7C15UL);
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return z ^ (z >> 31);
        }

        // XorShift128+ 핵심 생성기
        private ulong NextUInt64()
        {
            ulong s1 = _s0;
            ulong s0 = _s1;
            _s0 = s0;
            s1 ^= s1 << 23;
            _s1 = s1 ^ s0 ^ (s1 >> 17) ^ (s0 >> 26);
            return _s1 + s0;
        }

        // 0 ≤ x < int.MaxValue
        public int NextInt()
        {
            // 63비트 양수로 변환
            return (int)(NextUInt64() & 0x7FFF_FFFF_FFFF_FFFFUL);
        }

        // 0 ≤ x < maxValue
        public int Next(int maxValue)
        {
            return (int)(NextUInt64() % (ulong)maxValue);
        }

        // minValue ≤ x < maxValue
        public int Next(int minValue, int maxValue)
        {
            return minValue + Next(maxValue - minValue);
        }

        // 0.0 ≤ x < 1.0
        public double NextDouble()
        {
            // 53비트 정밀도
            const double inv = 1.0 / (1UL << 53);
            return (NextUInt64() >> 11) * inv;
        }

        // 바이트 배열 채우기
        public void NextBytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            int i = 0;
            while (i + 8 <= buffer.Length)
            {
                ulong v = NextUInt64();
                for (int j = 0; j < 8; j++)
                    buffer[i++] = (byte)(v >> (8 * j));
            }
            if (i < buffer.Length)
            {
                ulong v = NextUInt64();
                for (; i < buffer.Length; i++)
                    buffer[i] = (byte)(v >> (8 * (i % 8)));
            }
        }

        // 정규분포(평균0, 표준편차1) 난수: Box–Muller 변환
        private bool _hasSpare = false;
        private double _spare;
        public double NextGaussian(double mean = 0, double stdDev = 1)
        {
            if (_hasSpare)
            {
                _hasSpare = false;
                return _spare * stdDev + mean;
            }

            double u, v, s;
            do
            {
                u = NextDouble() * 2.0 - 1.0;
                v = NextDouble() * 2.0 - 1.0;
                s = u * u + v * v;
            } while (s >= 1.0 || s == 0.0);

            double mul = Math.Sqrt(-2.0 * Math.Log(s) / s);
            _spare = v * mul;
            _hasSpare = true;
            return mean + stdDev * (u * mul);
        }
    }
}
