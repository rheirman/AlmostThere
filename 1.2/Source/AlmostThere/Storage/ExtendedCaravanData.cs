using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

//Note: Currently this class contains information specific for other mods (caravanMount, caravanRider, etc), which is of course not ideal for a core framework. Ideally it should be completely generic. However I have yet to come up with an
// way to do this properly without introducing a lot of extra work. So for now I'll just keep it as it is. 

namespace AlmostThere.Storage
{
    public class ExtendedCaravanData : IExposable
    {
        public bool fullyIgnoreRest = false;
        public bool forceRest = false;
        public bool almostThere = true;
        public void ExposeData()
        {
            Scribe_Values.Look(ref almostThere, "almostThere", false);            
            Scribe_Values.Look(ref fullyIgnoreRest, "fullyIgnoreRest", false);            
            Scribe_Values.Look(ref forceRest, "forceRest", false);            
        }
    }
}
