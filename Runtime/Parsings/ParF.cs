using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Parsings
{
    public static class ParF
    {
        public static readonly Func<string, string> TrimStart = src => src.TrimStart();

        public static readonly Func<string, Output<string>> GetScope = src =>
        {
            var regex = new Regex("^[{]([^{}]*)[}]");
            var match = regex.Match(src);
            if (!match.Success) return Output<string>.Fail(src);
            var value     = match.Groups[1].Value;
            var remaining = src.Remove(0, match.Length);
            return new Output<string>
            {
                value     = value,
                remaining = remaining
            };
        };

        public static readonly Func<string, Output<string>> GetClassName = src =>
        {
            var regex = new Regex("^[.]([a-zA-Z]+)");
            var match = regex.Match(src);
            if (!match.Success) return Output<string>.Fail(src);
            var value     = match.Groups[1].Value;
            var remaining = src.Remove(0, match.Length);
            return new Output<string>
            {
                value     = value,
                remaining = remaining
            };
        };

        public static readonly Func<string, Output<CssProp>> GetProperty = src =>
        {
            var regex = new Regex(@"^([A-Za-z]+)\:([^:;]+)\;");
            var match = regex.Match(src);
            if (!match.Success) return Output<CssProp>.Fail(src);
            var name      = match.Groups[1].Value;
            var value     = match.Groups[2].Value.Trim();
            var remaining = src.Remove(0, match.Length);
            var prop = new CssProp
            {
                name  = name,
                value = value,
            };
            return new Output<CssProp>
            {
                value     = prop,
                remaining = remaining,
            };
        };

        public static readonly Func<string, Output<CssClass>> GetClass = src =>
        {
            var name  = string.Empty;
            var props = new List<CssProp>();
            var clss = src.Pipe(GetClassName).Inject(n => name = n.value)
               .remaining.Pipe(TrimStart).Pipe(GetScope).Inject(scope =>
                {
                    var rest = scope.value.Pipe(TrimStart);
                    for (;;)
                    {
                        var prop = rest.Pipe(GetProperty);
                        if (prop.isRejected) break;
                        props.Add(prop.value);
                        rest = prop.remaining.Pipe(TrimStart);
                    }
                });
            var isRejected = clss.isRejected;
            return new Output<CssClass>
            {
                isRejected = isRejected,
                remaining = isRejected ? src : clss.remaining,
                value = isRejected ? default : new CssClass
                {
                    name  = name,
                    props = props,
                }
            };
        };

        private static string Escape(this string src)
        {
            var regex = new Regex(@"[\x00-\x1F\x7F]");
            return regex.Replace(src, m => $"\\x{(int)m.Value[0]:X2}");
        }
    }

    public static class FuncExts
    {
        public static B Apply<A, B>(this A a, Func<A, B> func)
        {
            return func(a);
        }

        public static Func<A, B> Inject<A, B>(this Func<A, B> p, Action<B> i)
        {
            return a =>
            {
                var b = p(a);
                i(b);
                return b;
            };
        }

        public static Func<B> Bind<A, B>(this A a, Func<A, B> p)
        {
            return () => p(a);
        }

        public static Func<A, C> Bind<A, B, C>(this Func<A, B> p, Func<B, C> q)
        {
            return a => q(p(a));
        }

        public static Func<A, Output<B>> Log<A, B>(this Func<A, Output<B>> p)
        {
            return a =>
            {
                var b = p(a);
                Debug.Log(b.value);
                return b;
            };
        }

        public static Func<A, string> Remaining<A, B>(this Func<A, Output<B>> p)
        {
            return a =>
            {
                var b = p(a);
                return b.remaining;
            };
        }

        public static B Pipe<A, B>(this A a, Func<A, B> p)
        {
            return p(a);
        }

        public static A Inject<A>(this A a, Action<A> p)
        {
            p(a);
            return a;
        }
    }
}