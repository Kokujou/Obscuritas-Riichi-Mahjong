using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules
{
    public static class RuleProvider
    {
        public static readonly List<MahjongRule> Rules = new List<MahjongRule>();

        static RuleProvider()
        {
            foreach (var type in
                Assembly.GetAssembly(typeof(MahjongRule)).GetTypes()
                    .Where(myType =>
                        myType.IsClass && !myType.IsAbstract &&
                        myType.IsSubclassOf(typeof(MahjongRule))))
                Rules.Add((MahjongRule) Activator.CreateInstance(type));
        }
    }
}