using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HugsLib.Utils;
using RimWorld.Planet;
using Verse;

namespace AlmostThere.Storage
{


    public class ExtendedDataStorage : WorldComponent, IExposable
    {
        private Dictionary<int, ExtendedCaravanData> _store =
            new Dictionary<int, ExtendedCaravanData>();

        private List<int> _idWorkingList;

        private List<ExtendedCaravanData> _extendedPawnDataWorkingList;


        public ExtendedDataStorage(World world) : base(world)
        {
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(
                ref _store, "store",
                LookMode.Value, LookMode.Deep,
                ref _idWorkingList, ref _extendedPawnDataWorkingList);
        }

        // Return the associate extended data for a given Pawn, creating a new association
        // if required.
        public ExtendedCaravanData GetExtendedDataFor(Caravan caravan)
        {

            var id = caravan.ID;
            ExtendedCaravanData data = null;
            if (_store.TryGetValue(id, out data))
            {
                return data;
            }

            var newExtendedData = new ExtendedCaravanData();

            _store[id] = newExtendedData;
            return newExtendedData;
        }

        public void DeleteExtendedDataFor(Caravan caravan)
        {
            _store.Remove(caravan.ID);
        }
    }
}
