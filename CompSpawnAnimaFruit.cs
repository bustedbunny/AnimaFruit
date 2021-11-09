using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AnimaFruit
{
    class CompSpawnAnimaFruit : ThingComp
    {
        private int nextSubplantTick;
        public CompProperties_SpawnSubplant Props => (CompProperties_SpawnSubplant)props;
        public override void CompTick()
        {
            if (Find.TickManager.TicksGame >= nextSubplantTick)
            {
                DoGrowAnimaFruit();
                SetupNextSubplantTick();
            }
        }
        public void SetupNextSubplantTick()
        {
            nextSubplantTick = Find.TickManager.TicksGame + (int)(60000f * Props.subplantSpawnDays);
        }
        public void DoGrowAnimaFruit(bool force = false)
        {
            if (!force && ((Plant)parent).Growth < Props.minGrowthForSpawn)
            {
                return;
            }
            IntVec3 position = parent.Position;
            int num = GenRadial.NumCellsInRadius(Props.maxRadius);
            int numberofplants = 0;
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = position + GenRadial.RadialPattern[i];
                if (!intVec.InBounds(parent.Map) || !WanderUtility.InSameRoom(position, intVec, parent.Map))
                {
                    continue;
                }
                
                bool flag = false;
                List<Thing> thingList = intVec.GetThingList(parent.Map);
                foreach (Thing item in thingList)
                {
                    if (item.def == Props.subplant)
                    {
                        numberofplants++;
                        flag = true;
                        break;
                    }
                    if (Props.plantsToNotOverwrite.NullOrEmpty())
                    {
                        continue;
                    }
                    for (int j = 0; j < Props.plantsToNotOverwrite.Count; j++)
                    {
                        if (item.def == Props.plantsToNotOverwrite[j])
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    continue;
                }
                if (!Props.canSpawnOverPlayerSownPlants)
                {
                    Plant plant = intVec.GetPlant(parent.Map);
                    Zone zone = parent.Map.zoneManager.ZoneAt(intVec);
                    if (plant != null && plant.sown && zone != null && zone is Zone_Growing)
                    {
                        continue;
                    }
                }
                if (!Props.subplant.CanEverPlantAt(intVec, parent.Map, canWipePlantsExceptTree: true))
                {
                    continue;
                }
                if (numberofplants >= Props.maxPlants)
                {
                    return;
                }
                for (int num2 = thingList.Count - 1; num2 >= 0; num2--)
                {
                    if (thingList[num2].def.category == ThingCategory.Plant)
                    {
                        thingList[num2].Destroy();
                    }
                }
                Plant plant2 = (Plant)GenSpawn.Spawn(Props.subplant, intVec, parent.Map);
                if (Props.initialGrowthRange.HasValue)
                {
                    plant2.Growth = Props.initialGrowthRange.Value.RandomInRange;
                }
                break;
            }
        }
    }
}
