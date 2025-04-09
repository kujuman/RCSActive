using System.Collections.Generic;
using UnityEngine;

namespace HighlightDisabled
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class AppHighlightDisabled : MonoBehaviour
    {
        // keep it alive
        // AppHighlightDisabled() { DontDestroyOnLoad(this); }
        Vessel v;
        List<ModuleRCSFX> modules;
        Color disabledColor = XKCDColors.Red;
        Color enabledColor = XKCDColors.Green;

        void HighlightSinglePart(Color highlightC, Color edgeHighlightColor, Part p)
        {
            p.SetHighlightDefault();
            p.SetHighlightType(Part.HighlightType.AlwaysOn);
            p.SetHighlight(true, false);
            p.SetHighlightColor(highlightC);
            p.highlighter.ConstantOn(edgeHighlightColor);
            //p.highlighter.SeeThroughOn();
        }

        // called at every simulation step
        public void Update()
        {
            if (FlightGlobals.ActiveVessel != this.v)
            {
                v = FlightGlobals.ActiveVessel;

                this.modules = new List<ModuleRCSFX>();
                foreach (Part p in v.Parts)
                {
                    // Skip any parts with pods
                    if (p.FindModulesImplementing<ModuleCommand>().Count > 0)
                    {
                        continue;
                    }

                    var mi = p.FindModulesImplementing<ModuleRCSFX>();
                    foreach (ModuleRCSFX m in mi)
                    {
                        this.modules.Add(m);
                    }
                }
            }

            foreach (ModuleRCSFX m in this.modules)
            {
                if (!m.rcsEnabled | m.flameout | !m.rcsEnabled | m.thrustPercentage < 0.00001f)
                {
                    this.HighlightSinglePart(disabledColor, disabledColor, m.part);
                }
                else
                {
                    this.HighlightSinglePart(enabledColor, enabledColor, m.part);
                }
            }
            //print("hello world");
        }
    }
}