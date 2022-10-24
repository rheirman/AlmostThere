using HugsLib;
using HugsLib.Utils;
using AlmostThere.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using HugsLib.Settings;

namespace AlmostThere
{
    public class Base : ModBase
    {
        public override string ModIdentifier => "AlmostThere";
        public static Base Instance { get; private set; }
        private ExtendedDataStorage _extendedDataStorage;
        internal static SettingHandle<int> almostThereHours;

        public bool cachedResult = false;

        public Base()
        {
            Instance = this;
        }
        public ExtendedDataStorage GetExtendedDataStorage()
        {
            return _extendedDataStorage;
        }
        public override void WorldLoaded()
        {
            if (_extendedDataStorage == null)
            {
                _extendedDataStorage = Find.World.GetComponent<ExtendedDataStorage>();
            }
            base.WorldLoaded();
        }

        public override void DefsLoaded()
        {
            almostThereHours = Settings.GetHandle<int>("AlmostThereHours", "AT_AlmostThereHours_Title".Translate(), "AT_AlmostThereHours_Description".Translate(), 4, Validators.IntRangeValidator(0, 1000));
            base.DefsLoaded();
        }
    }


}
