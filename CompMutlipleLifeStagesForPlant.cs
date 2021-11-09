using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;

namespace AnimaFruit
{
    public class CompMutlipleLifeStagesForPlant : ThingComp
    {
        public Thing thingToGrab;
        public Plant plant => thingToGrab as Plant;
        public Graphic_Single newGraphicSingle;
        public bool reloading = false;
        public string newGraphicSinglePath = "";

        public CompProperties_MutlipleLifeStagesForPlant Props => (CompProperties_MutlipleLifeStagesForPlant)props;
        
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            thingToGrab = parent;
            reloading = true;
            LongEventHandler.ExecuteWhenFinished(ChangeGraphic);

        }
        public override void CompTickLong()
        {
            LongEventHandler.ExecuteWhenFinished(ChangeGraphic);
            parent.Map.mapDrawer.MapMeshDirty(parent.Position, MapMeshFlag.Things);
        }

        public void ChangeGraphic()
        {
            Vector2 drawSize = parent.Graphic.drawSize;
            Color color = parent.Graphic.color;
            ShaderTypeDef shaderType = parent.def.graphicData.shaderType;
            if (reloading)
            {
                if (newGraphicSinglePath == "")
                {
                    if (plant.Growth < Props.StageB)
                    {
                        newGraphicSinglePath = Props.GraphicsA;
                    }
                    else if (plant.Growth < Props.StageC)
                    {
                        newGraphicSinglePath = Props.GraphicsB;
                    }
                    else if (plant.Growth < Props.StageD)
                    {
                        newGraphicSinglePath = Props.GraphicsC;
                    }
                    else
                    {
                        newGraphicSinglePath = Props.GraphicsD;
                    }
                }
                reloading = false;
            }
            else
            {
                if (plant.Growth < Props.StageB)
                {
                    if (newGraphicSinglePath == Props.GraphicsA)
                    {
                        return;
                    }
                    newGraphicSinglePath = Props.GraphicsA;
                }
                else if (plant.Growth < Props.StageC)
                {
                    if (newGraphicSinglePath == Props.GraphicsB)
                    {
                        return;
                    }
                    newGraphicSinglePath = Props.GraphicsB;
                }
                else if (plant.Growth < Props.StageD)
                {
                    if (newGraphicSinglePath == Props.GraphicsC)
                    {
                        return;
                    }
                    newGraphicSinglePath = Props.GraphicsC;
                }
                else
                {
                    if (newGraphicSinglePath == Props.GraphicsD)
                    {
                        return;
                    }
                    newGraphicSinglePath = Props.GraphicsD;
                }
            }

            newGraphicSingle = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>(newGraphicSinglePath, shaderType.Shader, drawSize, color);
            Type typeFromHandle = typeof(Thing);
            FieldInfo field = typeFromHandle.GetField("graphicInt", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(thingToGrab, newGraphicSingle);
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref newGraphicSinglePath, "newGraphicSinglePath");
            Scribe_Values.Look(ref reloading, "reloading", defaultValue: false);
        }
    }
}
