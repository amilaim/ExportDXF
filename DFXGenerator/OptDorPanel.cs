using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DFXGenerator.JasonHelper;
namespace DFXGenerator
{
    public class OptDorPanels
    {
        List<PNode> nodes;
        List<List<string>> csvDataList;
        public string ProjectName = "";

        string csvFilepath = "";
        public List<OptDorPanel> optDorPanels = new List<OptDorPanel>();
        bool SingleCut = false;

        bool cases = false;
        bool door = false;
        bool backpanel = false;

    }

    public class OptDorPanel
    {
        public int PanelId; //check ExportCoords folder in OptimaDor for panel numbers it starts with 1, so it is not optimador panel0 is caused here somewhere
        public double X;
        public double Y;
        public float PanelHeight;
        public float PanelWidth;
        public float PanelThickness;
        public int Shave;
        public List<OptDorPart> PartList = new List<OptDorPart>();
        public string ProjectName;
        public string FileName;
    }

    public sealed class OptDorPart
    {
        public int panelID { get; set; }
        public int modulID { get; set; }
        public int canvasID { get; set; }
        public int partID { get; set; }
        public int ID2 { get; set; }//partID2
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public bool Rotated { get; set; }
        public bool Reverse { get; set; }
        public string PartName { get; set; }

        public List<DorCircle> Circles = new List<DorCircle>();

        public string ProjectName { get; set; }

    }

    public sealed class DorCircle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
    }

    public static class NestingManager
    {
        public static List<OptDorPanel> GetNestingPanels(string ProjectName, float cutterOffset)
        {

            List<OptDorPanel> panelList = new List<OptDorPanel>();
            try
            {

                string nestedPath = Application.StartupPath + "\\app_resources\\CNC\\Manufacture\\OptimaDor\\v1\\ExportCoords\\" + ProjectName;

                if (!Directory.Exists(nestedPath))
                {
                    MessageBox.Show($"error. {nestedPath}", $"Proje file {ProjectName}not found in nesting.");
                    return panelList;
                }

                //works if name is int only , in OptimaDor we must export files as 1.txt 2.txt etc  then the order here is correct, otherwise if P1.txt, P2.txt  naming is used then P12.txt  comes before P2.txt
                List<string> fileList = Directory.GetFiles(nestedPath, "*.txt").OrderBy(n => Convert.ToInt16(Path.GetFileNameWithoutExtension(n))).ToList();

                if (fileList.Count == 0)
                {
                    MessageBox.Show(nestedPath, "NESTING YAPMADINIZ  !!!   NESTING YAPIN");//nesting not done
                    return panelList;
                }

                int PanelNumber = 1;

                foreach (string file in fileList)
                {
                    OptDorPanel panel = JsonSerialization.ReadFromFile<OptDorPanel>(file);
                    panel.PanelId = PanelNumber;
                    panel.ProjectName = ProjectName;
                    panel.FileName = Path.GetFileNameWithoutExtension(file);
                    panelList.Add(panel);

                    PanelNumber += 1;
                }

                return panelList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return panelList;
            }
        }
    }
}
