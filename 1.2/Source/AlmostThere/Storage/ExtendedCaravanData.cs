using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

//Note: Currently this class contains information specific for other mods (caravanMount, caravanRider, etc), which is of course not ideal for a core framework. Ideally it should be completely generic. However I have yet to come up with an
// way to do this properly without introducing a lot of extra work. So for now I'll just keep it as it is. 

namespace SearchAndDestroy.Storage
{
    public class ExtendedCaravanData : IExposable
    {
        public bool SD_enabled = false;
        public void ExposeData()
        {
            Scribe_Values.Look(ref SD_enabled, "SD_enabled", false);            
        }
    }
}
