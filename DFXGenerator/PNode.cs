using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFXGenerator
{
    public class PNode
    {
        public int panelID { get; set; }
        public int PanelHeight { get; set; }
        public int PanelWidth { get; set; }
        public double PanelThickness { get; set; }
        public double CutterDiameter { get; set; }
        public int Shave { get; set; }
        public List<PanelPart> PartList { get; set; }
    }

    public class PanelPart
    {
        public int PanelId { get; set; }

        public long modulID { get; set; }

        public long partID { get; set; }

        public int ID2 { get; set; }

        public string partName { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int XA { get; set; }

        public int YA { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public bool Rotated { get; set; }

        public bool LeftMost { get; set; }

        public bool TopMost { get; set; }

        public bool RightMost { get; set; }

        public bool BottomMost { get; set; }

        public string PartType { get; set; }

        public bool Reverse { get; set; }

        public List<circle> Circles { get; set; }
    }

    public class circle
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Radius { get; set; }
    }
}
