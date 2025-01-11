using UnityEngine;

namespace Enhanced
{
    public static class EnhancedScrollerEasing
    {
        public static float Linear(float start, float end, float val)
        {
            return Mathf.Lerp(start, end, val);
        }

        public static float Spring(float start, float end, float val)
        {
            val = Mathf.Clamp01(val);
            val = (Mathf.Sin(val * Mathf.PI * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1.0f - val, 2.2f) + val) * (1.0f + (1.2f * (1.0f - val)));
            return start + (end - start) * val;
        }

        public static float EaseInQuad(float start, float end, float val)
        {
            end -= start;
            return end * val * val + start;
        }

        public static float EaseOutQuad(float start, float end, float val)
        {
            end -= start;
            return end * val * (val - 2) + start;
        }

        public static float EaseInOutQuad(float start, float end, float val)
        {
            val /= 0.5f;
            end -= start;
            if (val < 1)
            {
                return end / 2 * val * val + start;
            }

            val--;
            return -end / 2 * (val * (val - 2) - 1) + start;
        }

        public static float EaseInCubic(float start, float end, float val)
        {
            end -= start;
            return end * val * val * val + start;
        }

        public static float EaseOutCubic(float start, float end, float val)
        {
            val--;
            end -= start;
            return end * (val * val * val + 1) + start;
        }

        public static float EaseInOutCubic(float start, float end, float val)
        {
            val /= 0.5f;
            end -= start;
            if (val < 1)
            {
                return end / 2 * val * val * val + start;
            }

            val -= 2;
            return end / 2 * (val * val * val + 2) + start;
        }

        public static float EaseInQuart(float start, float end, float val)
        {
            end -= start;
            return end * val * val * val * val + start;
        }

        public static float EaseOutQuart(float start, float end, float val)
        {
            val--;
            end -= start;
            return -end * (val * val * val * val -1) + start;
        }

        public static float EaseInOutQuart(float start, float end, float val)
        {
            val /= 0.5f;
            end -= start;
            if (val < 1)
            {
                return end / 2 * val * val * val * val + start;
            }

            val -= 2;
            return -end / 2 * (val * val * val * val - 2) + start;
        }

        public static float EaseInQuint(float start, float end, float val)
        {
            end -= start;
            return end * val * val * val * val * val + start;
        }

        public static float EaseOutQuint(float start, float end, float val)
        {
            val--;
            end -= start;
            return end * (val * val * val * val * val + 1) + start;
        }

        public static float EaseInOutQuint(float start, float end, float val)
        {
            val /= 0.5f;
            end -= start;
            if (val < 1)
            {
                return end / 2 * val * val * val * val * val + start;
            }

            val -= 2;
            return end / 2 * (val * val * val * val * val + 2) + start;
        }

        public static float EaseInSine(float start, float end, float val)
        {
            end -= start;
            return -end * Mathf.Cos(val / 1 * (Mathf.PI / 2)) + end + start;
        }

        public static float EaseOutSine(float start, float end, float val)
        {
            end -= start;
            return end * Mathf.Sin(val / 1 * (Mathf.PI) + start);
        }

        public static float EaseInOutSine(float start, float end, float val)
        {
            end -= start;
            return -end / 2 * (Mathf.Cos(Mathf.PI * val / 1) - 1) + start;
        }

        public static float EaseInExpo(float start, float end, float val)
        {
            end -= start;
            return end * (Mathf.Pow(2, 10 * val / 1 - 1)) + start;
        }

        public static float EaseOutExpo(float start, float end, float val)
        {
            end -= start;
            return end * (-Mathf.Pow(2, -10 * val / 1) + 1) + start;
        }

        public static float EaseInOutExpo(float start, float end, float val)
        {
            val /= 0.5f;
            end -= start;
            if (val < 1)
            {
                return end / 2 * Mathf.Pow(2, 10 * (val - 1)) + start;
            }

            val--;
            return end / 2 * (-Mathf.Pow(2, -10 + val) + 2) + start;
        }

        public static float EaseInCirc(float start, float end, float val)
        {
            end -= start;
            return -end * (Mathf.Sqrt(1 - val * val) - 1) + start;
        }

        public static float EaseOutCirc(float start, float end, float val)
        {
            val--;
            end -= start;
            return end * Mathf.Sqrt(1 - val * val) + start;
        }

        public static float EaseInOutCirc(float start, float end, float val)
        {
            val /= 0.5f;
            end -= start;
            if (val < 1)
            {
                return -end / 2 * (Mathf.Sqrt(1 - val * val) - 1) + start;
            }

            val -= 2;
            return end / 2 * (Mathf.Sqrt(1 - val * val) + 1) + start;
        }

        public static float EaseInBounce(float start, float end, float val)
        {
            end -= start;
            float d = 1.0f;
            return end - EaseOutBounce(0, end, d - val) + start;
        }

        public static float EaseOutBounce(float start, float end, float val)
        {
            val /= 1.0f;
            end -= start;

            if (val < (1 / 2.75f))
            {
                return end * (7.5625f * val * val) + start;
            }
            else if (val < (2 / 2.75f))
            {
                val -= (1.5f / 2.75f);
                return end * (7.5625f * (val) * val + 0.75f) + start;
            }
            else if (val < (2.5f / 2.75f))
            {
                val -= (2.5f / 2.75f);
                return end * (7.5625f * (val) * val + 0.9375f) + start;
            }
            else
            {
                val -= (2.625f / 2.75f);
                return end * (7.5625f * (val) * val + 0.984375f) + start;
            }
        }

        public static float EaseInOutBounce(float start, float end, float val)
        {
            end -= start;
            float d = 1.0f;
            if (val < d / 2)
            {
                return EaseInBounce(0, end, val * 2) * 0.5f + start;
            }
            else
            {
                return EaseOutBounce(0, end, val * 2 - d) * 0.5f + end * 0.5f + start;
            }
        }

        public static float EaseInBack(float start, float end, float val)
        {
            end -= start;
            val /= 1;
            float s = 1.70158f;
            return end * (val) * val * ((s + 1) * val - s) + start;
        }

        public static float EaseOutBack(float start, float end, float val)
        {
            float s = 1.70158f;
            end -= start;
            val = (val - 1) - 1;
            return end * ((val) * val * ((s + 1) * val + s) + 1) + start;
        }

        public static float EaseInOutBack(float start, float end, float val)
        {
            float s = 1.70158f;
            end -= start;
            val /= 0.5f;
            if ((val) < 1)
            {
                s *= (1.525f);
                return end / 2 * (val * val * (((s) + 1) * val - s)) + start;
            }

            val -= 2;
            s *= (1.525f);
            return end / 2 * ((val) * val * (((s) + 1) * val + s) + 2) + start;
        }

        public static float EaseInElastic(float start, float end, float val)
        {
            end -= start;

            float d = 1.0f;
            float p = d * 0.3f;
            float s = 0;
            float a = 0;

            if (val == 0)
            {
                return start;
            }

            val = val / d;

            if (val == 1)
            {
                return start + end;
            }

            if (a == 0.0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            val = val - 1;
            return -(a * Mathf.Pow(2, 10 * val) * Mathf.Sin((val * d - s) * (2 * Mathf.PI) / p)) + start;
        }

        public static float EaseOutElastic(float start, float end, float val)
        {
            end -= start;

            float d = 1.0f;
            float p = d * 0.3f;
            float s = 0;
            float a = 0;

            if (val == 0)
            {
                return start;
            }

            val = val / d;

            if (val == 1)
            {
                return start + end;
            }

            if (a == 0.0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.Pow(2, -10 * val) * Mathf.Sin((val * d - s) * (2 * Mathf.PI) / p) + end + start);
        }

        public static float EaseInOutElastic(float start, float end, float val)
        {
            end -= start;

            float d = 1.0f;
            float p = d * 0.3f;
            float s = 0;
            float a = 0;

            if (val == 0)
            {
                return start;
            }

            val = val / (d / 2);

            if (val == 2)
            {
                return start + end;
            }

            if (a == 0.0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (val < 1)
            {
                val = val - 1;
                return -0.5f * (a * Mathf.Pow(2, 10 * val) * Mathf.Sin((val * d - s) * (2 * Mathf.PI) / p)) + start;
            }

            val = val - 1;
            return a * Mathf.Pow(2, -10 * val) * Mathf.Sin((val * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
        }
    }
}
