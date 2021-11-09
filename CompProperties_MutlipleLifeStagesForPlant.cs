using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AnimaFruit
{
    public class CompProperties_MutlipleLifeStagesForPlant : CompProperties
    {
        public string GraphicsA;
        public string GraphicsB;
        public string GraphicsC;
        public string GraphicsD;

        public float StageB;
        public float StageC;
        public float StageD;

        public CompProperties_MutlipleLifeStagesForPlant()
        {
            compClass = typeof(CompMutlipleLifeStagesForPlant);
        }

    }
}
