# DXFGenerator

DXFGenerator is a **.NET-based DXF (AutoCAD Drawing Exchange Format) generation tool** that creates CAD drawings programmatically from structured input data.
It is designed for **automation scenarios** such as manufacturing layouts, CNC/laser cutting preparation, geometry exports, and batch DXF generation.

---

## Overview

DXFGenerator converts structured input (JSON / TXT / parameters) into valid DXF files with precise geometry, layers, and units.
The output DXF files can be opened in **AutoCAD**, **DraftSight**, **LibreCAD**, and other CAD viewers.

---

## Key Features

- Programmatic DXF file generation
- Support for common geometric entities:
  - Lines
  - Polylines
  - Rectangles
  - Circles
  - Text (optional)
- Layer management (name, color, line type)
- Unit handling (mm / inch)
- Batch processing support
- Deterministic output (same input → same DXF)
- Suitable for automation and integration workflows

---

## Technology Stack

- Language: C#
- Framework: .NET 6 / .NET 7 / .NET 8
- DXF Library: netDxf (or compatible DXF library)
- IDE: Visual Studio 2022+

---

## Project Structure

ExportDXF.sln
  -  DXFGenerator
  -  Properties            # Assembly info and project settings
  -  References            # External references
  -  app_resources/         # Application resources (icons, assets)
  -  JsonHelper/            # JSON parsing and helper utilities
      -  JsonSerialization.cs
  -  App.config             # Application configuration
  -  DXFManager.cs          # Core DXF creation and export logic
  -  Form1.cs               # Main Windows Forms UI
  -  OptDocPanel.cs         # Optional document/panel UI logic
  -  PNodes.cs              # Geometry/node data models
  -  Program.cs             # Application entry point
  -  packages.config        # NuGet package references

---

## Getting Started

### Prerequisites

- .NET SDK installed
- Visual Studio 2022 recommended

Check installation:
dotnet --version

---

## Build

dotnet build

---

## Run (Console App Example)

dotnet run --project src/DXFGenerator.App -- --input samples/input/sample.json --output samples/output/sample.dxf

---

## Input Example (JSON)

{
  "units": "mm",
  "layers": [
    { "name": "CUT", "color": 1 },
    { "name": "ANNOTATION", "color": 7 }
  ],
  "entities": [
    { "type": "line", "layer": "CUT", "x1": 0, "y1": 0, "x2": 100, "y2": 0 },
    { "type": "rectangle", "layer": "CUT", "x": 10, "y": 10, "width": 80, "height": 40 },
    { "type": "circle", "layer": "CUT", "x": 50, "y": 30, "radius": 10 }
  ]
}

---

## Output

Generated DXF files are written to the specified output path and are compatible with AutoCAD, LibreCAD, DraftSight, and Fusion 360.

---

## Testing

dotnet test

---

## License

Specify license here (MIT / Proprietary / Client-Owned).

---

## Maintainer

Amila Munasinghe
.NET Developer
CAD Software Integration Specialist 
