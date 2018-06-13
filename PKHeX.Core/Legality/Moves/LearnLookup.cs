﻿using System;
using System.Linq;
using static PKHeX.Core.Legal;

namespace PKHeX.Core
{
    /// <summary>
    /// LevelUp Lookup object
    /// </summary>
    public class LearnLookup
    {
        private readonly GameVersion Version;
        private readonly PersonalTable Table;
        private readonly Learnset[] Learn;
        public LearnLookup(PersonalTable table, Learnset[] learn, GameVersion version)
        {
            Version = version;
            Table = table;
            Learn = learn;

        }

        public LearnVersion GetIsLevelUp(int species, int form, int move)
        {
            int index = Table.GetFormeIndex(species, form);
            if (index <= 0)
                return LearnNONE;
            var lv = Learn[index].GetLevelLearnMove(move);
            if (lv >= 0)
                return new LearnVersion(lv, Version);
            return LearnNONE;
        }

        public LearnVersion GetIsLevelUp(int species, int form, int move, int max)
        {
            int index = Table.GetFormeIndex(species, form);
            if (index <= 0)
                return LearnNONE;
            var lv = Learn[index].GetLevelLearnMove(move);
            if (lv >= 0 && lv <= max)
                return new LearnVersion(lv, Version);
            return LearnNONE;
        }

        public LearnVersion GetIsLevelUpMin(int species, int move, int max, int min, int form = 0)
        {
            int index = Table.GetFormeIndex(species, form);
            if (index <= 0)
                return LearnNONE;
            var lv = Learn[index].GetLevelLearnMove(move, min);
            if (lv >= min && lv <= max)
                return new LearnVersion(lv, Version);
            return LearnNONE;
        }

        public LearnVersion GetIsLevelUpG1(int species, int move, int max, int min = 0)
        {
            int index = PersonalTable.RB.GetFormeIndex(species, 0);
            if (index == 0)
                return LearnNONE;

            // No relearner -- have to be learned on levelup
            var lv = Learn[index].GetLevelLearnMove(move, min);
            if (lv >= 0 && lv <= max)
                return new LearnVersion(lv, Version);

            if (min >= 1)
                return LearnNONE;

            var pi = (PersonalInfoG1)Table[index];
            var i = Array.IndexOf(pi.Moves, move);

            // Check if move was not overwritten by higher level moves before it was encountered
            if (i >= 0)
            {
                var unique = Learn[index].GetUniqueMovesLearned(pi.Moves.Where(z => z != 0), max);
                if (unique.Count - i <= 4)
                    return new LearnVersion(0, Version);
            }
            return LearnNONE;
        }
    }
}
