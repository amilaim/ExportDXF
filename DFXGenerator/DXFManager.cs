using netDxf;
using netDxf.Entities;
using netDxf.Header;
using netDxf.Tables;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DFXGenerator
{
    public class DXFManager
    {
        private List<OptDorPanel> _OptimaCabinets = null;
        public DXFManager(List<OptDorPanel> OptimaCabinets)
        {
            _OptimaCabinets = OptimaCabinets;
        }

        public  void GenerateScriptPanels(string fileName)
        {
            if (_OptimaCabinets == null) throw new ArgumentNullException("panels");

            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("Invalid file path.", "filePath");
            
            foreach (var panel in _OptimaCabinets)
            {
                string scriptFilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                DxfVersion dxfVersion = DxfVersion.AutoCad2018;

                var document = new DxfDocument(dxfVersion);

                // Layers
                var layerPanel = new Layer("PANEL") { Color = new AciColor(2) }; // yellow
                var layerParts = new Layer("PARTS") { Color = new AciColor(4) }; // cyan
                var layerText = new Layer("TEXT") { Color = new AciColor(7) }; // white

                // Simple layout: place each panel to the right with a gap
                double gap = 100; // mm
                double offsetX = 0;
                double offsetY = 0;

                // Panel border
                var panelRect = MakeRectanglePolyline(offsetX, offsetY, panel.PanelWidth, panel.PanelHeight, layerPanel, true);
                document.AddEntity(panelRect);

                // Panel label
                var pText = new Text("Panel " + panel.PanelId, new Vector2(offsetX + 10, offsetY + panel.PanelHeight + 20), 10);
                pText.Layer = layerText;
                document.AddEntity(pText);

                // Parts
                if (panel.PartList != null)
                {
                    for (int i = 0; i < panel.PartList.Count; i++)
                    {
                        var part = panel.PartList[i];
                        if (part == null) continue;
                        if (part.Width <= 0 || part.Height <= 0) continue;

                        //double w = part.Rotated ? part.Width : part.Height;
                        //double h = part.Rotated ? part.Height : part.Width;

                        double w = part.Width;
                        double h = part.Height;

                        // Assume X,Y is bottom-left of part
                        double x = offsetX + part.X;
                        double y = offsetY + part.Y;

                        var rect = MakeRectanglePolyline(x, y, w, h, layerParts, true);
                        document.AddEntity(rect);

                        var label = (part.PartName ?? "") + " (" + part.partID + ")";
                        var t = new Text(label, new Vector2(x + 5, y + 5), 8);
                        t.Layer = layerText;
                        document.AddEntity(t);

                        if (part.Reverse)
                        {
                            var rev = new Text("REV", new Vector2(x + 5, y + h - 12), 8);
                            rev.Layer = layerText;
                            document.AddEntity(rev);
                        }

                        // ---- CIRCLES ----
                        if (part.Circles != null)
                        {
                            for (int c = 0; c < part.Circles.Count; c++)
                            {
                                var circle = part.Circles[c];
                                if (circle == null) continue;

                                double cx = circle.X;
                                double cy = circle.Y;

                                // Handle rotation (90 degrees clockwise around part origin)
                                if (part.Rotated)
                                {
                                    double temp = cx;
                                    cx = h - cy;
                                    cy = temp;
                                }

                                // Handle reverse (mirror horizontally)
                                if (part.Reverse)
                                {
                                    cx = w - cx;
                                }

                                // Convert to absolute coordinates
                                double absX = x + cx;
                                double absY = y + cy;

                                var dxfCircle = new netDxf.Entities.Circle(
                                    new Vector2(absX, absY),
                                    circle.Radius
                                );
                                dxfCircle.Color = new AciColor(2);
                                dxfCircle.Layer = layerParts;

                                // IMPORTANT: use Entities.Add (more compatible)
                                document.AddEntity(dxfCircle);
                            }
                        }

                    }
                }

                // Move next panel to the right
                offsetX += panel.PanelWidth + gap;

                scriptFilePath = Path.Combine(scriptFilePath, panel.ProjectName);

                if (!Directory.Exists(scriptFilePath))
                    Directory.CreateDirectory(scriptFilePath);

                scriptFilePath += "\\" + panel.FileName + ".dxf";

                document.Save(scriptFilePath);
            }

            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //savePath = Path.Combine(savePath, panel.ProjectName);

            MessageBox.Show($"OptCAM Procedure was saved to {savePath}",
                "Saved Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static LwPolyline MakeRectanglePolyline(double x, double y, double w, double h, Layer layer, bool closed)
        {
            var pl = new LwPolyline();
            pl.Layer = layer;
            pl.IsClosed = closed;

            pl.Vertexes.Add(new LwPolylineVertex(x, y));
            pl.Vertexes.Add(new LwPolylineVertex(x + w, y));
            pl.Vertexes.Add(new LwPolylineVertex(x + w, y + h));
            pl.Vertexes.Add(new LwPolylineVertex(x, y + h));

            return pl;
        }
    }
}
